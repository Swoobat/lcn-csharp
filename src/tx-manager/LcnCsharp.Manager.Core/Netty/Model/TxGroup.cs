using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        public List<TxInfo> Infos;

        public TxGroup()
        {
            Infos=new List<TxInfo>();
        }


        public static TxGroup parser(String json)
        {
            try
            {
                //JSONObject jsonObject = JSONObject.parseObject(json);
                //TxGroup txGroup = new TxGroup();
                //txGroup.setGroupId(jsonObject.getString("g"));
                //txGroup.setStartTime(jsonObject.getLong("st"));
                //txGroup.setNowTime(jsonObject.getLong("nt"));
                //txGroup.setState(jsonObject.getInteger("s"));
                //txGroup.setIsCompensate(jsonObject.getInteger("i"));
                //txGroup.setRollback(jsonObject.getInteger("r"));
                //txGroup.setHasOver(jsonObject.getInteger("o"));
                //JSONArray array = jsonObject.getJSONArray("l");
                //int length = array.size();
                //for (int i = 0; i < length; i++)
                //{
                //    JSONObject object = array.getJSONObject(i);
                //    TxInfo info = new TxInfo();
                //    info.setKid(object.getString("k"));
                //    info.setChannelAddress(object.getString("ca"));
                //    info.setNotify(object.getInteger("n"));
                //    info.setIsGroup(object.getInteger("ig"));
                //    info.setAddress(object.getString("a"));
                //    info.setUniqueKey(object.getString("u"));

                //    info.setModel(object.getString("mn"));
                //    info.setModelIpAddress(object.getString("ip"));
                //    info.setMethodStr(object.getString("ms"));

                //    txGroup.getList().add(info);
                //}

                //var jsonObject = JsonConvert.DeserializeObject(json);
                //return txGroup;
                return null;

            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}