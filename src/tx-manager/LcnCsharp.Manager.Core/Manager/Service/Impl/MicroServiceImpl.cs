using LcnCsharp.Manager.Core.Model;

namespace LcnCsharp.Manager.Core.Manager.Service.Impl
{
    public class MicroServiceImpl:IMicroService
    {
        public string GetTmKey(string tmkey = "tx-manager")
        {
            throw new System.NotImplementedException();
        }

        public TxServer GetServer()
        {
            throw new System.NotImplementedException();
        }

        public TxState GetState()
        {
            throw new System.NotImplementedException();
        }
    }
}