using System;
using System.Globalization;
using System.Text;

namespace LcnCsharp.Common.Utils
{
    public class KidUtils
    {

        public static string GetKid()
        {
            string kid = GenerateShortUuid();
            kid = DateTime.Now.ToString("yyyyMMddHHmmss") + kid;
            return kid;
        }

        public static string GetKKid()
        {
            string kid = GenerateShortUuid();
            kid = "k" + DateTime.Now.ToString("yyyyMMddHHmmss") + kid;
            return kid;
        }

        public static string GetUUID()
        {
            string kid = GenerateShortUuid();
            kid = DateTime.Now.ToString("yyyyMMddHHmmss") + "ud" + kid;
            return kid;
        }

        public static bool IsUUID(String uuid)
        {
            if (uuid != null)
            {
                if (uuid.Length == 24)
                {
                    String time = uuid.Substring(0, 14);
                    try
                    {
                        //DateUtil.parseDate(time, DateUtil.LOCATE_DATE_FORMAT);
                        if ("ud".Equals(uuid.Substring(14, 16)))
                        {
                            return true;
                        }
                    }
                    catch (System.Exception e)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private static string[] chars = new string[]{"a", "b", "c", "d", "e", "f",
            "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s",
            "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5",
            "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I",
            "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
            "W", "X", "Y", "Z"};


        public static string GenerateShortUuid()
        {
            StringBuilder shortBuffer = new StringBuilder();
            String uuid = Guid.NewGuid().ToString().Replace("-", "");
            for (int i = 0; i < 8; i++)
            {
                String str = uuid.Substring(i * 4, i * 4 + 4);
                int x = int.Parse(str, (NumberStyles) 16);
                shortBuffer.Append(chars[x % 0x3E]);
            }
            return shortBuffer.ToString();

        }

    }
}