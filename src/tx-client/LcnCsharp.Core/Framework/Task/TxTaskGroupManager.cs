using System.Collections.Concurrent;
using System.Linq;

namespace LcnCsharp.Core.Framework.Task
{ 
    public class TxTaskGroupManager
    {
        #region Field
        private static readonly TxTaskGroupManager instance = new TxTaskGroupManager();
        private readonly ConcurrentDictionary<string, TxTaskGroup> taskMap = new ConcurrentDictionary<string, TxTaskGroup>();

        #endregion

        #region Constructor

        private TxTaskGroupManager() { }

        #endregion

        #region Static
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static TxTaskGroupManager GetInstance()
        {
            return instance;
        }
        #endregion

        #region Public

        /// <summary>
        /// 创建一个TxTask组
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public TxTaskGroup CreateTxTaskGroup(string key, string type = "db")
        {
            TxTaskGroup taskGroup = GetTxTaskGroup(key);
            if (taskGroup == null)
            {
                taskGroup = new TxTaskGroup(key);
            }

            string taskKey = type + "_" + key;

            TxTask task = TxTaskManager.GetInstance().CreateTxTask(taskKey);
            taskGroup.CurrentTxTask = task;
            taskGroup.AddTxTask(task);
            taskMap.AddOrUpdate(key, taskGroup, (oldVlaue, newVlaue) => newVlaue);
            return taskGroup;
        }

        /// <summary>
        /// 从缓存中拿已创建的TxTask组
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TxTaskGroup GetTxTaskGroup(string key)
        {
            taskMap.TryGetValue(key, out var txTaskGroup);
            return txTaskGroup;
        }

        /// <summary>
        /// 根据类型和key获取txTask(默认一种类型只有1个TxTask)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public TxTask GetTxTask(string key, string type="db")
        {
            string taskKey = type + "_" + key;
            if (taskMap.TryGetValue(key, out TxTaskGroup txGroup))
            {
                return txGroup.GetTxTasks().FirstOrDefault(r => r.Key.Equals(taskKey));
            }

            return null;
        }

        /// <summary>
        /// 从缓存中移除TxTaskGroup
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                taskMap.TryRemove(key, out _);
            }
        }
        #endregion
    }
}


