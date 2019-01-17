using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common
{
    /// <summary>
    /// 配置文件(AppSettings)操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        public static bool GetConfigBool(string key)
        {
            bool flag = false;
            string configString = GetConfigString(key);
            if ((configString != null) && (string.Empty != configString))
            {
                try
                {
                    flag = bool.Parse(configString);
                }
                catch (FormatException)
                {
                }
            }
            return flag;
        }

        public static string GetConfigString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object cache = CacheHelper.Get(cacheKey);
            if (cache == null)
            {
                try
                {
                    cache = ConfigurationManager.AppSettings[key];
                    if (cache != null)
                    {
                        CacheHelper.Insert(cacheKey, cache,"", DateTime.UtcNow.AddMinutes(180.0), TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return cache.ToString();
        }
    }
}
