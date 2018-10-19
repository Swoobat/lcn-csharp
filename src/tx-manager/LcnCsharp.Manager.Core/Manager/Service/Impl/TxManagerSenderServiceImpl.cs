using LcnCsharp.Common.Logging;
using LcnCsharp.Manager.Core.Config;
using LcnCsharp.Manager.Core.Netty.Model;
using LcnCsharp.Manager.Core.Redis.Service;
using Microsoft.Extensions.Logging;

namespace LcnCsharp.Manager.Core.Manager.Service.Impl
{
    public class TxManagerSenderServiceImpl:ITxManagerSenderService
    {
        private readonly ILogger _logger =
            LcnCsharpLogManager.LoggerFactory.CreateLogger(typeof(TxManagerSenderServiceImpl));

        private ITxManagerService txManagerService;
        private readonly IRedisServerService _redisServerService;
        private readonly ConfigReader configReader;
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