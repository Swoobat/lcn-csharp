using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LcnCsharp.Core.Framework.Task;
using Xunit;

namespace LcnCsharp.Core.Tests
{
    public class TxTaskTest
    {
        [Fact]
        public void TxTaskTest1()
        {

            LcnCsharp.Common.Utils.Task.Task signal = new LcnCsharp.Common.Utils.Task.Task();
            new Task(() =>
            {
                Trace.WriteLine("当前线程ID" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);

                signal.AwaitTask();

                Trace.WriteLine("结束等待,当前线程ID" + Thread.CurrentThread.ManagedThreadId);
            }).Start();

            new Task(() =>
            {
                Thread.Sleep(3000);
                Trace.WriteLine("开始释放线程线程ID" + Thread.CurrentThread.ManagedThreadId);
                signal.SignalTask();
                Trace.WriteLine("结束释放线程线程ID" + Thread.CurrentThread.ManagedThreadId);
            }).Start();

            Thread.Sleep(10000);
        }

        [Fact]
        public void TxTaskManagerTest1()
        {
           
        }
    }
}
