using LcnCsharp.Manager.Core.Config;
using LcnCsharp.Manager.Core.Model;
using LcnCsharp.Manager.Core.Redis.Service;

namespace LcnCsharp.Manager.Core.Manager.Service.Impl
{
    public class LoadBalanceServiceImpl:ILoadBalanceService
    {
        private readonly IRedisServerService _redisServerService;
        private readonly ConfigReader _configReader;
        public bool Put(LoadBalanceInfo loadBalanceInfo)
        {
            var groupName = GetLoadBalanceGroupName(loadBalanceInfo.GroupId);
            _redisServerService.SaveLoadBalance(groupName,loadBalanceInfo.Key,loadBalanceInfo.Data);
            return true;
        }

        public LoadBalanceInfo Get(string groupId, string key)
        {
            var groupName = GetLoadBalanceGroupName(groupId);
            var bytes = _redisServerService.GetLoadBalance(groupName, key);
            if (bytes == null)
            {
                return null;
            }

            LoadBalanceInfo loadBalanceInfo=new LoadBalanceInfo()
            {
                GroupId = groupId,
                Key = key,
                Data = bytes
            };
            return loadBalanceInfo;
        }

        public bool Remove(string groupId)
        {
            _redisServerService.DeleteKey(GetLoadBalanceGroupName(groupId));
            return true;
        }

        public string GetLoadBalanceGroupName(string groupId)
        {
            return _configReader.Key_prefix_loadbalance + groupId;
        }
    }
}