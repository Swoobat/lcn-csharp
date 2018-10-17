using DotNetty.Transport.Channels;
using LcnCsharp.Common.Utils.Task;
using LcnCsharp.Manager.Core.Utils;

namespace LcnCsharp.Manager.Core.Model
{
    public class ChannelSender
    {
        public class  Back:IBack
        {
            public  string Res { get; set; }
            public object Doing(params object[] objs)
            {
                return Res;
            }
        }

        
        public IChannel Channel { private get;  set; }
        public  string Address { private get; set; }
        public string ModelName { private get; set; }

        public void Send(string msg)
        {
            if (Channel !=null)
            {
                SocketUtils.SendMsg(Channel,msg);
            }
        }

        public void Send(string msg, Task task)
        {
            if (Channel != null)
            {
                SocketUtils.SendMsg(Channel, msg);
            }
            else
            {
                var url = $"http://{Address}/tx/manager/sendMsg";
                string res = "aa"; // HttpUtils.post(url, "msg=" + msg + "&model=" + modelName);
                if (string.IsNullOrEmpty(res))
                {
                    if (task != null)
                    {
                        task.SetBack(new Back(){Res = res});
                        task.SignalTask();
                    }
                }
                else
                {
                    if (task != null)
                    {
                        task.SetBack(new Back(){Res ="2"});
                        task.SignalTask();
                    }
            }
            }
        }
    }
}