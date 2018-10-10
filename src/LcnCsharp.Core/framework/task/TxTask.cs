using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LcnCsharp.Core.framework.task
{
    /// <summary>
    /// 信号器
    /// </summary>
    public class TxTask
    {
        #region Field
        private readonly AutoResetEvent condition;
        /// <summary>
        /// 是否被唤醒
        /// </summary>
        private volatile bool isNotify = false;
        /// <summary>
        /// 是否执行等待
        /// </summary>
        private volatile bool isAwait = false;

        /// <summary>
        /// 数据状态用于业务处理
        /// </summary>
        private volatile int state = 0;
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
            condition = new AutoResetEvent(false);
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
            return isNotify;
        }

        /// <summary>
        /// 是否被挂起
        /// </summary>
        /// <returns></returns>
        public bool IsAwait()
        {
            return isAwait;
        }

        public int GetState()
        {
            return state;
        }

        public void SetState(int _state)
        {
            this.state = _state;
        }


        /// <summary>
        /// 中断线程等待信号
        /// </summary>
        /// <param name="_back">前置执行任务</param>
        public void AwaitTask(Action _back = null)
        {
            #region 前置执行
            try
            {
                _back?.Invoke();
            }
            catch (Exception)
            {
                //ignore
            }
            #endregion

            isAwait = true;
            //阻塞当前线程
            condition.WaitOne();
        }

        /// <summary>
        /// 释放信号
        /// </summary>
        /// <param name="_back">执行之后停顿</param>
        public void SignalTask(Action _back = null)
        {
            isNotify = true;
            try
            {
                _back?.Invoke();
            }
            catch (Exception)
            {
                //ignore
            }
            condition.Set();
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
