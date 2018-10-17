using LcnCsharp.Core.Datasource.Impl;
using System;
using System.Data;
using LcnCsharp.Core.Framework.Task;

namespace LcnCsharp.Core.Datasource
{
    /// <summary>
    /// 一个抽象的托管DB连接器
    /// </summary>
    public abstract class AbstractTxcConnection : AbstractTransactionThread, ITxcConnection
    {
        #region Field
        /// <summary>
        /// 被托管的真实DB连接对象
        /// </summary>
        private readonly IDbConnection _dbConnection;
        /// <summary>
        /// 被退关的真实DB的事物对象
        /// </summary>
        private IDbTransaction _dbTransaction;

        /// <summary>
        /// 事务组id
        /// </summary>
        private readonly string _groupId;

        #endregion

        #region Property
        public TxTask TxTask { get; set; }
        #endregion

        #region Constructor
        protected AbstractTxcConnection(IDbConnection dbConnection, string groupId)
        {
            this._dbConnection = dbConnection ?? throw new ArgumentException(nameof(dbConnection));
            this._groupId = groupId ?? throw new ArgumentException(nameof(groupId));
            //创建信号管理器组
            if (string.IsNullOrEmpty(groupId)) throw new ArgumentException(nameof(groupId));
            TxTaskGroup taskGroup = TxTaskGroupManager.GetInstance().CreateTxTaskGroup(this.GroupId);
            this.TxTask = taskGroup.CurrentTxTask;
        }
        #endregion

        #region 抽象的事物方法
        protected abstract void Commit();
        protected abstract void Rollback();

        #endregion

        #region Implement Of ILCNResource
        /// <summary>
        /// 用于关闭时检查是否未删除
        /// </summary>
        public TxTask WaitTask => this.TxTask;

        /// <summary>
        /// 事务组id
        /// </summary>
        public string GroupId => this._groupId;
        #endregion

        #region Implement Of IDbConnection

        public virtual IDbTransaction BeginTransaction()
        {
            _dbTransaction = _dbConnection.BeginTransaction();
            return new LCNDBTransaction(_dbTransaction, Commit, Rollback);
        }

        public virtual IDbTransaction BeginTransaction(IsolationLevel il)
        {
            _dbTransaction = _dbConnection.BeginTransaction(il);
            return new LCNDBTransaction(_dbTransaction, Commit, Rollback);
        }

        public virtual void ChangeDatabase(string databaseName)
        {
            _dbConnection.ChangeDatabase(databaseName);
        }

        public virtual void Close()
        {
            CloseConnection();
        }

        public virtual IDbCommand CreateCommand()
        {
            return _dbConnection.CreateCommand();
        }

        public virtual void Open()
        {
            _dbConnection.Open();
        }
        public virtual void Dispose()
        {
            CloseConnection();
        }
        public virtual string ConnectionString
        {
            get => _dbConnection.ConnectionString;
            set => _dbConnection.ConnectionString = value;
        }

        public virtual int ConnectionTimeout => _dbConnection.ConnectionTimeout;
        public virtual string Database => _dbConnection.Database;
        public virtual ConnectionState State => _dbConnection.State;

        #endregion

        #region Implement Of ITxcConnection

        public IDbConnection GetRealDbConnection() => _dbConnection;
        public IDbTransaction GetRealDbTransaction() => _dbTransaction;

        #endregion
    }
}
