using LcnCsharp.Manager.Core.Netty.Model;

namespace LcnCsharp.Manager.Core.Manager.Service
{
    public interface ITxManagerService
    {
        /**
        * 创建事物组
        *
        * @param groupId 补偿事务组id
        */
        TxGroup CreateTransactionGroup(string groupId);


        /**
         * 添加事务组子对象
         *
         * @return
         */

        TxGroup AddTransactionGroup(string groupId, string taskId, int isGroup, string channelAddress, string methodStr);


        /**
         * 关闭事务组
         * @param groupId 事务组id
         * @param state    事务状态
         * @return  0 事务存在补偿 1 事务正常  -1 事务强制回滚
         */
        int CloseTransactionGroup(string groupId, int state);


        void DealTxGroup(TxGroup txGroup, bool hasOk);


        /**
         * 删除事务组
         * @param txGroup 事务组
         */
        void DeleteTxGroup(TxGroup txGroup);


        /**
         * 获取事务组信息
         * @param groupId    事务组id
         * @return  事务组
         */
        TxGroup GetTxGroup(string groupId);


        /**
         * 获取事务组的key
         * @param groupId 事务组id
         * @return key
         */
        string GetTxGroupKey(string groupId);


        /**
         * 检查事务组数据
         * @param groupId   事务组id
         * @param taskId    任务id
         * @return  本次请求的是否提交 1提交 0回滚
         */
        int CleanNotifyTransaction(string groupId, string taskId);


        /**
         * 设置强制回滚事务
         * @param groupId 事务组id
         * @return  true 成功 false 失败
         */
        bool RollbackTransactionGroup(string groupId);
    }
}