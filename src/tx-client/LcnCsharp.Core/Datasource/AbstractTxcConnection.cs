using System;
using System.Data;
using LcnCsharp.Core.Datasource.Impl;
using LcnCsharp.Core.Framework.Task;

namespace LcnCsharp.Core.Datasource
{
    public abstract class AbstractTxcConnection : AbstractTransactionThread, ITxcConnection
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        public string GroupId { get; set; }
        public TxTask TxTask { get; set; }

        protected AbstractTxcConnection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentException(nameof(dbConnection));
        }
        public virtual void Dispose()
        {
            CloseConnection();
        }

        protected abstract void Commit();
        protected abstract void Rollback();

        public IDbTransaction BeginTransaction()
        {
            _dbTransaction = _dbConnection.BeginTransaction();
            return new LCNDbTransaction(_dbTransaction, Commit, Rollback);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            _dbTransaction = _dbConnection.BeginTransaction(il);
            return new LCNDbTransaction(_dbTransaction, Commit, Rollback);
        }

        public void ChangeDatabase(string databaseName)
        {
            _dbConnection.ChangeDatabase(databaseName);
        }

        public virtual void Close()
        {
            CloseConnection();
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
            return this.TxTask;
        }

        public string GetGroupId()
        {
            return GroupId;
        }

        public IDbConnection GetRealDbConnection() => _dbConnection;
        public IDbTransaction GetRealDbTransaction() => _dbTransaction;
    }
}
