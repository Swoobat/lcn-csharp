using System.Collections.Generic;
using LcnCsharp.Manager.Core.Netty.Model;

namespace LcnCsharp.Manager.Core.Redis.Service
{
    public interface IRedisServerService
    {
        string LoadNotifyJson();

        void SaveTransaction(string key, string json);

        TxGroup GetTxGroupByKey(string key);

        void SaveCompensateMsg(string name, string json);

        List<string> GetKeys(string key);

        List<string> GetValuesByKeys(List<string> keys);

        string GetValueByKey(string key);

        void DeleteKey(string key);

        void SaveLoadBalance(string groupName, string key, string data);


        string GetLoadBalance(string groupName, string key);
    }
}