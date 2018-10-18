using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LcnCsharp.Manager.Core.Netty.Model
{
    public class TxGroup
    {
        public string GroupId { get; set; }

        public long StartTime { get; set; }

        public long NowTime { get; set; }

        public int State { get; set; }

        public int HasOver { get; set; }

        /**
         * 补偿请求
         */
        public int IsCompensate { get; set; }


        /**
         * 是否强制回滚(1:开启，0:关闭)
         */
        public int Rollback { get; set; } = 0;

        public List<TxInfo> Infos { get; set; }


        public TxGroup()
        {
            Infos = new List<TxInfo>();
        }


        public TxGroup Parser(string json)
        {
            try
            {
                var jsonObject = JObject.Parse(json);

                var txGroup = new TxGroup()
                {
                    GroupId = jsonObject["g"].ToString(),
                    StartTime = (long)jsonObject["st"],
                    NowTime = (long)jsonObject["nt"],
                    State = (int)jsonObject["s"],
                    IsCompensate = (int)jsonObject["i"],
                    Rollback = (int)jsonObject["r"],
                    HasOver = (int)jsonObject["o"],

                };
                var array = (JArray)jsonObject["l"];

                for (var i = 0; i < array.Count; i++)
                {
                    var info = new TxInfo();
                    info.Kid = array["k"][i].ToString();
                    info.ChannelAddress = array["ca"][i].ToString();
                    info.Notify = (int)array["n"][i];
                    info.IsGroup = (int)array["ig"][i];
                    info.Address = array["a"][i].ToString();
                    info.UniqueKey = array["u"][i].ToString();
                    info.Model = array["mn"][i].ToString();
                    info.ModelIpAddress = array["ip"][i].ToString();
                    info.MethodStr = array["ms"][i].ToString();
                    Infos.Add(info);
                }

                return txGroup;

            }
            catch (Exception e)
            {
                return null;
            }

        }

        public string ToJsonString(bool noList)
        {
            var jsonObject = new JObject
            {
                { "g",GroupId},
                { "st",StartTime},
                { "nt",NowTime},
                { "s",State},
                { "i",IsCompensate},
                { "r",Rollback},
                { "o",HasOver}

            };
            if (noList)
            {
                var jsonArray = new JArray();
                foreach (var info in Infos)
                {
                    JObject  item= new JObject();
                    item.Add("k",info.Kid);
                    item.Add("ca", info.ChannelAddress);
                    item.Add("n", info.Notify);
                    item.Add("ig", info.IsGroup);
                    item.Add("a", info.Address);
                    item.Add("u", info.UniqueKey);
                    item.Add("mn", info.Model);
                    item.Add("ip", info.ModelIpAddress);
                    item.Add("ms", info.MethodStr);
                    jsonArray.Add(item);
                }
                jsonObject.Add("l",jsonArray);
            }

            return jsonObject.ToString();
        }

        public string ToJsonTring()
        {
            return ToJsonString(true);
        }

    }
}