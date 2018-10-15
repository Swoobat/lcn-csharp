using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LcnCsharp.Core.datasource
{
    public class LCNStartConnection: AbstractTxcConnection
    {
        public LCNStartConnection(IDbConnection dbConnection) : base(dbConnection)
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
