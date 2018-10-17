using DotNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet.Common.Tests
{
    [TestClass()]
    public class SendSmsHelperTests
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger("SendSmsHelperTests");

        [TestMethod()]
        public void SendSingleSMSTest()
        {
            string[] phoneNumbers = { "13824394952", "18171097305" };
            string validCode = RandomHelper.GetRandomString(4);

            var result1 = string.Empty;
            var result2 = string.Empty;

            result1 = SendSmsHelper.SendSingleSMS(phoneNumbers, validCode);
            result2 = SendSmsHelper.SendMultiSMS(phoneNumbers, validCode);

            _log.Info(string.Format("指定模板单发短信:手机号：{0}，验证码：{1}，返回信息：{2}", phoneNumbers[0], validCode, result1));
            _log.Info(string.Format("指定模板群发短信:手机号：{0}，验证码：{1}，返回信息：{2}", phoneNumbers[0], validCode, result1));
        }
    }
}