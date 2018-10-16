using System;

namespace LcnCsharp.Core.datasource
{
    public interface ILCNTransactionControl
    {
        /// <summary>
        /// 是否是同一个事务下
        /// </summary>
        /// <param name="group">事务组id</param>
        /// <returns></returns>
        bool HasGroup(String group);
        /// <summary>
        /// 有无执行过事务操作
        /// </summary>
        /// <returns>true 有，false 否</returns>
        bool ExecuteTransactionOperation();

        /// <summary>
        /// 是否没有事务操作  default false
        /// </summary>
        /// <returns>true 是 false 否</returns>
        bool IsNoTransactionOperation();
        /// <summary>
        /// 设置开启没有事务操作
        /// </summary>
        /// <returns></returns>
        void AutoNoTransactionOperation();
    }
}
