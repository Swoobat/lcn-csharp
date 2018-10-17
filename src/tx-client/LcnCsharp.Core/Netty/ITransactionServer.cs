using System;

namespace LcnCsharp.Core.netty
{
    /// <summary>
    /// 分布式事物Server处理接口
    /// </summary>
    public interface ITransactionServer
    {
        /// <summary>
        /// 创建分布式事物组
        /// </summary>
        /// <param name="groupId"></param>
        void CreateTransactionGroup(string groupId);
        /// <summary>
        /// 加入分布式事务组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="taskId"></param>
        void AddTransactionGroup(string groupId, String taskId);
        /// <summary>
        /// 关闭分布式事务组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        int CloseTransactionGroup(string groupId, int state);
    }
}
