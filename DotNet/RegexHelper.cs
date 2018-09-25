using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNet.Common
{
    public class RegexHelper
    {
        public static bool IsCheckIphoneFormat(string text)
        {
            return Regex.IsMatch(text, @"^[1]+[3-8]+\d{9}");
        }
        public static bool IsCheckPassWordFormat(string text)
        {
            return Regex.IsMatch(text, @"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,16}$");
        }
    }
}
