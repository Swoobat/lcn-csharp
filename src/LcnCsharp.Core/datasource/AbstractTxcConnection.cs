using LcnCsharp.Core.framework.task;
using System;
using System.Data;

namespace LcnCsharp.Core.datasource
{
    public abstract class AbstractTxcConnection : ILCNDbConnection
    {
        private readonly IDbConnection _dbConnection;
        private String groupId;
        private TxTask waitTask;

        protected AbstractTxcConnection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        public abstract void Commit(IDbTransaction dbTransaction);
        public abstract void Rollback(IDbTransaction dbTransaction);
        public abstract void Close(IDbConnection dbConnection);

        public IDbTransaction BeginTransaction()
        {
            var realTransaction = _dbConnection.BeginTransaction();
            return new LCNDbTransaction(realTransaction, Commit, Rollback);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            var realTransaction = _dbConnection.BeginTransaction(il);
            return new LCNDbTransaction(realTransaction, Commit, Rollback);
        }

        public void ChangeDatabase(string databaseName)
        {
            _dbConnection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            Close(_dbConnection);
        }

        public IDbCommand CreateCommand()
        {
            return _dbConnection.CreateCommand();
        }

        public void Open()
        {
            _dbConnection.Open();
        }

        public string ConnectionString
        {
            get => _dbConnection.ConnectionString;
            set => _dbConnection.ConnectionString = value;
        }

        public int ConnectionTimeout => _dbConnection.ConnectionTimeout;
        public string Database => _dbConnection.Database;
        public ConnectionState State => _dbConnection.State;

        public TxTask GetWaitTask()
        {
            return this.waitTask;
        }

        public string GetGroupId()
        {
            return groupId;
        }
    }
}
