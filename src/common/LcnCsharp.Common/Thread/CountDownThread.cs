using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace LcnCsharp.Common.Thread
{
    public class CountDownThread<T>
    {
        public CountDownThread(BlockingCollection<T> list, IExecute<T> execute, CountdownEvent currentThread)
        {
            _list = list;
            _execute = execute;
            _countdownEvent = currentThread;
        }

        private CountdownEvent _countdownEvent;
        private IExecute<T> _execute;
        private BlockingCollection<T> _list;

        public void Run()
        {
            //_list.Add(_execute.Execute());
            _countdownEvent.Signal();
        }

        public System.Threading.Thread Eexecute()
        {
           var  thread= new System.Threading.Thread(Run);
            thread.Start();
            return thread;
        }
    }
}