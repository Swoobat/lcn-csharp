using LcnCsharp.Core.framework.task;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace LcnCsharp.Core.datasource
{
    public class LCNDbConnection : AbstractTxcConnection
    {

        public LCNDbConnection(IDbConnection dbConnection, string groupId) : base(dbConnection)
        {
            //创建信号管理器组
            if (string.IsNullOrEmpty(groupId)) throw new ArgumentException(nameof(groupId));
            this.GroupId = groupId;
            TxTaskGroup taskGroup = TxTaskGroupManager.GetInstance().CreateTxTaskGroup(this.GroupId);
            this.TxTask = taskGroup.CurrentTxTask;
        }

        public override void Dispose()
        {
           
        }

        public override void Close()
        {

        }

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


        #region 信号器操作线程

        protected override void Transaction()
        {
            this.TxTask.AwaitTask();
            int rs = this.TxTask.GetState();
            try
            {
                if (rs==1)
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

        protected override void CloseConnection()
        {
            GetRealDbConnection()?.Close();
        }

        protected override void RollbackConnection()
        {
            Rollback();
        }
        #endregion
    }
}
