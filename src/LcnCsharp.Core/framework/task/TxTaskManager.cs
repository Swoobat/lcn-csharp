using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LcnCsharp.Core.framework.task
{
    /// <summary>
    /// 信号管理器
    /// </summary>
    public class TxTaskManager
    {
        #region Filed
        private static readonly TxTaskManager instance = new TxTaskManager();

        private readonly ConcurrentDictionary<string, TxTask> taskMap = new ConcurrentDictionary<string, TxTask>();
        #endregion

        #region Constructor

        private TxTaskManager() { }

        #endregion

        #region Static
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static TxTaskManager GetInstance()
        {
            return instance;
        }
        #endregion

        #region Public

        /// <summary>
        /// 创建一个信号器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TxTask CreateTxTask(string key)
        {
            TxTask task = new TxTask(key);
            taskMap.AddOrUpdate(key, task, (oldValue, newValue) => newValue);
            return task;
        }

        /// <summary>
        /// 根据Key获取信号器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TxTask GetTxTask(string key)
        {
            return taskMap[key];
        }

        /// <summary>
        /// 根据Key删除信号器
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                taskMap.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// 是否存在对应Key的信号器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ExistKey(string key)
        {
            return taskMap.ContainsKey(key);
        } 
        #endregion
    }
}
