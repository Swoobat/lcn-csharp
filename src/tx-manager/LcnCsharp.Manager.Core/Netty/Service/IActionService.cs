using Newtonsoft.Json.Linq;

namespace LcnCsharp.Manager.Core.Netty.Service
{
    public interface IActionService
    {
        string Execute(string channelAddress, string key, JObject param);
    }
}