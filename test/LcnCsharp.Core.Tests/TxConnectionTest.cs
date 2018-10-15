using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LcnCsharp.Core.datasource;
using LcnCsharp.Core.framework.task;
using Xunit;

namespace LcnCsharp.Core.Tests
{
    public class TxConnectionTest
    {
        [Fact]
        public void TxConnectionTest1()
        {
            IDbConnection dbConnection = new LCNStartConnection(new SqlConnection());
            var transaction = dbConnection.BeginTransaction();
            transaction.Commit();
        }

    }
}
