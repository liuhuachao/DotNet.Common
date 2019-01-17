using System;
using System.Text;

namespace DotNet.Common
{
    public class Md5Helper
    {
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="password">需要加密的密码</param>
        /// <param name="codeLength">加密位数</param>
        /// <returns></returns>
        public static string MD5(string password, int codeLength = 32)
        {
            string md5Password = string.Empty;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            if (codeLength == 32)
            {               
                md5Password = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", null);
            }
            if (codeLength == 16)
            {
                md5Password = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", null).Substring(8, 16);
            }
            return md5Password;
        }
    }
}
