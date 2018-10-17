using System.Collections.Generic;

namespace LcnCsharp.Manager.Core.Model
{
    public class TxState
    {
        /**
        * socket ip
        */
        public string Ip { get; set; }
        /**
         * socket port
         */
        public int Port { get; set; }

        /**
         * max connection
         */
        public int MaxConnection { get; set; }

        /**
         * now connection
         */
        public int NowConnection { get; set; }


        /**
         *  transaction_netty_heart_time
         */
        public int TransactionNettyHeartTime { get; set; }

        /**
         *  transaction_netty_delay_time
         */
        public int TransactionNettyDelayTime { get; set; }


        /**
         * redis_save_max_time
         */
        public int RedisSaveMaxTime { get; set; }


        /**
         * 回调地址
         */
        public string NotifyUrl { get; set; }

        /**
         * 自动补偿
         */
        public bool IsCompensate { get; set; }

        /**
         * 补偿尝试时间
         */
        public int CompensateTryTime { get; set; }

        /**
         * slb list
         */
        public List<string> SlbList { get; set; }

        /**
         * 自动补偿间隔时间
         */
        public int CompensateMaxWaitTime { get; set; }
    }
}