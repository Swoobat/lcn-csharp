using DotNetty.Transport.Channels;
using System.Collections.Concurrent;

namespace LcnCsharp.Manager.Core.Utils
{
    public class SocketManager
    {
        public int MaxConnection { get; set; } = Constants.MaxConnection;

        public int NowConnection { get; set; }

        public bool AllowConnection { get; set; } = true;

        private BlockingCollection<IChannel> _clients = null;

        private ConcurrentDictionary<string, string> _lines = null;

        private static SocketManager _manager = null;

        private  static object _lock =new object();

        public static SocketManager GetInstance()
        {
            if (_manager != null) return _manager;
            lock (_lock)
            {
                if (_manager == null)
                {
                    // ReSharper disable once PossibleMultipleWriteAccessInDoubleCheckLocking
                    _manager =new SocketManager();
                }
            }

            return _manager;
        }

        public IChannel GetChannelByModelName(string name)
        {
            foreach (var channel in _clients)
            {
                var modelName = channel.RemoteAddress.ToString();

                if (modelName.Equals(name))
                {
                    return channel;
                }
            }

            return null;
        }

        private SocketManager()
        {
            _clients = new BlockingCollection<IChannel>();
            _lines = new ConcurrentDictionary<string, string>();
        }

        public void AddClient(IChannel client)
        {
            _clients.Add(client);
            NowConnection = _clients.Count;
            AllowConnection = (MaxConnection != NowConnection);
        }

        public void RemoveClient(IChannel client)
        {
            _clients.TryTake(out client);
            NowConnection = _clients.Count;
            AllowConnection = (MaxConnection != NowConnection);
        }

        public void OutLine(string modelName)
        {
            _lines.TryRemove(modelName, out string value);
        }

        public void OnLine(string modleName, string uniqueKey)
        {
            _lines.TryAdd(modleName, uniqueKey);
        }

        public IChannel GetChannelByUniqueKey(string uniqueKey)
        {
            foreach (var channel in _clients)
            {
                var modelName = channel.RemoteAddress.ToString();
                _lines.TryGetValue(modelName, out var value);
                if (uniqueKey.Equals(value))
                {
                    return channel;
                }
               
            }
            return null;
        }



    }
}
