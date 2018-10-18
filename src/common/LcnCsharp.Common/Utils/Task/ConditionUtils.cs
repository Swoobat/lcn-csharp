using System.Collections.Concurrent;

namespace LcnCsharp.Common.Utils.Task
{
    public class ConditionUtils
    {
        private static readonly ConditionUtils _instance = new ConditionUtils();

        private static readonly object _object = new object();

        private readonly ConcurrentDictionary<string, Task> _taskMap = new ConcurrentDictionary<string, Task>();

        public static ConditionUtils GetInstance()
        {
            return _instance;
        }

        public Task CreateTask(string key)
        {
            var task = new Task(key);
            _taskMap.TryAdd(key, task);
            return task;
        }

        public Task GeTask(string key)
        {
            _taskMap.TryGetValue(key, out var task);
            return task;
        }

        public void RemoveKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
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