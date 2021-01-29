using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyPinyin.Core;

namespace DotNet.Common
{
    public class StringHelper
    {
        public static string SplitAndComb(string srcStr)
        {
            var a = srcStr.Split(' ')[1];
            var b = srcStr.Split(' ')[2];
            var c = string.Empty;
            var d = string.Empty;
            var e = "E";
            var f = string.Empty;
            var g = "G";

            var z = string.Join(" ", new string[] { a, b, c, d, e, f, g });
            return c;
        }

        /// <summary>
        /// 获取单个字符的拼音
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetPinyin(char c)
        {
            var pinyin = PinyinHelper.GetPinyin(c);
            return pinyin;
        }

        /// <summary>
        /// 获取文本字符串的拼音
        /// </summary>
        /// <param name="text">要获取拼音的文本</param>
        /// <returns></returns>
        public static string GetPinyin(string text)
        {
            var separator = ",";
            var pinyin = PinyinHelper.GetPinyin(text,separator);
            return pinyin;
        }

        /// <summary>
        /// 获取文本字符串的拼音首字母
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetFirstPinyin(string text)
        {
            var returnStr = string.Empty;
            var pinyinArray = PinyinHelper.GetPinyin(text, ",").Split(',');
            foreach (var item in pinyinArray)
            {
                returnStr += item.Substring(0,1);
            }
            return returnStr;
        }

        /// <summary>
        /// 判断单个字符是否是中文
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsChinese(char c)
        {
            var isChinese = PinyinHelper.IsChinese(c);
            return isChinese;
        }
    }
}
