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
    public class FileHelperTest
    {
        [TestMethod()]
        public void TestGetAbsolutePath()
        {
            var srcPath = "FilePath\\Test";
            var actualPath = FileHelper.GetAbsolutePath(srcPath);
            var expectedPath = @"F:\personal\DotNet.Common\DotNet.Test\bin\Debug\FilePath\Test";
            Assert.AreEqual(expectedPath, actualPath);
        }

        [TestMethod()]
        public void TestReplaceFileString()
        {
            var srcFile = @"F:\personal\DotNet.Common\DotNet.Test\bin\Debug\FilePath\Source\template.tlk";
            var targetFile = @"F:\personal\DotNet.Common\DotNet.Test\bin\Debug\FilePath\Target\1.tlk";

            FileHelper.ReplaceFileString(srcFile, targetFile, "HelloWorld", out string msg);
        }

        [TestMethod()]
        public void TestRunProcess()
        {
            var sendMsg = string.Empty;
            var result = FileHelper.RunProcess($"cmd.exe", $"adb push c:/test/ /mnt/sdcard/msg/1.tlk", out sendMsg);
        }
    }
}