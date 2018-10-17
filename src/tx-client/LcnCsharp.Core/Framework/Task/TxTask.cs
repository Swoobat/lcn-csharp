using System;
using System.Threading;

namespace LcnCsharp.Core.Framework.Task
{
    /// <summary>
    /// 信号器
    /// </summary>
    public class TxTask
    {
        #region Field
        private readonly AutoResetEvent _condition;
        /// <summary>
        /// 是否被唤醒
        /// </summary>
        private volatile bool _isNotify = false;
        /// <summary>
        /// 是否执行等待
        /// </summary>
        private volatile bool _isAwait = false;

        /// <summary>
        /// 数据状态用于业务处理
        /// </summary>
        private volatile int _state = 0;
        #endregion

        #region Property

        /// <summary>
        /// 唯一标示key
        /// </summary>
        public string Key { get; set; }

        #endregion

        #region Constructor

        public TxTask()
        {
            _condition = new AutoResetEvent(false);
        }

        public TxTask(string key):this()
        {
            this.Key = key;
        }
        #endregion

        #region Public

        /// <summary>
        /// 是否被唤醒
        /// </summary>
        /// <returns></returns>
        public bool IsNotify()
        {
            return _isNotify;
        }

        /// <summary>
        /// 是否被挂起
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 中断线程等待信号
        /// </summary>
        /// <param name="_back">前置执行任务</param>
        public void AwaitTask(Action back = null)
        {
            #region 前置执行
            try
            {
                back?.Invoke();
            }
            catch (Exception)
            {
                //ignore
            }
            #endregion

            _isAwait = true;
            //阻塞当前线程
            _condition.WaitOne();
        }

        /// <summary>
        /// 释放信号
        /// </summary>
        /// <param name="_back">执行之后停顿</param>
        public void SignalTask(Action back = null)
        {
            _isNotify = true;
            try
            {
                back?.Invoke();
            }
            catch (Exception)
            {
                //ignore
            }
            _condition.Set();
        }

        /// <summary>
        /// 从信号管理器移除
        /// </summary>
        public void Remove()
        {
            TxTaskManager.GetInstance().RemoveKey(this.Key);
        }
        #endregion
    }
}
