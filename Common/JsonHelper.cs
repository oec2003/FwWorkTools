using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace FW.CommonFunction
{
    public class JsonHelper
    {
        public static string EntityToJson<T>(T data)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(data.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, data);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                return null;
            }
        }

        public static string EntityListToJson<T>(List<T> data)
        {
            try
            {
                StringBuilder sbJoin = new StringBuilder();
                foreach (T t in data)
                {
                    var josn = EntityToJson<T>(t);
                    if (!string.IsNullOrWhiteSpace(josn))
                    {
                        sbJoin.Append(josn + ",");
                    }
                }
                sbJoin.Remove(sbJoin.Length - 1, 1);
                sbJoin.Insert(0, "[");
                sbJoin.Append("]");
                return sbJoin.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static List<T> JsonToEntity<T>(string json)
        {
            json=@"{'CustomerName':'111','CustomerArea':'222','VSSAddress':'333','ProjVersion':'','DBName':'','ServerPort':'','ServerName':'','ApplicationName':'','UserName':'','UserPwd':'','Remark':''}";
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<T>));
            StreamWriter wr = new StreamWriter(stream);
            wr.Write(json);
            wr.Flush();
            stream.Position = 0;
            Object obj = new Object();
            try
            {
                obj = ser.ReadObject(stream);
            }
            catch
            {
                return null;
            }
            List<T> list = (List<T>)obj;
            return list;
        }
    }
}
