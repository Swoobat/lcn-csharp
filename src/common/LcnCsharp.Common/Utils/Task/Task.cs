using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LcnCsharp.Common.Utils.Task
{
    public class Task
    {
        public AutoResetEvent Condition { get; set; }

        private static readonly object _signalLock = new object();
        private static readonly object _excuteSignalLock = new object();
        private static readonly object _signalBackLock = new object();
        private static readonly object _waitLock = new object();

        private volatile IBack _back;

        private object _obj;

        private volatile IBack _execute;

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
            return !ConditionUtils.GetInstance().HasKey(Key);
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

        public object Execute(IBack back)
        {
            object result = null;
            try
            {
                _execute = back;
                _hasExecute = true;
                ExecuteSignalTask();
                while (_execute != null && !Thread.CurrentThread.IsAlive)
                {
                    Thread.Sleep(1);
                }
                result = _obj;
            }
            catch (Exception)
            {

            }
            finally
            {
                _obj = null;
            }
            return result;
        }

        private void ExecuteSignalTask()
        {
            while (!_isAwait && !Thread.CurrentThread.IsAlive)
            {
                try
                {
                    Thread.Sleep(1);
                }
                catch (Exception e)
                {

                }

            }

            lock (_excuteSignalLock)
            {
                Condition.Set();
            }
        }

        public void Remove()
        {
            ConditionUtils.GetInstance().RemoveKey(Key);
        }

        public void SignalTask()
        {
            while (_hasExecute && !Thread.CurrentThread.IsAlive)
            {
                Thread.Sleep(1);
            }

            while (!_isAwait && !Thread.CurrentThread.IsAlive)
            {
                Thread.Sleep(1);
            }

            lock (_signalLock)
            {
                _isNotify = true;
                Condition.Set();
            }
        }



        public void SignalTask(IBack back)
        {
            while (_hasExecute && !Thread.CurrentThread.IsAlive)
            {
                Thread.Sleep(1);
            }

            while (!_isAwait && !Thread.CurrentThread.IsAlive)
            {
                Thread.Sleep(1);
            }

            lock (_signalBackLock)
            {
                _isNotify = true;
                back.Doing();
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
                    _obj = _execute.Doing();
                }
                catch (Exception)
                {

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
                lock (_waitLock)
                {
                    _isAwait = true;
                    WaitTask();
                }
            }
            catch (Exception)
            {

            }
        }

        public void AwaitTask(IBack back)
        {
            try
            {
                back?.Doing();
            }
            catch (Exception)
            {

            }

            _isAwait = true;
            WaitTask();
        }


    }
}
