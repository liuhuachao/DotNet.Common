using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common
{
    /// <summary>
    /// 日期时间操作类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 将泛型数据转换为DateTime
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dt">泛型数据</param>
        /// <returns></returns>
        public static DateTime? ToDateTime<T>(T dt)
        {
            if (dt == null || Convert.IsDBNull(dt))
            {
                return null;
            }
            try
            {
                return Convert.ToDateTime(dt);
            }
            catch (Exception)
            {              
                return null;
                throw;
            }           
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            DateTime ts1 = DateTime.UtcNow;
            DateTime ts2 = DateTime.Parse("1970-1-1");
            var timestamp = ts1.Subtract(ts2).TotalSeconds.ToString();
            return timestamp;
        }

        /// <summary>
        /// 返回时间差(几月几日或几小时前或几分钟前)
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string GetTimeDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            {

            }
            return dateDiff;
        }
    }
}
