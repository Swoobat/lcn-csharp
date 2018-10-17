using System.Collections.Concurrent;

namespace LcnCsharp.Common.Utils.Task
{
    public class ConditionUtils
    {
        private static ConditionUtils _instance = null;

        private static readonly object _object = new object();

        private ConcurrentDictionary<string, Task> _taskMap = new ConcurrentDictionary<string, Task>();

        public static ConditionUtils GetInstance()
        {
            if (_instance == null)
            {
                lock (_object)
                {
                    if (_instance == null)
                    {
                        _instance = new ConditionUtils();
                    }
                }
            }

            return _instance;
        }

        public Task CreateTask(string key)
        {
            var task = new Task();
            task.Key = key;
            _taskMap.TryAdd(key, task);
            return task;
        }

        public Task GeTask(string key)
        {
            Task task = null;
            _taskMap.TryGetValue(key, out task);
            return task;
        }

        public void RemoveKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _taskMap.TryRemove(key, out var value);
            }
        }

        public bool HasKey(string key)
        {
            return _taskMap.ContainsKey(key);
        }
    }
}