using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LcnCsharp.Core.datasource;
using LcnCsharp.Core.framework.task;
using LcnCsharp.Core.netty;
using Xunit;

namespace LcnCsharp.Core.Tests
{
    public class MyClass
    {
        public string Test { get; set; }
        public int TestInt { get; set; }
    }
    public class TxConnectionTest
    {
        private static AsyncLocal<MyClass> TestAsyncLocal = new AsyncLocal<MyClass>();
        private static AsyncLocal<int> TestIntAsyncLocal = new AsyncLocal<int>();
        [Fact]
        public void TxConnectionTest1()
        {
            IDbConnection dbConnection = new LCNDbConnection(new SqlConnection());
            var transaction = dbConnection.BeginTransaction();
            transaction.Commit();
        }

        [Fact]
        public async Task TxConnectionTest2()
        {
            TestAsyncLocal.Value = new MyClass{Test = "ad",TestInt = 1};
            TestIntAsyncLocal.Value = 1;
            await Task2();
            Trace.WriteLine(TestAsyncLocal.Value.Test);
            Trace.WriteLine(TestIntAsyncLocal.Value);
            Trace.WriteLine(TestAsyncLocal.Value.TestInt);
        }
        [Fact]
        public void TxConnectionTest3()
        {
            ITransactionServer server = new LocalTransactionServer();
            var groupId = Guid.NewGuid().ToString("N");
            int state = 0;
            server.CreateTransactionGroup(groupId);
            try
            {
                using (IDbConnection dbConnection = new LCNDbConnection(new SqlConnection("XXXXX")))
                {
                    dbConnection.Open();
                    var transaction = dbConnection.BeginTransaction(); //返回的是我们包装的LCNDbTransaction

                    try
                    {
                        //执行db代码
                        transaction.Commit(); //会调用LCNDbConnection的Commit方法 会拦截
                        state = 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); //会调用LCNDbConnection的Rollback方法 会拦截
                    }
                }//using结束会调用LCNDbConnection的Close方法 会拦截

            }
            finally
            {
                server.CloseTransactionGroup(groupId, state);
            }
        }
        public async Task Task2()
        {
            Trace.WriteLine(TestAsyncLocal.Value.Test);
            Trace.WriteLine(TestIntAsyncLocal.Value);
            await Task.Delay(1000);
            TestAsyncLocal.Value.Test = "Task2";
            TestAsyncLocal.Value.TestInt = 2;
            TestIntAsyncLocal.Value = 2;
        }
    }
}
