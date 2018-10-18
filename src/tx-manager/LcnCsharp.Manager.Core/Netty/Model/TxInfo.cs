using System;
using DotNetty.Codecs.Json;
using LcnCsharp.Manager.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LcnCsharp.Manager.Core.Netty.Model
{
    public class TxInfo
    {
        /**
     * 任务唯一标示
     */
        public string Kid { get; set; }

        /**
         * 模块管道名称（netty管道名称）
         */
        public string ChannelAddress { get; set; }
        /**
         * 是否通知成功
         */
        public int Notify { get; set; }

        /**
         * 0 不组合
         * 1 组合
         */
        public int IsGroup { get; set; }

        /**
         * tm识别标示
         */
        public string Address { get; set; }

        /**
         * tx识别标示
         */
        public string UniqueKey { get; set; }


        /**
         * 管道发送数据
         */
        public ChannelSender Channel { get; set; }


        /**
         * 业务方法名称
         */
        public string MethodStr { get; set; }

        /**
         * 模块名称
         */
        public string Model { get; set; }

        /**
         * 模块地址
         */
        public string ModelIpAddress { get; set; }

        /**
         * 是否提交（临时数据）
         */
        public int IsCommit { get; set; }


        public override string ToString()
        {
            var jsonObject = new JObject
            {
                { "kid",Kid},
                { "channelAddress",ChannelAddress},
                { "notify",Notify},
                { "notify",Notify},
                { "isGroup",IsGroup},
                { "address",Address},
                { "uniqueKey",UniqueKey},
                { "model",Model},
                { "modelIpAddress",ModelIpAddress},
                { "methodStr",MethodStr}

            };
            return jsonObject.ToString();

        }
    }
}