using LcnCsharp.Manager.Core.Netty.Model;

namespace LcnCsharp.Manager.Core.Manager.Service.Impl
{
    public class TxManagerServiceImpl:ITxManagerSenderService
    {
        public int Confirm(TxGroup @group)
        {
            throw new System.NotImplementedException();
        }

        public string SendMsg(string model, string msg, int delay)
        {
            throw new System.NotImplementedException();
        }

        public string SendCompensateMsg(string model, string groupId, string data, int startState)
        {
            throw new System.NotImplementedException();
        }
    }
}