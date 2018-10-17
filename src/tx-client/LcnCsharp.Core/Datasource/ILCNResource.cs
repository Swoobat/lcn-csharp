using LcnCsharp.Core.Framework.Task;

namespace LcnCsharp.Core.Datasource
{
    public interface ILCNResource
    {
        /// <summary>
        /// 用于关闭时检查是否未删除
        /// </summary>
        /// <returns></returns>
        TxTask WaitTask { get; }

        /// <summary>
        /// 事务组id
        /// </summary>
        /// <returns></returns>
        string GroupId { get; }
    }
}
