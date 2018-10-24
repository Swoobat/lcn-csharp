using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LcnCsharp.Common.Thread
{
    public class CountDownLatchHelper<T>
    {
        private bool _isExecute = false;
        private int _count;
        private readonly BlockingCollection<T> _data;
        private List<CountDownThread<T>> _list;
        private CountdownEvent _end;
        private readonly List<IExecute<T>> _executes = null;


        public CountDownLatchHelper()
        {
            _executes = new List<IExecute<T>>();
            _data = new BlockingCollection<T>();
            _list = new List<CountDownThread<T>>();
        }

        public CountDownLatchHelper<T> AddExecute(IExecute<T> execute)
        {
            _executes.Add(execute);
            return this;
        }


        public CountDownLatchHelper<T> Execute()
        {
            _count = _executes.Count;
            BlockingCollection<System.Threading.Thread> threads=new BlockingCollection<System.Threading.Thread>();
            if (_count > 0)
            {
                _end = new CountdownEvent(_count);
                foreach (IExecute<T> countDown in _executes)
                {
                    var countDownThread = new CountDownThread<T>(_data, countDown, _end);
                    threads.Add(countDownThread.Eexecute());
                }
                try
                {
                    _end.Wait();
                }
                catch (System.Exception e)
                {
                    
                }
            }

            foreach (var item in threads)
            {
                item.Abort();
            }
            _isExecute = true;
            return this;
        }

        public BlockingCollection<T> GetData()
        {
            if (!_isExecute)
                throw new System.Exception("no execute !");
            return _data;
        }

    }
}