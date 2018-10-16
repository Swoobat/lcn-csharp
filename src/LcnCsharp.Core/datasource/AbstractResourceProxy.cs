using System.Collections.Concurrent;

namespace LcnCsharp.Core.datasource
{
    public abstract class AbstractResourceProxy<C, T> : ILCNTransactionControl where T : ILCNResource
    {
        protected ConcurrentDictionary<string, ILCNResource> pools = new ConcurrentDictionary<string, ILCNResource>();

        //default size
        protected volatile int maxCount = 5;

        //default time (seconds)
        protected int maxWaitTime = 30;

        protected volatile int nowCount = 0;


        protected volatile bool hasTransaction = false;

        private volatile bool isNoTransaction = false;

        public bool HasGroup(string @group)
        {
            return pools.ContainsKey(group);
        }

        public bool ExecuteTransactionOperation()
        {
            return hasTransaction;
        }

        public bool IsNoTransactionOperation()
        {
            return isNoTransaction;
        }

        public void AutoNoTransactionOperation()
        {
            isNoTransaction = true;
        }

        public void SetMaxWaitTime(int _maxWaitTime)
        {
            this.maxWaitTime = _maxWaitTime;
        }

        public void SetMaxCount(int _maxCount)
        {
            this.maxCount = _maxCount;
        }
    }
}
