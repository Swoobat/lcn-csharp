namespace LcnCsharp.Core.Framework.Task
{
    public enum TxTaskState
    {
        NetWorkError = -1,
        NetworkTimeOut = -2,
        ConnectionError = -3,
        Rollback = 0,
        Commit = 1,
    }
}
