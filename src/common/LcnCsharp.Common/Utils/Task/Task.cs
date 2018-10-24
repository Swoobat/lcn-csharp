using LcnCsharp.Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace LcnCsharp.Common.Utils.Task
{
    public class Task
    {

        public AutoResetEvent Condition { get; set; }

        private static readonly object _lockWaitObj = new object();
        private static readonly object _lockSignalObj = new object();
        private static readonly object _lockExcuteObj = new object();


        private readonly ILogger logger = LcnCsharpLogManager.LoggerFactory.CreateLogger<Task>();

        private volatile IBack _back;

        private object _obj;

        private volatile Func<object> _execute;

        private volatile bool _hasExecute = false;

        private volatile bool _isNotify = false;

        private volatile bool _isAwait = false;

        public string Key { get; set; }

        private volatile int _state = 0;

        public bool IsNotify()
        {
            return _isNotify;
        }

        public bool IsRemove()
        {
            return !ConditionUtils.Instance.HasKey(Key);
        }

        public bool IsAwait()
        {
            return _isAwait;
        }

        public int GetState()
        {
            return _state;
        }

        public void SetState(int state)
        {
            _state = state;
        }

        public IBack GetBack()
        {
            return _back;
        }

        public void SetBack(IBack back)
        {
            _back = back;
        }

        public Task()
        {
            Condition = new AutoResetEvent(false);
        }
        public Task(string key) : this()
        {
            this.Key = key;
        }
        public object Execute(Func<object> back)
        {
            lock (_lockExcuteObj)
            {
                try
                {
                    _execute = back;
                    _hasExecute = true;
                    ExecuteSignalTask();
                    while (_execute != null && !System.Threading.Thread.CurrentThread.IsAlive)
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(1);
                        }
                        catch (System.Exception ex)
                        {
                            logger.LogError(ex, "exception on Task.Thread.Sleep(1);");
                        }
                    }
                    return _obj;
                }
                finally
                {
                    _obj = null;
                }
            }

        }

        private void ExecuteSignalTask()
        {
            while (!_isAwait && !System.Threading.Thread.CurrentThread.IsAlive)
            {
                try
                {
                    System.Threading.Thread.Sleep(1);
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "exception on Task.Thread.Sleep(1);");
                }
            }

            lock (_lockSignalObj)
            {
                Condition.Set();
            }
        }

        public void Remove()
        {
            ConditionUtils.Instance.RemoveKey(Key);
        }

        public void SignalTask()
        {
            while (_hasExecute && !System.Threading.Thread.CurrentThread.IsAlive)
            {
                try
                {
                    System.Threading.Thread.Sleep(1);
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "exception on Task.Thread.Sleep(1);");
                }
            }

            while (!_isAwait && !System.Threading.Thread.CurrentThread.IsAlive)
            {
                try
                {
                    System.Threading.Thread.Sleep(1);
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "exception on Task.Thread.Sleep(1);");
                }
            }

            lock (_lockSignalObj)
            {
                _isNotify = true;
                Condition.Set();
            }
        }



        public void SignalTask(Action back)
        {
            while (_hasExecute && !System.Threading.Thread.CurrentThread.IsAlive)
            {
                try
                {
                    System.Threading.Thread.Sleep(1);
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "exception on Task.Thread.Sleep(1);");
                }
            }

            while (!_isAwait && !System.Threading.Thread.CurrentThread.IsAlive)
            {
                try
                {
                    System.Threading.Thread.Sleep(1);
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "exception on Task.Thread.Sleep(1);");
                }
            }

            lock (_lockSignalObj)
            {
                _isNotify = true;
                try
                {
                    back?.Invoke();
                }
                catch (System.Exception)
                {
                    //ignore
                }
                Condition.Set();
            }
        }

        private void WaitTask()
        {
            Condition.WaitOne();
            if (_hasExecute)
            {
                try
                {
                    _obj = _execute.Invoke();
                }
                catch (System.Exception e)
                {
                    _obj = e;
                }
                _hasExecute = false;
                _execute = null;
                WaitTask();
            }
        }

        public void AwaitTask()
        {
            try
            {
                lock (_lockWaitObj)
                {
                    _isAwait = true;
                    WaitTask();
                }
            }
            catch (System.Exception)
            {
                //ignore
            }
        }

        public void AwaitTask(Action back)
        {
            try
            {
                back?.Invoke();
            }
            catch (System.Exception)
            {
                //ignore
            }

            try
            {
                lock (_lockWaitObj)
                {
                    _isAwait = true;
                    WaitTask();
                }
            }
            catch (System.Exception)
            {
                //ignore
            }
        }


    }
}
