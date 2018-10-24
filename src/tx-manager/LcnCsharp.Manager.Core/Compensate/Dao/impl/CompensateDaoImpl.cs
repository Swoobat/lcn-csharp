using System;
using System.Collections.Generic;
using LcnCsharp.Manager.Core.Compensate.Model;
using LcnCsharp.Manager.Core.Config;
using LcnCsharp.Manager.Core.Redis.Service;
using Newtonsoft.Json;

namespace LcnCsharp.Manager.Core.Compensate.Dao.impl
{
    public class CompensateDaoImpl:ICompensateDao
    {
        private readonly IRedisServerService _redisServerService;
        private readonly ConfigReader _configReader;

        public string SaveCompensateMsg(TransactionCompensateMsg transactionCompensateMsg)
        {
            var name =
                $"{_configReader.Key_prefix_compensate}{transactionCompensateMsg.Model}:{DateTime.Now.ToString("yyyy-MM-dd")}:{transactionCompensateMsg.GroupId}.json";
            var json = JsonConvert.SerializeObject(transactionCompensateMsg);
            _redisServerService.SaveCompensateMsg(name,json);
            return name;
        }

        public List<string> LoadCompensateKeys()
        {
            string key = _configReader.Key_prefix_compensate + "*";
            return _redisServerService.GetKeys(key);
        }

        public List<string> LoadCompensateTimes(string model)
        {
            var key = _configReader.Key_prefix_compensate + model + ":*";
            var keys = _redisServerService.GetKeys(key);
            var times = new List<string>();
            foreach (var item in keys)
            {
                if (item.Length>36)
                {
                    var time = item.Substring(item.Length - 24, item.Length - 14);
                    if (!times.Contains(time))
                    {
                        times.Add(time);
                    }
                }
            }
            return times;
        }

        public List<string> LoadCompensateByModelAndTime(string path)
        {
            var key = $"{_configReader.Key_prefix_compensate}{path}*";
            var keys = _redisServerService.GetKeys(key);
            var values = _redisServerService.GetValuesByKeys(keys);
            return values;
        }

        public string GetCompensate(string key)
        {
            var k = $"{_configReader.Key_prefix_compensate}{key}.json";
            return _redisServerService.GetValueByKey(k);
        }

        public string GetCompensateByGroupId(string groupId)
        {
            var key = $"{_configReader.Key_prefix_compensate}{groupId}.json";
            var keys = _redisServerService.GetKeys(key);
            if (keys != null && keys.Count == 1)
            {
                var k = keys[0];
                return _redisServerService.GetValueByKey(k);
            }
            return null;
        }

        public void DeleteCompensateByPath(string path)
        {
            var k = $"{_configReader.Key_prefix_compensate}{path}.json";
            _redisServerService.DeleteKey(k);
        }

        public void DeleteCompensateByKey(string key)
        {
            _redisServerService.DeleteKey(key);
        }

        public bool HasCompensate()
        {
            var key = _configReader.Key_prefix_compensate + "*";
            var keys = _redisServerService.GetKeys(key);
            return keys != null && keys.Count > 0;
        }
    }
}