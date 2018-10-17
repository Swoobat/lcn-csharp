using System;
using System.Data.Common;
using System.Threading;

namespace LcnCsharp.Core.Datasource
{
    /// <summary>
    /// 信号器处理事物
    /// </summary>
    public abstract class AbstractTransactionThread
    {
        /// <summary>
        /// 开启一个新的线程来用信号的方法处理事物
        /// </summary>
        protected void StartRunnable()
        {

            new Thread(() =>
            {
                try
                {
                    Transaction();
                }
                catch (Exception)
                {
                    try
                    {
                        RollbackConnection();
                    }
                    catch (DbException)
                    {
                    }
                }
                finally
                {
                    try
                    {
                        CloseConnection();
                    }
                    catch (DbException)
                    {
                    }
                }
            }).Start();

        }
        /// <summary>
        /// 信号器执行事物方法
        /// </summary>
        protected abstract void Transaction();
        /// <summary>
        /// 信号器执行关闭真实db连接
        /// </summary>
        protected abstract void CloseConnection();
        /// <summary>
        /// 信号器执行回滚事物
        /// </summary>
        protected abstract void RollbackConnection();
    }
}
