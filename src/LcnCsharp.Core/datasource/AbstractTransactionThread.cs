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
