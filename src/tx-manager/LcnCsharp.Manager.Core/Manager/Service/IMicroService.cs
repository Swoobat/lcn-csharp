using LcnCsharp.Manager.Core.Model;

namespace LcnCsharp.Manager.Core.Manager.Service
{
    public interface IMicroService
    {
        string GetTmKey(string tmkey= "tx-manager");
        TxServer GetServer();

        TxState GetState();
    }
}