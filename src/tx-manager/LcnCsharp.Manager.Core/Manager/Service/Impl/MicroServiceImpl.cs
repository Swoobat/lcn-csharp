using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using LcnCsharp.Manager.Core.Config;
using LcnCsharp.Manager.Core.Model;
using LcnCsharp.Manager.Core.Utils;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.Eureka.AppInfo;

namespace LcnCsharp.Manager.Core.Manager.Service.Impl
{
    public class MicroServiceImpl:IMicroService
    {
        private DiscoveryClient _discoveryClient;

        private ConfigReader _configReader;
        public string GetTmKey(string tmkey = "tx-manager")
        {
            return tmkey;
        }

        public TxServer GetServer()
        {
            List<string> urls = GetServices();
            List<TxState> states = new List<TxState>();
            foreach (var url in urls)
            {
                try
                {
                    //TxState state = restTemplate.getForObject(url + "/tx/manager/state", TxState.class);
                    //states.add(state);
                }
                catch (Exception e)
                {
                }

            }
            if(states.Count>1) {
                TxState state = GetState();
                if (state.MaxConnection > state.NowConnection) {
                    return TxServer.Format(state);
                } else {
                    return null;
                }
            }else{
                //找默认数据
                TxState state = GetDefault(states, 0);
                if (state == null) {
                    //没有满足的默认数据
                    return null;
                }
                return TxServer.Format(state);
            }
        }

        public TxState GetState()
        {
            TxState state = new TxState();
            var ipAddress = GetLocalIPAddress();
            state.Ip = ipAddress;
            state.Port = Constants.SocketPort;
            state.MaxConnection = SocketManager.GetInstance().MaxConnection;
            state.NowConnection = SocketManager.GetInstance().NowConnection;
            state.RedisSaveMaxTime = _configReader.RedisSaveMaxTime;
            state.TransactionNettyDelayTime = _configReader.TransactionNettyDelayTime;
            state.NotifyUrl = _configReader.CompensateNotifyUrl;
            state.IsCompensate = _configReader.IsCompensateAuto;
            state.CompensateTryTime = _configReader.CompensateTryTime;
            state.SlbList = GetServices();
            return state;
        }

        private List<string> GetServices()
        {
            var urls =new List<string>();
            var serviceInstances = _discoveryClient.GetInstanceById(GetTmKey()).ToList();
            foreach (var instanceInfo in serviceInstances)
            {
                urls.Add(instanceInfo.IpAddr);
            }
            return urls;
        }

        private  string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private TxState GetDefault(List<TxState> states, int index)
        {
            var state = states[index];
            if (state.MaxConnection == state.NowConnection)
            {
                index++;
                if (states.Count - 1 >= index)
                {
                    return GetDefault(states, index);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return state;
            }
        }
    }
}