using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DotNet.Common
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 获得绝对路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>绝对路径</returns>
        public static string GetAbsolutePath(string path)
        {
            String absolutePath = "";
            if (path.ToLower().StartsWith("http://"))
            {
                return path;
            }
            if (HttpContext.Current != null)
            {
                absolutePath = HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                path = path.Replace("/", "\\");
                if (path.StartsWith("\\"))
                {
                    path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
                }
                absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return absolutePath;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void WriteFile(string Path, string content)
        {
            if (!File.Exists(Path))
            {
                FileStream fs = File.Create(Path);
                fs.Close();
                fs.Dispose();
            }
            StreamWriter sw = new StreamWriter(Path, true, Encoding.UTF8);
            sw.WriteLine(content);
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string result = "";
            if (!System.IO.File.Exists(Path))
            {
                result = "不存在相应的目录";
            }                
            else
            {
                StreamReader sr = new StreamReader(Path, Encoding.GetEncoding("gb2312"));
                result = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 追加内容
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="content">文件内容</param>
        public static void AppendText(string Path, string content)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}
