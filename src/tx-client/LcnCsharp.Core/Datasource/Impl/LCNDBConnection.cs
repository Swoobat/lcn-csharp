using System.Data;
using System.Data.Common;
using LcnCsharp.Core.Framework.Task;

namespace LcnCsharp.Core.Datasource.Impl
{
    /// <summary>
    /// 分布式DB连接托管器
    /// </summary>
    public class LCNDBConnection : AbstractTxcConnection
    {
        #region Constructor

        public LCNDBConnection(IDbConnection dbConnection, string groupId) : base(dbConnection, groupId)
        {

        }

        #endregion

        #region Implement Of IDbConnection

        public override void Dispose()
        {

        }

        public override void Close()
        {

        }
        #endregion

        #region 抽象的事物方法实现
        /// <summary>
        /// IDbConnection创建的Transaction的Commit拦截
        /// </summary>
        protected override void Commit()
        {
            //记录信息
            //假提交
            //开启一个新的线程信号器 如果有信号会 执行真正的commit
            StartRunnable();
        }

        /// <summary>
        /// IDbConnection创建的Transaction的Rollback拦截
        /// </summary>
        protected override void Rollback()
        {
            GetRealDbTransaction()?.Rollback();
        }

        #endregion

        #region 信号器操作线程
        /// <summary>
        /// 信号器执行事物方法回调
        /// </summary>
        protected override void Transaction()
        {
            this.TxTask.AwaitTask();
            int rs = this.TxTask.GetState();
            try
            {
                if (rs == 1)
                {
                    GetRealDbTransaction()?.Commit();
                }
                else
                {
                    RollbackConnection();
                }
            }
            catch (DbException)
            {
                RollbackConnection();
                TxTask.SetState((int)TxTaskState.ConnectionError);
            }
            TxTask.Remove();
        }
        /// <summary>
        /// 信号器执行关闭真实db连接回调
        /// </summary>
        protected override void CloseConnection()
        {
            GetRealDbConnection()?.Close();
        }
        /// <summary>
        /// 信号器执行回滚事物回调
        /// </summary>
        protected override void RollbackConnection()
        {
            Rollback();
        }
        #endregion
    }
}
