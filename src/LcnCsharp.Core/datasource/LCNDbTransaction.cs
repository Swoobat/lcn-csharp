using System;
using System.Data;

namespace LcnCsharp.Core.datasource
{
    public class LCNDbTransaction : IDbTransaction
    {
        private readonly IDbTransaction _dbTransaction;
        private readonly Action<IDbTransaction> _commitAction;
        private readonly Action<IDbTransaction> _rollbackAction;
        public LCNDbTransaction(IDbTransaction dbTransaction, Action<IDbTransaction> commitAction, Action<IDbTransaction> rollbackAction)
        {
            _dbTransaction = dbTransaction;
            _commitAction = commitAction;
            _rollbackAction = rollbackAction;
        }
        public void Commit()
        {
            if (_commitAction == null)
            {
                _dbTransaction.Commit();
            }
            else
            {
                _commitAction(_dbTransaction);
            }
            
        }

        public void Rollback()
        {
            if (_commitAction == null)
            {
                _dbTransaction.Rollback();
            }
            else
            {
                _rollbackAction(_dbTransaction);
            }
        }
        public IDbConnection Connection => _dbTransaction.Connection;

        public IsolationLevel IsolationLevel => _dbTransaction.IsolationLevel;
        public void Dispose()
        {
            _dbTransaction.Dispose();
        }
    }
}
