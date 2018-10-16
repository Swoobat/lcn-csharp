using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public void AddTransactionGroup(string groupId, string taskId)
        {
            localGroups[groupId].Add(taskId);
        }

        public int CloseTransactionGroup(string groupId, int state)
        {
            //模拟通知
            new Task(() =>
            {
                var taskGroup = TxTaskGroupManager.GetInstance().GetTxTaskGroup(groupId);
                taskGroup.State = 1;
                taskGroup?.SignalTask();
            }).Start();

            return localGroups.TryRemove(groupId, out _) ? 1 : 0;
        }
    }
}
