namespace LcnCsharp.Manager.Core.Model
{
    public class TxServer
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public int Heart { get; set; }
        public int Delay { get; set; }
        public int CompensateMaxWaitTime { get; set; }

        public static TxServer Format(TxState state)
        {
            var txServer = new TxServer()
            {
                Ip = state.Ip,
                Port = state.Port,
                Heart = state.TransactionNettyHeartTime,
                Delay = state.TransactionNettyDelayTime,
                CompensateMaxWaitTime = state.CompensateMaxWaitTime
            };
            return txServer;
        }
    }
}