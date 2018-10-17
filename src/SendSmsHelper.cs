using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

namespace DotNet.Common
{
    public class SendSmsHelper
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger("SendSmsHelper");

        #region 配置参数
        // 短信应用SDK AppID
        static readonly int AppId = int.Parse(ConfigurationManager.AppSettings["AppId"]);
        // 短信应用SDK AppKey
        static readonly string AppKey = ConfigurationManager.AppSettings["AppKey"];
        // 短信签名
        static readonly string SmsSign = ConfigurationManager.AppSettings["SmsSign"].ToString();
        // 短信模板ID，需要在短信应用中申请
        static readonly int TemplateId = int.Parse(ConfigurationManager.AppSettings["TemplateId"]);
        // 过期时间（分钟）
        static readonly string ExpirationTime = ConfigurationManager.AppSettings["ExpirationTime"];
        #endregion 

        /// <summary>
        /// 指定模板 ID 单发短信
        /// </summary>
        /// <param name="phoneNumbers">需要发送短信的手机号码</param>
        /// <param name="securityCode">验证码</param>
        /// <param name="expirationTime">过期时间（分钟）</param>
        /// <param name="templateId">短信模板ID，需要在短信应用中申请</param>
        /// /// <returns>返回信息</returns>
        public static string SendSingleSMS(String[] phoneNumbers, string securityCode, string expirationTime = "5", int templateId = 195250)
        {
            Exception ex = null;
            string result = string.Empty;
            try
            {
                expirationTime = (expirationTime == ExpirationTime) ? expirationTime : ExpirationTime;
                templateId = (templateId == TemplateId) ? templateId : TemplateId;

                SmsSingleSender ssender = new SmsSingleSender(AppId, AppKey);
                //result = ssender.sendWithParam("86", phoneNumbers[0], templateId, new[] { securityCode, expirationTime }, SmsSign, "", "").ToString();
                Console.WriteLine(result);
            }
            catch (JSONException e)
            {
                ex = e;
                Console.WriteLine(e);
            }
            catch (HTTPException e)
            {
                ex = e;
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                ex = e;
                Console.WriteLine(e);
            }
            _log.Info(string.Format("指定模板单发短信:手机号：{0}，验证码：{1}，返回信息：{2}", phoneNumbers[0], securityCode, result), ex);
            return result;
        }

        /// <summary>
        /// 指定模板 ID 群发短信
        /// </summary>
        /// <param name="phoneNumbers">需要发送短信的手机号码</param>
        /// <param name="securityCode">验证码</param>
        /// <param name="expirationTime">过期时间（分钟）</param>
        /// <param name="templateId">短信模板ID，需要在短信应用中申请</param>
        /// <returns>返回信息</returns>
        public static string SendMultiSMS(string[] phoneNumbers, string securityCode, string expirationTime = "5", int templateId = 195250)
        {
            Exception ex = null;
            string result = string.Empty;
            try
            {
                expirationTime = (expirationTime == ExpirationTime) ? expirationTime : ExpirationTime;
                templateId = (templateId == TemplateId) ? templateId : TemplateId;

                SmsMultiSender msender = new SmsMultiSender(AppId, AppKey);
                //result = msender.sendWithParam("86", phoneNumbers, templateId, new[] { securityCode, expirationTime }, SmsSign, "", "").ToString();
                Console.WriteLine(result);
            }
            catch (JSONException e)
            {
                ex = e;
                Console.WriteLine(e);
            }
            catch (HTTPException e)
            {
                ex = e;
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                ex = e;
                Console.WriteLine(e);
            }

            _log.Info(string.Format("指定模板单发短信:手机号：{0}，验证码：{1}，返回信息：{2}", phoneNumbers[0], securityCode, result), ex);
            return result;
        }
    }
}
