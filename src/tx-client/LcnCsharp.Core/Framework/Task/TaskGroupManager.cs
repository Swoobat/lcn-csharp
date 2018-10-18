using LcnCsharp.Common.Utils.Task;
using System.Collections.Concurrent;
using System.Linq;

namespace LcnCsharp.Core.Framework.Task
{
    public class TaskGroupManager
    {
        #region Field
        private static readonly TaskGroupManager instance = new TaskGroupManager();
        private readonly ConcurrentDictionary<string, TaskGroup> taskMap = new ConcurrentDictionary<string, TaskGroup>();

        #endregion

        #region Constructor

        private TaskGroupManager() { }

        #endregion

        #region Static
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static TaskGroupManager GetInstance()
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
        public TaskGroup CreateTask(string key, string type)
        {
            TaskGroup taskGroup = GetTaskGroup(key);
            if (taskGroup == null)
            {
                taskGroup = new TaskGroup(key);
            }

            string taskKey = type + "_" + key;

            TxTask task = new TxTask(ConditionUtils.GetInstance().CreateTask(taskKey));
            taskGroup.CurrentTask = task;
            taskGroup.AddTask(task);
            taskMap.AddOrUpdate(key, taskGroup, (oldVlaue, newVlaue) => newVlaue);
            return taskGroup;
        }

        /// <summary>
        /// 从缓存中拿已创建的TxTask组
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TaskGroup GetTaskGroup(string key)
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
        public TxTask GetTask(string key, string type = "db")
        {
            string taskKey = type + "_" + key;
            if (taskMap.TryGetValue(key, out TaskGroup txGroup))
            {
                return txGroup.GetTasks().FirstOrDefault(r => r.Key.Equals(taskKey));
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


