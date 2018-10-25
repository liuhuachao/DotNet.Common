using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace DotNet
{
    /// <summary>
    /// 微信授权使用的是OAuth2.0授权的方式，主要步骤如下：
    /// 第一步：用户同意授权，获取code
    /// 第二步：通过code换取网页授权access_token
    /// 第三步：刷新access_token（如果需要）
    /// 第四步：拉取用户信息(需scope为 snsapi_userinfo)
    /// </summary>
    public class WeixinHelper
    {
        public WeixinHelper() { }

        /// <summary>
        /// 获取code
        /// </summary>
        /// <param name="app_id"></param>
        /// <param name="app_key"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetCodeUrl(string appId, string redirectUri, string scope = "snsapi_userinfo", string device = "mobile")
        {
            string state = Guid.NewGuid().ToString().Replace("-", "");
            string weixinCodeUrl = System.Configuration.ConfigurationManager.AppSettings["WeixinCodeUrl"];
            string send_url = string.Format("{0}/get-weixin-code.html?appid={1}&scope={2}&state={3}&redirect_uri={4}&device={5}",weixinCodeUrl,appId, scope, state, redirectUri, device);
            return send_url;
        }

        /// <summary>
        /// 获取Access Token
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
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="open_id"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static WXUserInfo GetUserInfo(string access_token, string open_id, string lang = "zh_CN")
        {
            var client = new RestClient("https://api.weixin.qq.com/");
            var request = new RestRequest("sns/userinfo", Method.GET);
            request.AddParameter("access_token", access_token);
            request.AddParameter("openid", open_id);
            request.AddParameter("lang", lang);

            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;
            return JsonConvert.DeserializeObject<WXUserInfo>(response.Content);
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
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

    /// <summary>
    /// 微信用户信息
    /// </summary>
    public class WXUserInfo
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户特权信息
        /// </summary>
        public object privilege { get; set; }
        /// <summary>
        /// 联合Id,只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        //public string unionid { get; set; }
    }
}
