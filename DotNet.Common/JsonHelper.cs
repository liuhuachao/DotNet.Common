using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNet.Common
{
    /// <summary>
    /// Json 帮助类
    /// </summary>
    public class JsonHelper
    {
        public static string ObjectToJson<T>(T t)
        {
            string json = JsonConvert.SerializeObject(t);
            return json;
        }

        public static T JsonToObject<T>(string json)
        {
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }

        public static string DicToJson(Dictionary<string, string> t)
        {
            string json = JsonConvert.SerializeObject(t);
            return json;
        }

        public static Dictionary<string, string> JsonToDic(string json)
        {
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dic;
        }
    }
}
