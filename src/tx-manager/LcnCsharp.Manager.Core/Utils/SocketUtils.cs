using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;

namespace LcnCsharp.Manager.Core.Utils
{
    public class SocketUtils
    {
        public static string GetJson(object msg)
        {
            string json;
            try
            {
                var buf = (IByteBuffer)msg;
                var bytes = new byte[buf.ReadableBytes];
                buf.ReadBytes(bytes);
                json = bytes.ToString();
            }
            finally
            {
                ReferenceCountUtil.Release(msg);
            }
            return json;

        }

        public static void SendMsg(IChannelHandlerContext ctx, string msg)
        {
            ctx.WriteAndFlushAsync(Unpooled.Buffer().WriteBytes(Encoding.UTF8.GetBytes(msg)));
        }


        public static void SendMsg(IChannel ctx, String msg)
        {
            ctx.WriteAndFlushAsync(Unpooled.Buffer().WriteBytes(Encoding.UTF8.GetBytes(msg)));
        }
    }
}