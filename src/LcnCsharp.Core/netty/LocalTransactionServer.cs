using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using LcnCsharp.Core.framework.task;

namespace LcnCsharp.Core.netty
{
    public class LocalTransactionServer: ITransactionServer
    {
        private  readonly ConcurrentDictionary<string,List<string>> localGroups = new ConcurrentDictionary<string, List<string>>();
        public void CreateTransactionGroup(string groupId)
        {
            if (!localGroups.ContainsKey(groupId))
            {
                localGroups.TryAdd(groupId, new List<string>());
            }
        }

        public TxTaskGroup AddTransactionGroup(string groupId, string taskId)
        {
            var group = new TxTaskGroup(groupId);
            return group;
        }

        public int CloseTransactionGroup(string groupId, int state)
        {
           return localGroups.TryRemove(groupId,out _)?1:0;
        }
    }
}
