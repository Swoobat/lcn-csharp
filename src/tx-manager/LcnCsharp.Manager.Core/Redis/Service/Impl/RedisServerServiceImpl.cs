using System;
using System.Collections.Generic;
using System.Text;
using LcnCsharp.Manager.Core.Config;
using LcnCsharp.Manager.Core.Netty.Model;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace LcnCsharp.Manager.Core.Redis.Service.Impl
{
    public class RedisServerServiceImpl : IRedisServerService
    {
        private readonly IDistributedCache _cache;
        private readonly ConfigReader _configReader;
        public string LoadNotifyJson()
        {
            var keys = _cache.Keys(_configReader.Key_prefix_compensate + "*");
            var jsonArray = new JArray();
            foreach (var item in keys)
            {
                var json = Encoding.UTF8.GetString(_cache.Get(item));
                var jsonJObject = new JObject()
                {
                    {"key",item},
                    {"value", JObject.Parse(json)}
                };
                jsonArray.Add(jsonJObject);
            }

            return jsonArray.ToString();
        }

        public void SaveTransaction(string key, string json)
        {
            _cache.Set(key, Encoding.Default.GetBytes(json), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromSeconds(_configReader.RedisSaveMaxTime) });
        }

        public TxGroup GetTxGroupByKey(string key)
        {
            var json = Encoding.UTF8.GetString(_cache.Get(key));
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return TxGroup.Parser(json);
        }

        public void SaveCompensateMsg(string name, string json)
        {
            _cache.Set(name, Encoding.Default.GetBytes(json), new DistributedCacheEntryOptions());
        }

        public List<string> GetKeys(string key)
        {
            var keys = _cache.Keys(key);
            return keys;
        }

        public List<string> GetValuesByKeys(List<string> keys)
        {
            var  list=new List<string>();
            foreach (var item in keys)
            {
                var json = Encoding.UTF8.GetString(_cache.Get(item));
                list.Add(json);
            }

            return list;
        }

        public string GetValueByKey(string key)
        {
            return Encoding.UTF8.GetString(_cache.Get(key));
        }

        public void DeleteKey(string key)
        {
            _cache.Remove(key);
        }

        public void SaveLoadBalance(string groupName, string key, string data)
        {
          
            _cache.HashSet(groupName, new[] { new HashEntry(key, data) });
        }

        public string GetLoadBalance(string groupName, string key)
        {
           return _cache.HashGet(groupName, key);
        }
    }
}