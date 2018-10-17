using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DotNet
{
    public class WeixinHelper
    {
        public WeixinHelper() { }

        /// <summary>
        /// 取得Access Token
        /// </summary>
        /// <param name="config">接口参数配置文件</param>
        /// <param name="code">临时Authorization Code</param>
        /// <returns></returns>
        public static Dictionary<string, object> get_access_token(string app_id, string app_key, string code = "")
        {
            string send_url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + app_id + "&secret=" + app_key;
            if (!string.IsNullOrEmpty(code))
            {
                send_url += "&code=" + code + "&grant_type=authorization_code";
            }
            string result = HttpHelper.HttpGet(send_url);
            if (result.Contains("errcode"))
            {
                return null;
            }
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (dic.Count > 0)
                {
                    return dic;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 刷新Access Token
        /// </summary>
        /// <param name="refresh_token">用户刷新Access Token</param>
        /// <returns></returns>
        public static Dictionary<string, object> get_refresh_token(string app_id, string refresh_token)
        {
            string send_url = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + app_id + "&grant_type=refresh_token&refresh_token=" + refresh_token;
            string result = HttpHelper.HttpGet(send_url);
            if (result.Contains("errcode"))
            {
                return null;
            }
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (dic.Count > 0)
                {
                    return dic;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token">网页授权凭证</param>
        /// <param name="open_id">用户唯一标识</param>
        /// <returns></returns>
        public static Dictionary<string, object> get_user_info(string access_token, string open_id)
        {
            string send_url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + open_id + "&lang=zh_CN";
            string result = HttpHelper.HttpGet(send_url);
            if (result.Contains("errcode"))
            {
                return null;
            }
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (dic.Count > 0)
                {
                    return dic;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        public bool CheckSignature(string token, string timestamp, string nonce, string signature)
        {
            string[] args = { token, timestamp, nonce };
            var _signature = CreateSignature(args);

            if (_signature.ToLower() == signature.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 生成微信签名
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string CreateSignature(string[] args)
        {
            Array.Sort(args);
            string signature = string.Join("", args);
            signature = Sha1(signature).ToLower();
            return signature;
        }

        /// <summary>
        /// 生成微信签名
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string CreateSignature(Dictionary<string, string> dic)
        {
            dic = dic.OrderBy(d => d.Key).ToDictionary(k => k.Key, v => v.Value);
            IList<string> list = new List<string>();
            foreach (var item in dic)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            string signature = string.Join("&", list);
            signature = Sha1(signature);
            signature = signature.ToLower();
            return signature;
        }

        /// <summary>
        /// JSSDK 生成微信签名
        /// </summary>
        /// <param name="jsapi_ticket"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CreateSignature(string jsapi_ticket, string noncestr, string timestamp, string url)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("jsapi_ticket", jsapi_ticket);
            dic.Add("noncestr", noncestr);
            dic.Add("timestamp", timestamp);
            dic.Add("url", url);
            var signature = CreateSignature(dic);
            return signature;
        }

        /// <summary>
        /// sha1加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Sha1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

    }
}
