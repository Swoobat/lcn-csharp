using LcnCsharp.Manager.Core.Netty.Model;

namespace LcnCsharp.Manager.Core.Manager.Service
{
    public interface ITxManagerSenderService
    {
        int Confirm(TxGroup group);

        string SendMsg(string model, string msg, int delay);

        string SendCompensateMsg(string model, string groupId, string data, int startState);
    }
}