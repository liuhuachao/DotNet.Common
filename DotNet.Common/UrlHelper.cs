using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DotNet.Common
{
    /// <summary>
    /// Url 处理类
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }

        /// <summary>
        /// 判断页面URL是否存在
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static bool IsExistUrl(string strUrl)
        {
            if (!strUrl.Contains("http"))
            {
                strUrl = "http://" + strUrl;
            }
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(strUrl);
                myRequest.Method = "HEAD";
                myRequest.Timeout = 5000;
                HttpWebResponse res = (HttpWebResponse)myRequest.GetResponse();
                return (res.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 执行Url
        /// </summary>
        /// <param name="urlPath"></param>
        /// <returns></returns>
        public static string UrlExecute(string urlPath)
        {
            string str;
            if (string.IsNullOrEmpty(urlPath))
            {
                return "error";
            }
            StringWriter writer = new StringWriter();
            try
            {
                HttpContext.Current.Server.Execute(urlPath, writer);
                str = writer.ToString();
                return str;
            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                writer.Close();
                writer.Dispose();
            }            
        }
    }
}
