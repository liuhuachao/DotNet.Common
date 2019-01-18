using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace DotNet.Common
{
    /// <summary>
    /// IP 操作类
    /// </summary>
    public class IPHelper
    {
        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            HttpRequest Request = HttpContext.Current.Request;
            string userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIP == null || userIP == "")
            {
                userIP = GetHostAddress();
            }
            else
            {
                if (userIP.ToString().Contains(','))
                {
                    userIP = userIP.ToString().Split(',')[0].Trim();
                }
            }

            if (!string.IsNullOrEmpty(userIP) && RegexHelper.IsMatch(userIP,RegexHelper.IPV4Pattern))
            {
                return userIP;
            }
            else
            {
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && RegexHelper.IsMatch(userHostAddress, RegexHelper.IPV4Pattern))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 获取IP所在地
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetAddress(string ip)
        {
            string address = string.Empty;
            try
            {
                string JosnIP = GetIPInfo(ip);
                var dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(ip);
                address = ((Dictionary<string, object>)((Dictionary<string, object>)dic["content"])["address_detail"])["city"].ToString();
            }
            catch
            {
                address = string.Empty;
            }
            return address;
        }

        /// <summary>
        /// 获取IP信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetIPInfo(string ip)
        {
            string JosnIP = "{}";
            if (RegexHelper.IsMatch(ip, RegexHelper.IPV4Pattern))
            {
                string mapBaiduUrl = "http://api.map.baidu.com/location/ip?ak=cV5LotxgWTgnUoPOYNsdpCyScpl1cwq0&ip=" + ip;
                JosnIP = HttpHelper.HttpGet(mapBaiduUrl);
            }
            return JosnIP;
        }
    }
}
