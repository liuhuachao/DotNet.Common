using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
