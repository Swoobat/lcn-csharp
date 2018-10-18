using LcnCsharp.Core.Framework.Task;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace LcnCsharp.Core.Tests
{
    public class TxTaskTest
    {

        private readonly ITestOutputHelper output;
        public TxTaskTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TxTask_Await_AND_Signal_Test()
        {
            bool isAwait = false;
            output.WriteLine("主线程ID" + Thread.CurrentThread.ManagedThreadId);
            LcnCsharp.Common.Utils.Task.Task signal = new LcnCsharp.Common.Utils.Task.Task();
            new Task(() =>
            {
                Thread.Sleep(1000);
                output.WriteLine("开始等待,当前线程ID" + Thread.CurrentThread.ManagedThreadId);
                signal.AwaitTask();
                isAwait = true;
                output.WriteLine("结束等待,当前线程ID" + Thread.CurrentThread.ManagedThreadId);
            }).Start();

            new Task(() =>
            {
                Thread.Sleep(3000);
                output.WriteLine("开始释放线程线程ID" + Thread.CurrentThread.ManagedThreadId);
                Assert.False(isAwait);
                signal.SignalTask();
                output.WriteLine("结束释放线程线程ID" + Thread.CurrentThread.ManagedThreadId);
            }).Start();

            Thread.Sleep(10000);
            Assert.True(isAwait);
        }

        [Fact]
        public void TxTask_Excute_Test()
        {
            var task = new LcnCsharp.Common.Utils.Task.Task("test");

            new Task(() =>
            {
                Thread.Sleep(1000);
                task.AwaitTask();
            }).Start();

            new Task(() =>
            {
                var tId = Thread.CurrentThread.ManagedThreadId;
                Thread.Sleep(3000);
                var rtValue = "test";
                var result = task.Execute(() =>
                {
                    Assert.NotEqual(Thread.CurrentThread.ManagedThreadId, tId);
                    return rtValue;
                });
                Assert.Equal(rtValue, result);
            }).Start();

            Thread.Sleep(5000);

        }

        [Fact]
        public void TxTask_Group_Test()
        {
            TaskGroup group = TaskGroupManager.Instance.CreateTask("test", "db");
            Assert.NotNull(group.CurrentTask);
            var group2 = TaskGroupManager.Instance.GetTaskGroup("test");
            Assert.StrictEqual(group2, group);
            var tasks = group2.GetTasks();
            Assert.NotEmpty(tasks);
            Assert.Single(tasks);
            Assert.IsType<TxTask>(tasks.Single());
            var task = TaskGroupManager.Instance.GetTask("test", "db");
            Assert.IsType<TxTask>(task);
            Assert.StrictEqual(task, tasks.Single());
            Assert.False(task.IsAwait());
            new Task(() =>
            {
                Thread.Sleep(1000);
                task.AwaitTask();
                Assert.Equal((int)TaskState.Commit, task.GetState());
            }).Start();
            Thread.Sleep(3000);
            Assert.True(task.IsAwait());
            Assert.True(group.IsAwait());
            new Task(() =>
            {
                Thread.Sleep(1000);
                task.SetState((int)TaskState.Commit);
                task.SignalTask();
            }).Start();
            Thread.Sleep(3000);
            Assert.True(task.IsNotify());
            task.Remove();
            Assert.True(task.IsRemove());
            var group3 = TaskGroupManager.Instance.GetTaskGroup("test");
            Assert.Null(group3);
        }


    }

}
