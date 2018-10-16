using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace LcnCsharp.Core.datasource
{
    public abstract class AbstractTransactionThread
    {
        private volatile bool hasStartTransaction = false;

        protected void StartRunnable()
        {
            if (hasStartTransaction)
            {
                return;
            }

            new Thread(() =>
            {
                try
                {
                    Transaction();
                }
                catch (Exception e)
                {
                    try
                    {
                        RollbackConnection();
                    }
                    catch (DbException e1)
                    {
                    }
                }
                finally
                {
                    try
                    {
                        CloseConnection();
                    }
                    catch (DbException e)
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
