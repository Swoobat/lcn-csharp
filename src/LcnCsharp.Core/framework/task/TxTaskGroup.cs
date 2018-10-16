using System.Collections.Generic;

namespace LcnCsharp.Core.framework.task
{
    /// <summary>
    /// TxTask组
    /// </summary>
    public class TxTaskGroup
    {
        #region Field
        private readonly List<TxTask> TxTasks = new List<TxTask>();
        #endregion

        #region Property

        public int State { get; set; }

        public string Key { get; set; }

        public TxTask CurrentTxTask { get; set; }

        #endregion

        #region Constructor
        public TxTaskGroup()
        {

        }

        public TxTaskGroup(string key)
        {
            this.Key = key;
        }

        #endregion

        #region Public

        /// <summary>
        /// 向组内添加一个TxTask
        /// </summary>
        /// <param name="task"></param>
        public void AddTxTask(TxTask task)
        {
            TxTasks.Add(task);
        }

        /// <summary>
        /// 获取组内所有的TxTask
        /// </summary>
        /// <returns></returns>
        public List<TxTask> GetTxTasks()
        {
            return TxTasks;
        }

        /// <summary>
        /// 看组内是否存在等待中的TxTask
        /// </summary>
        /// <returns></returns>
        public bool IsAwait()
        {
            foreach (var task in TxTasks)
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
            foreach (var task in TxTasks)
            {
                task.SetState(this.State);
                task.SignalTask();
            }
        } 
        #endregion
    }
}
