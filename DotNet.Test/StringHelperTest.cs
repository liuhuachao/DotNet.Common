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
        public void TestSplitAndComb()
        {
            StringHelper.SplitAndComb("短心轨 8125 客专07004-Ⅲ-8 右-G-H/Z");
        }
    }
}