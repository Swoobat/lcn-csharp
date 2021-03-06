﻿using System.Collections.Generic;

namespace LcnCsharp.Core.Framework.Task
{
    /// <summary>
    /// TxTask组
    /// </summary>
    public class TaskGroup
    {
        #region Field
        private readonly List<TxTask> _txTasks = new List<TxTask>();
        #endregion

        #region Property

        public int State { get; set; }

        public string Key { get; set; }

        public TxTask CurrentTask { get; set; }

        #endregion

        #region Constructor
        public TaskGroup()
        {

        }

        public TaskGroup(string key)
        {
            this.Key = key;
        }

        #endregion

        #region Public

        /// <summary>
        /// 向组内添加一个TxTask
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(TxTask task)
        {
            _txTasks.Add(task);
        }

        /// <summary>
        /// 获取组内所有的TxTask
        /// </summary>
        /// <returns></returns>
        public List<TxTask> GetTasks()
        {
            return _txTasks;
        }

        /// <summary>
        /// 看组内是否存在等待中的TxTask
        /// </summary>
        /// <returns></returns>
        public bool IsAwait()
        {
            foreach (var task in _txTasks)
            {
                if (!task.IsAwait())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 释放组内每个TxTask
        /// </summary>
        public void SignalTask()
        {
            foreach (var task in _txTasks)
            {
                task.SetState(this.State);
                task.SignalTask();
            }
        }
        #endregion
    }
}
