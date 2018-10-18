using LcnCsharp.Manager.Core.Model;

namespace LcnCsharp.Manager.Core.Manager.Service
{
    public interface ILoadBalanceService
    {
        bool Put(LoadBalanceInfo loadBalanceInfo);

        LoadBalanceInfo Get(string groupId, string key);

        bool Remove(string groupId);

        string GetLoadBalanceGroupName(string groupId);
    }
}