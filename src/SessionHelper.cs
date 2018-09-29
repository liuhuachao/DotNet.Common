using System.Web;

namespace DotNet.Common
{
    /// <summary>
    /// Session 帮助类
    /// 获取、设置、移除和清空Session
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="sessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static object Get(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[sessionName];
            }
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="sessionName">session 名</param>
        /// <param name="sessionValue">session 值</param>
        public static void Set(string sessionName, object sessionValue)
        {
            HttpContext.Current.Session.Remove(sessionName);
            HttpContext.Current.Session.Add(sessionName, sessionValue);
        }

        /// <summary>
        /// 设置 Session
        /// </summary>
        /// <param name="sessionName">session 名</param>
        /// <param name="sessionValue">session 值</param>
        /// <param name="expirationTime">过期时间（以分为单位）</param>
        public static void Set(string sessionName, object sessionValue, int expirationTime)
        {
            HttpContext.Current.Session.Remove(sessionName);
            HttpContext.Current.Session.Add(sessionName, sessionValue);
            HttpContext.Current.Session.Timeout = expirationTime;
        }

        /// <summary>
        /// 删除一个指定的session
        /// </summary>
        /// <param name="sessionName">Session名称</param>
        /// <returns></returns>
        public static void Remove(string sessionName)
        {
            HttpContext.Current.Session.Remove(sessionName);
        }

        /// <summary>
        /// 清空所有的Session
        /// </summary>
        /// <returns></returns>
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
