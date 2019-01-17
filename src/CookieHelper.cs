using System;
using System.Collections.Generic;
using System.Web;

namespace DotNet.Common
{
    /// <summary>
    /// Cookie 帮助类
    /// 获取、设置和清除Cookie
    /// </summary>
    public class CookieHelper
    {
        #region 不带子健
        /// <summary>
        /// 根据Cookie名获取指定Cookie值
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <returns>返回字符串值</returns>
        public static string Get(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        public static void Add(string cookieName, string cookieValue)
        {
            HttpCookie cookie = new HttpCookie(cookieName)
            {
                Value = cookieValue
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 添加一个Cookie,带过期时间
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="ts">过期时间间隔</param>
        public static void Add(string cookieName, string cookieValue, TimeSpan ts)
        {
            HttpCookie cookie = new HttpCookie(cookieName)
            {
                Value = cookieValue,
                Expires = DateTime.Now.Add(ts)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 添加一个Cookie，带过期时间、域名和路径
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="ts"></param>
        /// <param name="domain"></param>
        /// <param name="path"></param>
        public static void Add(string cookieName, string cookieValue, TimeSpan ts, string domain, string path)
        {
            HttpCookie cookie = new HttpCookie(cookieName)
            {
                Value = cookieValue,
                Expires = DateTime.Now.Add(ts),
                Domain = domain,
                Path = path
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 移除指定Cookie
        /// </summary>
        /// <param name="cookieName">cookiename</param>
        public static void Remove(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 移除指定的Cookie,带域名
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="domain"></param>
        public static void RemoveByDomain(string cookieName, string domain)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Domain = domain;
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region 带子健
        /// <summary>
        /// 获取指定Cookie里的指定键的值
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <param name="cookieKey">cookie键</param>
        /// <returns>返回字符串值</returns>
        public static string Get(string cookieName, string cookieKey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Values[cookieKey];
            }
            return str;
        }

        /// <summary>
        /// 获取Cookie键值对
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetValues(string cookieName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                for (int i = 0; i < cookie.Values.Count; i++)
                {
                    dic.Add(cookie.Values.AllKeys[i], cookie.Values[i]);
                }
            }
            return dic;
        }

        /// <summary>
        /// 添加一个带子健的Cookie
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <param name="dicCookies">键值对字典</param>
        public static void Add(string cookieName, Dictionary<string, string> dicValues)
        {
            HttpCookie httpCookie = new HttpCookie(cookieName);
            foreach (var cookie in dicValues)
            {
                httpCookie.Values[cookie.Key] = cookie.Value;
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        /// <summary>
        /// 添加一个带子健的Cookie,带过期时间
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="dicCookies"></param>
        /// <param name="ts"></param>
        public static void Add(string cookieName, Dictionary<string, string> dicValues, TimeSpan ts)
        {
            HttpCookie httpCookie = new HttpCookie(cookieName)
            {
                Expires = DateTime.Now.Add(ts)
            };
            foreach (var cookie in dicValues)
            {
                httpCookie.Values[cookie.Key] = cookie.Value;
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }
        public static void Add(string cookieName, Dictionary<string, string> dicValues, string domain, string path)
        {
            HttpCookie httpCookie = new HttpCookie(cookieName)
            {
                Domain = domain,
                Path = path
            };
            foreach (var cookie in dicValues)
            {
                httpCookie.Values[cookie.Key] = cookie.Value;
            }
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        /// <summary>
        /// 移除Cookie中指定的键，若是最后一个键则移除整个Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="keyName"></param>
        public static void RemoveByNameAndKey(string cookieName, string keyName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                if (cookie.Values.Count > 0)
                {
                    cookie.Values.Remove(keyName);
                    if (cookie.Values.Count == 1)
                    {
                        cookie.Expires = DateTime.Now.AddDays(-1);
                    }
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
        public static void RemoveByNameAndKey(string cookieName, string keyName, string domain)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                if (cookie.Values.Count > 0)
                {
                    cookie.Domain = domain;
                    cookie.Values.Remove(keyName);
                    if (cookie.Values.Count == 1)
                    {
                        cookie.Expires = DateTime.Now.AddDays(-1);
                    }
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
        #endregion
    }
}
