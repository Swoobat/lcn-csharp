using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LcnCsharp.Core.Datasource.Impl;
using LcnCsharp.Core.netty;
using LcnCsharp.Core.Netty.Impl;
using MySql.Data.MySqlClient;
using Xunit;
using Xunit.Abstractions;

namespace LcnCsharp.Core.Tests
{
    public class MyClass
    {
        public string Test { get; set; }
        public int TestInt { get; set; }
    }
    public class TxConnectionTest
    {
        private static readonly AsyncLocal<MyClass> TestAsyncLocal = new AsyncLocal<MyClass>();
        private static readonly AsyncLocal<int> TestIntAsyncLocal = new AsyncLocal<int>();
        private readonly ITestOutputHelper output;
        public TxConnectionTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task AsyncLocal_Test()
        {
            var ad = "ad";
            var testInt = 1;
            TestAsyncLocal.Value = new MyClass{Test = ad, TestInt = testInt };
            TestIntAsyncLocal.Value = 1;
            await TaskInvoke();
            Assert.NotEqual(2, TestIntAsyncLocal.Value);
            Assert.NotEqual(TestAsyncLocal.Value.Test, ad);
            Assert.NotEqual(TestAsyncLocal.Value.TestInt, testInt);
        }
        private async Task TaskInvoke()
        {
            Trace.WriteLine(TestAsyncLocal.Value.Test);
            Trace.WriteLine(TestIntAsyncLocal.Value);
            await Task.Delay(1000);
            TestAsyncLocal.Value.Test = "Task2";
            TestAsyncLocal.Value.TestInt = 2;
            TestIntAsyncLocal.Value = 2;
        }

        [Fact]
        public void TxConnection_Test01()
        {
            ITransactionServer server = new LocalTransactionServer();
            var groupId = Guid.NewGuid().ToString("N");
            int state = 0;
            server.CreateTransactionGroup(groupId);
            try
            {
                var mysqlDbConnection = new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=root;Pwd=p@ssw0rd;charset=utf8;SslMode=none;");
                using (IDbConnection dbConnection = new LCNDBConnection(mysqlDbConnection, groupId))
                {
                    dbConnection.Open();
                    var transaction = dbConnection.BeginTransaction(); //返回的是我们包装的LCNDbTransaction
                    try
                    {
                        //执行db代码
                        var command = dbConnection.CreateCommand();
                        command.CommandText = "insert into t_test (name) values ('bbbb');";
                        command.ExecuteNonQuery();

                        transaction.Commit(); //会调用LCNDbConnection的Commit方法 会拦截
                        state = 1;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); //会调用LCNDbConnection的Rollback方法 会拦截
                    }
                }//using结束会调用LCNDbConnection的Close方法 会拦截

            }
            finally
            {
                server.CloseTransactionGroup(groupId, state);
            }
            Thread.Sleep(10000);
        }

       
    }
}
