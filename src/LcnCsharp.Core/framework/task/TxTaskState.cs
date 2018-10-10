namespace LcnCsharp.Core.framework.task
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
