using System;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace DotNet.Common
{
    /// <summary>
    /// DES 加密解密类
    /// </summary>
    public class EncryptHelper
    {
        private const string SECRETKEY = "liuhuachao";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, SECRETKEY);
        }
        /// <summary> 
        /// 加密
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            des.Key = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sKey))).Replace("-", null).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sKey))).Replace("-", null).Substring(0, 8));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            if (String.IsNullOrEmpty(Text))
            {
                return null; ;
            }
            else
            {
                return Decrypt(Text, SECRETKEY);
            }
        }
        /// <summary> 
        /// 解密
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            des.Key = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sKey))).Replace("-", null).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sKey))).Replace("-", null).Substring(0, 8));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}
