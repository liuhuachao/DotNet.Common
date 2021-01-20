using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

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

        /// <summary>
        /// 替换文件中的字符串
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="targetFile"></param>
        /// <param name="targetStr"></param>
        /// <param name="msg"></param>
        /// <param name="srcStr"></param>
        /// <returns></returns>
        public static bool ReplaceFileString(string srcFile, string targetFile, string targetStr, out string msg, string srcStr = "[PMJCode]")
        {
            var isSuccess = false;
            msg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(srcFile) || string.IsNullOrEmpty(targetFile) || string.IsNullOrEmpty(targetStr))
                {
                    msg = "替换文件字符串的方法参数不能为空！";
                }
                else
                {
                    if (!File.Exists(srcFile))
                    {
                        msg = "替换文件字符串的模板文件（tlk文件）不存在！";
                    }
                    else
                    {
                        var targetPath = Path.GetDirectoryName(targetFile);
                        if (!System.IO.Directory.Exists(targetPath))
                        {
                            System.IO.Directory.CreateDirectory(targetPath);
                        }

                        if (File.Exists(targetFile))
                        {
                            File.Delete(targetPath);
                        }

                        StreamReader reader = new StreamReader($@"{srcFile}", Encoding.Default);
                        String str = reader.ReadToEnd();
                        str = str.Replace($@"{srcStr}", $@"{targetStr}");
                        StreamWriter readTxt = new StreamWriter($@"{targetFile}", false, Encoding.Default);
                        readTxt.Write(str);
                        readTxt.Flush();
                        readTxt.Close();
                        reader.Close();

                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = $"替换文件字符串异常：{ex.Message + ex.StackTrace}";
                throw;
            }

            return isSuccess;
        }

        #region 文件压缩/解压缩

        /// <summary>
        /// 将文件夹压缩为文件
        /// </summary>
        /// <param name="zipFileName">zip 文件名</param>
        /// <param name="sourceDirectory">原目录</param>
        /// <param name="recurse">是否递归子目录</param>
        /// <param name="fileFilter">目录过滤参数（正则表达式）</param>
        public static void CreateZipByDictory(string zipFileName, string sourceDirectory, bool recurse = true, string fileFilter = "")
        {
            (new FastZip()).CreateZip($@"{zipFileName}", $@"{sourceDirectory}", recurse, fileFilter);
        }

        /// <summary>
        /// 创建zip文件，并添加文件
        /// </summary>
        /// <param name="zipFileName">zip 文件</param>
        /// <param name="fileNameList">要添加的文件名列表</param>
        public static void CreateZipAddFile(string zipFileName, List<string> fileNameList)
        {
            using (ZipFile zip = ZipFile.Create($@"{zipFileName}"))
            {
                zip.BeginUpdate();
                foreach (var fileName in fileNameList)
                {
                    zip.Add($@"{fileName}");
                }
                zip.CommitUpdate();
            }
        }

        /// <summary>
        /// 修改zip文件，并添加文件
        /// </summary>
        /// <param name="zipFileName">zip 文件</param>
        /// <param name="fileNameList">要添加的文件名列表</param>
        public static void UpdateZipAddFile(string zipFileName, List<string> fileNameList)
        {
            using (ZipFile zip = new ZipFile($@"{zipFileName}"))
            {
                zip.BeginUpdate();
                foreach (var fileName in fileNameList)
                {
                    zip.Add($@"{fileName}");
                }
                zip.CommitUpdate();
            }
        }

        /// <summary>
        /// 修改zip文件，并删除文件
        /// </summary>
        /// <param name="zipFileName">zip 文件</param>
        /// <param name="fileNameList">要添加的文件名列表</param>
        public static void UpdateZipDeleteFile(string zipFileName, List<string> fileNameList)
        {
            using (ZipFile zip = new ZipFile($@"{zipFileName}"))
            {
                zip.BeginUpdate();
                var fileList = GetFileListByZip($@"{zipFileName}");
                foreach (var fileName in fileNameList)
                {
                    if (fileList.Contains($@"{fileName}"))
                    {
                        zip.Delete($@"{fileName}");
                    }
                }
                zip.CommitUpdate();
            }
        }

        /// <summary>
        /// 获取 Zip 文件中的文件
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <returns></returns>
        public static List<string> GetFileListByZip(string zipFileName)
        {
            var fileList = new List<string>();
            using (ZipFile zip = new ZipFile($@"{zipFileName}"))
            {
                foreach (ZipEntry entry in zip)
                {
                    fileList.Add(entry.Name);
                }
            }
            return fileList;
        }

        /// <summary>
        /// 解压zip文件中文件到指定目录下
        /// </summary>
        /// <param name="zipFileName">zip 文件名</param>
        /// <param name="targetDirectory">目标目录</param>
        /// <param name="fileFilter">目录过滤参数（正则表达式）</param>
        public static void ExtractZip(string zipFileName, string targetDirectory, string fileFilter = "")
        {
            (new FastZip()).ExtractZip($@"{zipFileName}", $@"{targetDirectory}", fileFilter);
        }

        #endregion
    }
}
