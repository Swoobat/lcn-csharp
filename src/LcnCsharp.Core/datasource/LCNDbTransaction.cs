using System;
using System.Data;

namespace LcnCsharp.Core.datasource
{
    public class LCNDbTransaction : IDbTransaction
    {
        private readonly IDbTransaction _dbTransaction;
        private readonly Action _commitAction;
        private readonly Action _rollbackAction;
        public LCNDbTransaction(IDbTransaction dbTransaction, Action commitAction, Action rollbackAction)
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
                _commitAction();
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
                _rollbackAction();
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
