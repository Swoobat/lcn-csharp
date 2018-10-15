using System;
using System.Collections.Generic;
using System.Text;
using LcnCsharp.Core.framework.task;

namespace LcnCsharp.Core.datasource
{
    public interface ILCNResource
    {
        /// <summary>
        /// 用于关闭时检查是否未删除
        /// </summary>
        /// <returns></returns>
        TxTask GetWaitTask();

        /// <summary>
        /// 事务组id
        /// </summary>
        /// <returns></returns>
        String GetGroupId();
    }
}
