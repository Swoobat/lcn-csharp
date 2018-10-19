namespace LcnCsharp.Manager.Core.Config
{
    public class ConfigReader
    {
        
        public int SocketPort { get; set; }


        public int SocketMaxConnection { get; set; }


        public int TransactionNettyHeartTime { get; set; }


        public int TransactionNettyDelayTime { get; set; }


        public int RedisSaveMaxTime { get; set; }



        public string CompensateNotifyUrl { get; set; }



        public bool IsCompensateAuto { get; set; }



        public int CompensateTryTime { get; set; }


        public int CompensateMaxWaitTime { get; set; }

        /**
         * 事务默认数据的位置，有最大时间
         */
        public  string Key_prefix = "tx:manager:default:";
        /**
         * 负载均衡模块存储信息
         */
        public  string Key_prefix_loadbalance = "tx:manager:loadbalance:";

        /**
         * 补偿事务永久存储数据
         */
        public  string Key_prefix_compensate = "tx:manager:compensate:";
    }
}