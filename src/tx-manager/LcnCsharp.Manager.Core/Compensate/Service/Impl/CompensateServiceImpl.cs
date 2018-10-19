using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DotNetty.Codecs.Base64;
using LcnCsharp.Common.Exception;
using LcnCsharp.Common.Logging;
using LcnCsharp.Common.Utils.Task;
using LcnCsharp.Manager.Core.Compensate.Dao;
using LcnCsharp.Manager.Core.Compensate.Model;
using LcnCsharp.Manager.Core.Config;
using LcnCsharp.Manager.Core.Manager;
using LcnCsharp.Manager.Core.Manager.Service;
using LcnCsharp.Manager.Core.Model;
using LcnCsharp.Manager.Core.Netty.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LcnCsharp.Manager.Core.Compensate.Service.Impl
{
    public class CompensateServiceImpl:ICompensateService
    {
        private readonly ILogger
            _logger = LcnCsharpLogManager.LoggerFactory.CreateLogger(typeof(CompensateServiceImpl));

        private readonly ICompensateDao _compensateDao;
        private readonly ConfigReader _configReader;
        private ITxManagerSenderService _managerSenderService;
        private ITxManagerService _txManagerService;

        public bool SaveCompensateMsg(TransactionCompensateMsg transactionCompensateMsg)
        {
            TxGroup txGroup = _txManagerService.GetTxGroup(transactionCompensateMsg.GroupId);
            if (txGroup == null)
            {
                //仅发起方异常，其他模块正常
                txGroup = new TxGroup
                {
                    NowTime = DateTime.Now.Millisecond,
                    GroupId = transactionCompensateMsg.GroupId,
                    IsCompensate = 1
                };
            }
            else
            {
                _txManagerService.DeleteTxGroup(txGroup);
            }

            transactionCompensateMsg.txGroup=txGroup;

            string json = JsonConvert.SerializeObject(transactionCompensateMsg);

            _logger.LogInformation($"Compensate->{json}");

             string compensateKey = _compensateDao.SaveCompensateMsg(transactionCompensateMsg);

            //调整自动补偿机制，若开启了自动补偿，需要通知业务返回success，方可执行自动补偿
            new System.Threading.Tasks.Task(() =>
                {
                    try
                    {
                        var groupId = transactionCompensateMsg.GroupId;
                        JObject requestJson = new JObject()
                        {

                            {"action","compensate" },
                            {"groupId",groupId },
                            {"json",json }

                        };

                        var url = _configReader.CompensateNotifyUrl;
                        _logger.LogError($"Compensate Callback Address->{url}");
                        //var res = HttpUtils.postJson(url, requestJson.toJSONString());
                        var res = "";
                        _logger.LogError($"Compensate Callback Result->{res}");
                        if (_configReader.IsCompensateAuto)
                        {
                            //自动补偿,是否自动执行补偿
                            if (res.Contains("success") || res.Contains("SUCCESS"))
                            {
                                //自动补偿
                                AutoCompensate(compensateKey, transactionCompensateMsg);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Compensate Callback Fails->{e.Message}");
                    }
                    
                    
                });

            return string.IsNullOrEmpty(compensateKey);
        }

        public List<ModelName> LoadModelList()
        {
            var keys = _compensateDao.LoadCompensateKeys();

            var models = new Dictionary<string,int>();

            foreach (var key in keys)
            {
                if (key.Length > 36)
                {
                    var name = key.Substring(11, key.Length - 25);
                    int v = 1;
                    if (models.ContainsKey(name))
                    {
                        v = models[name] + 1;
                    }
                    models.Add(name, v);
                }
            }
            List<ModelName> names =new List<ModelName>();

            foreach (var key in models.Keys)
            {
                int v = models[key];
                ModelName modelName = new ModelName();
                modelName.Name=(key);
                modelName.Count=(v);
                names.Add(modelName);
            }
            return names;
        }

        public List<string> LoadCompensateTimes(string model)
        {
            return _compensateDao.LoadCompensateTimes(model);
        }

        public List<TxModel> LoadCompensateByModelAndTime(string path)
        {
            List<String> logs = _compensateDao.LoadCompensateByModelAndTime(path);

            List<TxModel> models = new List<TxModel>();
            foreach (var json in logs)
            {
                var jsonObject =JObject.Parse(json);
                var model = new TxModel();
                long currentTime = (long)jsonObject["currentTime"];
                DateTime dt = new DateTime(currentTime);
                model.Time = dt.ToString("yyyy-MM-dd HH:mm:ss");
                model.ClassName = jsonObject["className"].ToString();
                model.Method = jsonObject["methodStr"].ToString();
                model.ExecuteTime = (int)jsonObject["time"];
                model.Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                model.State = (int) jsonObject["state"];
                model.Order = currentTime;

                var groupId = jsonObject["groupId"].ToString();

                var key = $"{path}:{groupId}";
                model.Key= key;

                models.Add(model);
            }

            models.Sort((x, y) =>
            {
                if (y.Order >  x.Order)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
            return models;
        }

        public void AutoCompensate(string compensateKey, TransactionCompensateMsg transactionCompensateMsg)
        {
            var json = JsonConvert.SerializeObject(transactionCompensateMsg);
            _logger.LogInformation($"Auto Compensate->{json}");
            //自动补偿业务执行...
            int tryTime = _configReader.CompensateTryTime;
            bool autoExecuteRes = false;
            try
            {
                int executeCount = 0;
                autoExecuteRes = ExecuteCompensateNow(json);
                _logger.LogInformation($"Automatic Compensate Result->{autoExecuteRes},json->{json}");
                while (!autoExecuteRes)
                {
                    _logger.LogInformation($"Compensate Failure, Entering Compensate Queue->{autoExecuteRes},json->{json}");
                    executeCount++;
                    if (executeCount == 3)
                    {
                        autoExecuteRes = false;
                        break;
                    }
                    try
                    {
                        Thread.Sleep(tryTime * 1000);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        _logger.LogError(e.StackTrace);
                    }
                    autoExecuteRes = ExecuteCompensateNow(json);
                }

                //执行成功删除数据
                if (autoExecuteRes)
                {
                    _compensateDao.DeleteCompensateByKey(compensateKey);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Auto Compensate Fails,msg:{e.Message}");
                //推送数据给第三方通知
                autoExecuteRes = false;
            }

            //执行补偿以后通知给业务方
            var groupId = transactionCompensateMsg.GroupId;
            JObject requestJson = new JObject()
            {
                {"action","notify" },
                {"groupId",groupId },
                {"resState",autoExecuteRes }
            };


            var url = _configReader.CompensateNotifyUrl;
            _logger.LogError($"Compensate Result Callback Address->{url}");
            //String res = HttpUtils.postJson(url, requestJson.toJSONString());
            var res = "";
            _logger.LogError($"Compensate Result Callback Result->{res}");
        }

        public bool ExecuteCompensate(string key)
        {
            var json = _compensateDao.GetCompensate(key);
            if (json == null)
            {
                throw new LcnException("no data existing");
            }

            bool hasOk = ExecuteCompensateNow(json);
            if (hasOk)
            {
                // 删除本地补偿数据
                _compensateDao.DeleteCompensateByPath(key);

                return true;
            }
            return false;
        }

        public void ReloadCompensate(TxGroup txGroup)
        {
            TxGroup compensateGroup = GetCompensateByGroupId(txGroup.GroupId);
            if (compensateGroup != null)
            {

                if (compensateGroup.GetList() != null && compensateGroup.GetList().Count > 0)
                {
                    //引用集合 iterator，方便匹配后剔除列表
                    //Iterator<TxInfo> iterator = Lists.newArrayList(compensateGroup.getList()).iterator();
                    foreach (TxInfo txInfo in txGroup.GetList())
                    {
                       var cinfo = compensateGroup.GetList().Find(x =>
                            x.Model.Equals(txInfo.Model) && x.MethodStr.Equals(txInfo.MethodStr));
                        if (cinfo != null)
                        {
                            //根据之前的数据补偿现在的事务
                            int oldNotify = cinfo.Notify;

                            if (oldNotify == 1)
                            {
                                //本次回滚
                                txInfo.IsCommit=(0);
                            }
                            else
                            {
                                //本次提交
                                txInfo.IsCommit=(1);
                            }
                        }
                       
                    }
                }
                else
                {//当没有List数据只记录了补偿数据时，理解问仅发起方提交其他均回滚
                    foreach (TxInfo txInfo in txGroup.GetList())
                    {
                        //本次回滚
                        txInfo.IsCommit = 0;
                    }
                }
            }
            _logger.LogInformation($"Compensate Loaded->{JsonConvert.SerializeObject(txGroup)}");
        }

        public bool HasCompensate()
        {
            return _compensateDao.HasCompensate();
        }

        public bool DelCompensate(string path)
        {
            _compensateDao.DeleteCompensateByPath(path);
            return true;
        }

        public TxGroup GetCompensateByGroupId(string groupId)
        {
            var json = _compensateDao.GetCompensateByGroupId(groupId);
            if (json == null)
            {
                return null;
            }
            var jsonObject = JObject.Parse(json);
            var txGroup = jsonObject["txGroup"].ToString();
            return JsonConvert.DeserializeObject<TxGroup>(txGroup);
        }


        private bool ExecuteCompensateNow(string json)
        {
            var jsonObject = JObject.Parse(json);

            var model = jsonObject["model"].ToString();

            var startError = (int)jsonObject["startError"];

            var modelInfo = ModelInfoManager.GetInstance().GetModelByModel(model);
            if (modelInfo == null)
            {
                throw new LcnException("current model offline.");
            }

            var data = jsonObject["data"].ToString();

            var groupId = jsonObject["groupId"].ToString();

            var res = _managerSenderService.SendCompensateMsg(modelInfo.ChannelName, groupId, data, startError);

            _logger.LogDebug($"executeCompensate->{json} ,@@->{res}");

            return "1".Equals(res);
        }
    }
}