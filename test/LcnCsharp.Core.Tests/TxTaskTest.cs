using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LcnCsharp.Core.framework.task;
using Xunit;

namespace LcnCsharp.Core.Tests
{
    public class TxTaskTest
    {
        [Fact]
        public void TxTaskTest1()
        {
            
            TxTask signal = new TxTask();

            new Task(() =>
            {
                Trace.WriteLine("当前线程ID" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
                signal.AwaitTask(() =>
                {
                    Trace.WriteLine("开始等待,当前线程ID" +Thread.CurrentThread.ManagedThreadId);
                });

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
            TxTask signal = TxTaskManager.GetInstance().CreateTxTask("test");
            var signal2 = TxTaskManager.GetInstance().GetTxTask("test");
            Assert.Equal(signal2,signal);
            new Task(() =>
            {
                Trace.WriteLine("当前线程ID" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
                signal.AwaitTask(() =>
                {
                    Trace.WriteLine("开始等待,当前线程ID" + Thread.CurrentThread.ManagedThreadId);
                });

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
            signal.Remove();
        }
    }
}
