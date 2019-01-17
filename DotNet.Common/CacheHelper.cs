using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace DotNet.Common
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>object对象</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return HttpContext.Current.Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 插入缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        public static void Insert(string key, object obj)
        {
            HttpContext.Current.Cache.Insert(key, obj);
        }

        /// <summary>
        /// 插入缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="fileName">依赖项文件名</param>
        public static void Insert(string key, object obj,string fileName)
        {
            CacheDependency dep = new CacheDependency(fileName);  //创建缓存依赖项
            HttpContext.Current.Cache.Insert(key, obj, dep);
        }

        /// <summary>
        /// 插入缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="slidingExpiration">相对过期时间</param>
        public static void Insert(string key, object obj,string fileName, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            CacheDependency dep = new CacheDependency(fileName);  //创建缓存依赖项
            HttpContext.Current.Cache.Insert(key, obj, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        public static void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
    }
}
