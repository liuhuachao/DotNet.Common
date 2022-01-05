using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static LogHelper logHelper = new LogHelper("FileHelper");

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
                absolutePath = Path.Combine(HttpRuntime.AppDomainAppPath.ToString(), path);
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

        /// <summary>
        /// 替换文本文件中的字符串
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

        public static bool RunProcess(string processPath, string inputStr, out string outPutStr, int millisecond = 3000)
        {
            bool result = false;
            outPutStr = string.Empty;

            if (!string.IsNullOrEmpty(processPath) && !string.IsNullOrEmpty(inputStr))
            {
                Process process = new Process(); //创建进程对象               
                process.StartInfo.FileName = processPath; //设定需要执行的命令
                process.StartInfo.UseShellExecute = false; //不使用系统外壳程序启动
                process.StartInfo.RedirectStandardInput = true; //不重定向输入
                process.StartInfo.RedirectStandardOutput = true; //重定向输出
                process.StartInfo.RedirectStandardError = true; //
                process.StartInfo.CreateNoWindow = true; //不创建窗口
                
                try
                {
                    if (process.Start()) //开始进程
                    {                        
                        process.StandardInput.WriteLine(inputStr + "&exit"); //向cmd窗口发送输入信息
                        process.StandardInput.AutoFlush = true;

                        var errorStr = process.StandardError.ReadToEnd();
                        if (!string.IsNullOrEmpty(errorStr))
                        {
                            outPutStr = errorStr;
                        }
                        else
                        {
                            //获取cmd窗口的输出信息
                            outPutStr = process.StandardOutput.ReadToEnd();

                            if (millisecond == 0)
                            {
                                process.WaitForExit(); //这里无限等待进程结束
                            }
                            else
                            {
                                process.WaitForExit(millisecond); //等待进程结束，等待时间为指定的毫秒
                            }

                            result = true;
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    FileHelper.logHelper.Info($"RunProcess方法执行异常，消息如下：{ex.Message + ex.StackTrace}");
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }

            FileHelper.logHelper.Info($"RunProcess方法执行参数：processPath：{processPath}，inputStr：{inputStr}");
            return result;
        }
    }
}
