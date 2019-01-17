using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace DotNet.Common
{
    /// <summary>
    /// 邮件操作类
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件(通过SMTP协议)
        /// </summary>
        /// <param name="mailSubjct">邮件主题</param>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="isHtml">正文格式</param>
        /// <param name="filename">附件名</param>
        /// <param name="mailFrom">发件地址</param>
        /// <param name="replyTo">回复地址</param>
        /// <param name="nickName">昵称</param>
        /// <param name="mailTo">收件地址</param>
        /// <param name="host">主机名称或IP地址</param>
        /// <param name="port">端口号</param>        
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ssl">加密类型</param>
        /// <returns></returns>
        public static bool SendMail(string mailSubjct, string mailBody, bool isHtml, string filename, string mailFrom, List<string> replyToList, string nickName, List<string> mailToList, string host, int port, string username, string password, bool ssl,out string msg)
        {
            msg = string.Empty;
            var result = false;
            try
            {
                // 创建邮件对象
                MailMessage mailMsg = new MailMessage
                {
                    Subject = mailSubjct,
                    Body = mailBody,
                    IsBodyHtml = isHtml,
                    From = new MailAddress(mailFrom, nickName),
                };

                // 添加收件地址
                foreach(var mailTo in mailToList)
                {
                    if(RegexHelper.IsMatch(mailTo,RegexHelper.EmailPattern))
                    {
                        mailMsg.To.Add(mailTo);
                    }                    
                }
                if (mailMsg.To.Count == 0)
                {
                    msg = "收件地址不正确！";
                    return false;
                }

                // 添加回复地址
                foreach(var replyTo in replyToList)
                {
                    if (RegexHelper.IsMatch(replyTo, RegexHelper.EmailPattern))
                    {
                        mailMsg.ReplyToList.Add(replyTo);
                    }
                }
                if (mailMsg.ReplyToList.Count == 0)
                {
                    msg = "回复地址不正确！";
                    return false;                    
                }

                // 添加邮件附件
                if (!string.IsNullOrEmpty(filename) && System.IO.File.Exists(filename))
                {
                    mailMsg.Attachments.Add(new Attachment(filename));
                }

                // 设置发件服务器
                SmtpClient smtpClient = new SmtpClient(host, port);
                NetworkCredential credential = new NetworkCredential(username, password);
                smtpClient.Credentials = credential;
                smtpClient.EnableSsl = ssl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMsg);

                result = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                Console.WriteLine(msg);
            }
            return result;
        }
    }
}
