using System;
using System.Collections.Concurrent;
using LcnCsharp.Manager.Core.Model;

namespace LcnCsharp.Manager.Core.Manager
{
    public class ModelInfoManager
    {
        private BlockingCollection<ModelInfo> modelInfos = new BlockingCollection<ModelInfo>();
        private static ModelInfoManager _manager = null;
        private static  readonly  object _lock=new object();

        public static ModelInfoManager GetInstance()
        {
            if (_manager == null)
            {
                lock (_lock)
                {
                    if (_manager == null)
                    {
                        _manager = new ModelInfoManager();
                    }
                }
            }
            return _manager;
        }

        public void RemoveModelInfo(string channelName)
        {
            foreach (var modelInfo in modelInfos)
            {
                var flag = string.Equals(channelName, modelInfo.ChannelName, StringComparison.OrdinalIgnoreCase);
                if (flag)
                {
                    var info = modelInfo;
                    modelInfos.TryTake(out info);
                }

               
            }
        }

        public void AddModelInfo(ModelInfo modelInfo)
        {
            foreach (var item in modelInfos)
            {
                var flagName = string.Equals(item.ChannelName, modelInfo.ChannelName, StringComparison.OrdinalIgnoreCase);
                if (flagName)
                {
                    return;
                }
                var flagAddress = string.Equals(item.IpAddress, modelInfo.IpAddress, StringComparison.OrdinalIgnoreCase);
                if (flagAddress)
                {
                    return;
                }

            }
            modelInfos.Add(modelInfo);
        }

        public BlockingCollection<ModelInfo> GetOnlines()
        {
            return modelInfos;
        }


        public ModelInfo GetModelByChannelName(string channelName)
        {
            foreach (var modelInfo in modelInfos)
            {
                var flagName = string.Equals(modelInfo.ChannelName, channelName, StringComparison.OrdinalIgnoreCase);
                if (flagName)
                {
                    return modelInfo;
                }
            }
            return null;
        }


        public ModelInfo GetModelByModel(string model)
        {
            foreach (var modelInfo in modelInfos)
            {
                var flagModel = string.Equals(modelInfo.Model, model, StringComparison.OrdinalIgnoreCase);
                if (flagModel)
                {
                    return modelInfo;
                }
            }
            return null;
        }

    }
}