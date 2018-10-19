using LcnCsharp.Manager.Core.Netty.Model;

namespace LcnCsharp.Manager.Core.Compensate.Model
{
    public class TransactionCompensateMsg
    {
        public long CurrentTime { get; set; }
        public string GroupId{ get; set; }
        public string Model { get; set; }
        public string Address { get; set; }
        public string UniqueKey { get; set; }
        public string ClassName { get; set; }
        public string MethodStr { get; set; }
        public string Data { get; set; }
        public int Time { get; set; }
        public int StartError { get; set; }

        public TxGroup txGroup { get; set; }

        public int State { get; set; }


        public TransactionCompensateMsg(long currentTime, string groupId, string model, string address,
            string uniqueKey, string className,
            string methodStr, string data, int time, int state, int startError)
        {
            CurrentTime = currentTime;
            GroupId = groupId;
            Model = model;
            UniqueKey = uniqueKey;
            ClassName = className;
            MethodStr = methodStr;
            Data = data;
            Time = time;
            Address = address;
            StartError = state;
            StartError = startError;
        }
    }
}