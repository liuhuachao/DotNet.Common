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
        public static void SendSingleSMS(String[] phoneNumbers, string securityCode, string expirationTime = "5", int templateId = 195250)
        {
            try
            {
                expirationTime = (expirationTime == ExpirationTime) ? expirationTime : ExpirationTime;
                templateId = (templateId == TemplateId) ? templateId : TemplateId;

                SmsSingleSender ssender = new SmsSingleSender(AppId, AppKey);
                var result = ssender.sendWithParam("86", phoneNumbers[0],
                    templateId, new[] { securityCode, expirationTime }, SmsSign, "", "");
                Console.WriteLine(result);
            }
            catch (JSONException e)
            {
                Console.WriteLine(e);
            }
            catch (HTTPException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 指定模板 ID 群发短信
        /// </summary>
        /// <param name="phoneNumbers">需要发送短信的手机号码</param>
        /// <param name="securityCode">验证码</param>
        /// <param name="expirationTime">过期时间（分钟）</param>
        /// <param name="templateId">短信模板ID，需要在短信应用中申请</param>
        public static void SendMultiSMS(string[] phoneNumbers, string securityCode, string expirationTime = "5", int templateId = 195250)
        {
            try
            {
                expirationTime = (expirationTime == ExpirationTime) ? expirationTime : ExpirationTime;
                templateId = (templateId == TemplateId) ? templateId : TemplateId;

                SmsMultiSender msender = new SmsMultiSender(AppId, AppKey);
                var sresult = msender.sendWithParam("86", phoneNumbers, templateId,
                    new[] { securityCode, expirationTime }, SmsSign, "", "");
                Console.WriteLine(sresult);
            }
            catch (JSONException e)
            {
                Console.WriteLine(e);
            }
            catch (HTTPException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
