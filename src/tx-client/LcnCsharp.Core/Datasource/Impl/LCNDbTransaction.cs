using System;
using System.Data;

namespace LcnCsharp.Core.Datasource.Impl
{
    /// <summary>
    /// 分布式托管事物Transaction
    /// </summary>
    public class LCNDBTransaction : IDbTransaction
    {
        #region Field
        /// <summary>
        /// 真实事物对象
        /// </summary>
        private readonly IDbTransaction _dbTransaction;
        /// <summary>
        /// Commit回调方法
        /// </summary>
        private readonly Action _commitAction;
        /// <summary>
        /// RollBack回调方法
        /// </summary>
        private readonly Action _rollbackAction;

        #endregion

        #region Constructor
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <param name="commitAction"></param>
        /// <param name="rollbackAction"></param>
        public LCNDBTransaction(IDbTransaction dbTransaction, Action commitAction, Action rollbackAction)
        {
            _dbTransaction = dbTransaction;
            _commitAction = commitAction;
            _rollbackAction = rollbackAction;
        }
        #endregion

        #region Implement Of IDbTransaction
        /// <summary>
        /// 重写Commit方法
        /// </summary>
        public void Commit()
        {
            if (_commitAction == null)
            {
                _dbTransaction.Commit();
            }
            else
            {
                _commitAction();
            }

        }

        /// <summary>
        /// 重写RollBack方法
        /// </summary>
        public void Rollback()
        {
            if (_commitAction == null)
            {
                _dbTransaction.Rollback();
            }
            else
            {
                _rollbackAction();
            }
        }

        public IDbConnection Connection => _dbTransaction.Connection;

        public IsolationLevel IsolationLevel => _dbTransaction.IsolationLevel;
        public void Dispose()
        {
            _dbTransaction.Dispose();
        } 
        #endregion
    }
}
