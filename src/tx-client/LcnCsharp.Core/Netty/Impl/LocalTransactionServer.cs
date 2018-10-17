using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using LcnCsharp.Core.netty;
using LcnCsharp.Core.Framework.Task;

namespace LcnCsharp.Core.Netty.Impl
{
    /// <summary>
    /// 本地分布式事物Server处理
    /// 用来模拟远程Server
    /// </summary>
    public class LocalTransactionServer: ITransactionServer
    {
        #region Field
        private readonly ConcurrentDictionary<string, List<string>> localGroups = new ConcurrentDictionary<string, List<string>>();
        #endregion

        #region Implement Of ITransactionServer
        /// <summary>
        /// 创建分布式事物组
        /// </summary>
        /// <param name="groupId"></param>
        public void CreateTransactionGroup(string groupId)
        {
            if (!localGroups.ContainsKey(groupId))
            {
                localGroups.TryAdd(groupId, new List<string>());
            }
        }
        /// <summary>
        /// 加入分布式事务组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="taskId"></param>
        public void AddTransactionGroup(string groupId, string taskId)
        {
            localGroups[groupId].Add(taskId);
        }
        /// <summary>
        /// 关闭分布式事务组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
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
        #endregion
    }
}
