using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Common
{
    /// <summary>
    /// 操作正则表达式的公共类
    /// </summary>    
    public class RegexHelper
    {
        #region 模式字符串
        public static readonly string PhonePattern = @"^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199)\d{8}$";       //手机号码：3位号段，后面加8位数字
        public static readonly string TelPattern = @"^0(\d{2}-?\d{8}|\d{3}-?\d{7})$";                                                         //电话号码：以0开头,3到4位数字的区号，中间加-(或者不加)，后面接7到8位的数字
        public static readonly string EmailPattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";                                        //Email地址
        public static readonly string PasswordPattern = @"^(?=.*[0-9]+)(?=.*[a-zA-Z]+)\S{6,16}$";                                             //强密码：6-16位至少包含字母和数字
        public static readonly string OrganizationCodePattern = @"^[1-9A-GNY]{1}[123459]{1}[1-9]{2}[0-9]{4}[0-9A-Z]{10}$";                    //社会统一信用代码
        #endregion

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 Regex 构造函数中指定的正则表达式的第一个匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(string input, string pattern)
        {
            return Regex.Match(input, pattern, RegexOptions.IgnoreCase).Value;
        }
        public static string Match(string input, string pattern, RegexOptions options)
        {
            return Regex.Match(input,pattern,options).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索正则表达式的所有匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static MatchCollection Matchs(string input, string pattern)
        {
            return Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
        }
        public static MatchCollection Matchs(string input, string pattern, RegexOptions options)
        {
            return Regex.Matches(input, pattern, options);
        }

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与某个正则表达式模式匹配的所有字符串。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace(string input, string pattern,string replacement)
        {
            return Regex.Replace(input,pattern,replacement);
        }
        public static string Replace(string input, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        /// <summary>
        /// 在由 Regex 构造函数指定的正则表达式模式所定义的位置，拆分指定的输入字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string[] Split(string input, string pattern)
        {
            return Regex.Split(input, pattern, RegexOptions.IgnoreCase);
        }
        public static string[] Split(string input, string pattern, RegexOptions options)
        {
            return Regex.Split(input, pattern, options);
        }
    }
}
