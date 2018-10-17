using System;
using System.Data.Common;
using System.Threading;

namespace LcnCsharp.Core.Datasource
{
    public abstract class AbstractTransactionThread
    {

        protected void StartRunnable()
        {

            new Thread(() =>
            {
                try
                {
                    Transaction();
                }
                catch (Exception)
                {
                    try
                    {
                        RollbackConnection();
                    }
                    catch (DbException)
                    {
                    }
                }
                finally
                {
                    try
                    {
                        CloseConnection();
                    }
                    catch (DbException)
                    {
                    }
                }
            }).Start();

        }

        protected abstract void Transaction();

        protected abstract void CloseConnection();

        protected abstract void RollbackConnection();
    }
}
