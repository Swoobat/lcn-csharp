using LcnCsharp.Common.Utils.Task;
using _Task = LcnCsharp.Common.Utils.Task.Task;
namespace LcnCsharp.Core.Framework.Task
{
    /// <summary>
    /// 信号器
    /// </summary>
    public class TxTask : _Task
    {
        private readonly _Task _task;

        public TxTask(_Task task)
        {
            this._task = task;
        }

        public new bool IsNotify()
        {
            return this._task.IsNotify();
        }

        public new bool IsRemove()
        {
            return this._task.IsRemove();
        }

        public new bool IsAwait()
        {
            return this._task.IsAwait();
        }

        public new int GetState()
        {
            return this._task.GetState();
        }

        public new void SetState(int state)
        {
            this._task.SetState(state);
        }

        public new string Key
        {
            get => this._task.Key;
            set => this._task.Key = value;
        }

        public new IBack GetBack()
        {
            return this._task.GetBack();
        }

        public new void SetBack(IBack back)
        {
            this._task.SetBack(back);
        }

        public new void SignalTask()
        {
            this._task.SignalTask();
        }



        public new void SignalTask(IBack back)
        {
            this._task.SignalTask(back);
        }

        public new void AwaitTask()
        {
            this._task.AwaitTask();
        }

        public new void AwaitTask(IBack back)
        {
            this._task.AwaitTask(back);
        }

        public new void Remove()
        {
            _task.Remove();
            bool hasData = true;//true没有，false有

            string groupKey = this.Key.Split('_')[1];
            TaskGroup taskGroup = TaskGroupManager.GetInstance().GetTaskGroup(groupKey);
            foreach (var task in taskGroup.GetTasks())
            {
                if (!task.IsRemove())
                {
                    hasData = false;
                }
            }

            if (hasData)
            {
                TaskGroupManager.GetInstance().RemoveKey(groupKey);
            }
        }
    }
}
