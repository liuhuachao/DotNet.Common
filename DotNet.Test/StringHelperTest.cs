using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common.Test
{
    [TestClass()]
    public class StringHelperTest
    {
        [TestMethod()]
        public void TestGetFirstPinyin()
        {
            var returnStr = StringHelper.GetFirstPinyin("客专07004");
            returnStr = returnStr.Split('-')[0];
            var srcNumber = System.Text.RegularExpressions.Regex.Replace(returnStr,@"[^0-9]+","");
            var desNumber = srcNumber;
            if (!string.IsNullOrEmpty(srcNumber) && srcNumber.Length > 2)
            {
                desNumber = "(" + srcNumber.Insert(2,")");
            }
            returnStr = returnStr.Replace(srcNumber,desNumber);            
        }
    }
}