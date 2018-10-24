using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using LcnCsharp.Common.Logging;
using LcnCsharp.Common.Thread;
using LcnCsharp.Common.Utils;
using LcnCsharp.Common.Utils.Task;
using LcnCsharp.Manager.Core.Compensate.Service;
using LcnCsharp.Manager.Core.Config;
using LcnCsharp.Manager.Core.Model;
using LcnCsharp.Manager.Core.Netty.Model;
using LcnCsharp.Manager.Core.Redis.Service;
using LcnCsharp.Manager.Core.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace LcnCsharp.Manager.Core.Manager.Service.Impl
{
    public class TxManagerSenderServiceImpl : ITxManagerSenderService
    {

        public class Excute : IExecute<bool>
        {
            private System.Timers.Timer Schedule(string key, int delayTime)
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = delayTime;
                timer.Elapsed += delegate
                {
                    var task = ConditionUtils.Instance.GeTask(key);
                    if (task != null && !task.IsNotify())
                    {
                        task.SetBack(new Back());
                        task.SignalTask();
                    }
                };
                timer.Start();
                return timer;
            }

            private void ThreadAwaitSend(Task task, TxInfo txInfo, string msg)
            {
                new Thread(() =>
                {
                    while (!task.IsAwait() && !Thread.CurrentThread.IsAlive)
                    {
                        try
                        {
                            Thread.Sleep(1);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (txInfo != null && txInfo.Channel != null)
                    {
                        txInfo.Channel.Send(msg, task);
                    }
                    else
                    {
                        task.SetBack(new Back());
                        task.SignalTask();
                    }
                }).Start();

            }

            public bool Execute(object obj, object ob, int checkSate, int deplyTime)
            {
                var txInfo = (TxInfo)obj;
                var txGroup = (TxGroup)ob;
                if (txInfo.Channel == null)
                {
                    return false;
                }

                JObject jsonObject = new JObject();
                jsonObject.Add("a", "t");


                if (txGroup.IsCompensate == 1)
                {
                    //补偿请求
                    jsonObject.Add("c", txInfo.IsCommit);
                }
                else
                { //正常业务
                    jsonObject.Add("c", checkSate);
                }
                jsonObject.Add("t", txInfo.Kid);
                var key = KidUtils.GenerateShortUuid();
                jsonObject.Add("k", key);

                Task task = ConditionUtils.Instance.CreateTask(key);

                System.Timers.Timer future = Schedule(key, deplyTime);

                ThreadAwaitSend(task, txInfo, jsonObject.ToString());

                task.AwaitTask();

                //if (!future.)
                //{
                //    future.cancel(false);
                //}

                try
                {
                    var data = (string)task.GetBack().Doing();
                    // 1  成功 0 失败 -1 task为空 -2 超过
                    bool res = "1".Equals(data);

                    if (res)
                    {
                        txInfo.Notify = (1);
                    }

                    return res;
                }
                catch (Exception throwable)
                {
                    //throwable.StackTrace;
                    return false;
                }
                finally
                {
                    task.Remove();
                }
            }
        }

        public class Back : IBack
        {
            public object Doing(params object[] objs)
            {
                return "-2";
            }
        }
        private readonly ILogger _logger =
            LcnCsharpLogManager.LoggerFactory.CreateLogger(typeof(TxManagerSenderServiceImpl));

        private ITxManagerService txManagerService;
        private readonly IRedisServerService _redisServerService;
        private readonly ConfigReader _configReader;
        private ICompensateService _compensateService;
        public int Confirm(TxGroup @group)
        {
            //绑定管道对象，检查网络
            SetChannel(@group.GetList());
            //事务不满足直接回滚事务
            if (@group.State == 0)
            {
                Transaction(@group, 0);
                return 0;
            }

            if (@group.Rollback == 1)
            {
                Transaction(@group, 0);
                return -1;
            }

            bool hasOk = Transaction(@group, 1);
            txManagerService.DealTxGroup(@group, hasOk);
            return hasOk ? 1 : 0;

        }

        public string SendMsg(string model, string msg, int delay)
        {
            throw new System.NotImplementedException();
        }

        public string SendCompensateMsg(string model, string groupId, string data, int startState)
        {
            throw new System.NotImplementedException();
        }

        /**
        * 匹配管道
        *
        * @param list
        */
        private void SetChannel(List<TxInfo> list)
        {
            foreach (TxInfo info in list)
            {
                if (Constants.Address.Equals(info.Address))
                {
                    var channel = SocketManager.GetInstance().GetChannelByModelName(info.ChannelAddress);
                    if (channel != null && channel.Active)
                    {
                        ChannelSender sender = new ChannelSender();
                        sender.Channel = channel;

                        info.Channel = (sender);
                    }
                }
                else
                {
                    var sender = new ChannelSender();
                    sender.Address = info.Address;
                    sender.ModelName = info.ChannelAddress;

                    info.Channel = sender;
                }
            }
        }







        /**
     * 事务提交或回归
     *
     * @param checkSate
     */
        private bool Transaction(TxGroup txGroup, int checkSate)
        {


            if (checkSate == 1)
            {

                //补偿请求，加载历史数据
                if (txGroup.IsCompensate == 1)
                {
                    _compensateService.ReloadCompensate(txGroup);
                }

                CountDownLatchHelper<bool> countDownLatchHelper = new CountDownLatchHelper<bool>();
                foreach (TxInfo txInfo in txGroup.GetList())
                {
                    if (txInfo.IsGroup == 0)
                    {
                        countDownLatchHelper.AddExecute(new Excute());
                    }
                }

                BlockingCollection<bool> hasOks = countDownLatchHelper.Execute().GetData();

                String key = _configReader.Key_prefix + txGroup.GroupId;
                _redisServerService.SaveTransaction(key, txGroup.ToJsonTring());

                bool hasOk = true;
                foreach (bool bl in hasOks)
                {
                    if (!bl)
                    {
                        hasOk = false;
                        break;
                    }
                }
                _logger.LogInformation("--->" + hasOk + ",group:" + txGroup.GroupId + ",state:" + checkSate + ",list:" + txGroup.ToJsonTring());
                return hasOk;
            }
            else
            {
                //回滚操作只发送通过不需要等待确认
                foreach (TxInfo txInfo in txGroup.GetList())
                {
                    if (txInfo.Channel != null)
                    {
                        if (txInfo.IsGroup == 0)
                        {
                            JObject jsonObject = new JObject();
                            jsonObject.Add("a", "t");
                            jsonObject.Add("c", checkSate);
                            jsonObject.Add("t", txInfo.Kid);
                            String key = KidUtils.GenerateShortUuid();
                            jsonObject.Add("k", key);
                            txInfo.Channel.Send(jsonObject.ToString());
                        }
                    }
                }
                txManagerService.DeleteTxGroup(txGroup);
                return true;
            }

        }
    }
}