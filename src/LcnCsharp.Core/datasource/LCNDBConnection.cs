using System;
using System.Data;

namespace LcnCsharp.Core.datasource
{
    public class LCNDbConnection: AbstractTxcConnection
    {
        public LCNDbConnection(IDbConnection dbConnection) : base(dbConnection)
        {

        }
        public override void Commit(IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public override void Rollback(IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        public override void Close(IDbConnection dbConnection)
        {
            throw new NotImplementedException();
        }
    }
}
