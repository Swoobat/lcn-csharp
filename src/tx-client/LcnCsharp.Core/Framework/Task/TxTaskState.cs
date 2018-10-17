namespace LcnCsharp.Core.Framework.Task
{
    /// <summary>
    /// 信号器执行状态
    /// </summary>
    public enum TxTaskState
    {
        /// <summary>
        /// 网络错误
        /// </summary>
        NetWorkError = -1,
        /// <summary>
        /// 超时
        /// </summary>
        NetworkTimeOut = -2,
        /// <summary>
        /// 连接错误
        /// </summary>
        ConnectionError = -3,
        /// <summary>
        /// 回滚
        /// </summary>
        Rollback = 0,
        /// <summary>
        /// 正常提交
        /// </summary>
        Commit = 1,
    }
}
