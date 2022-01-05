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
    /// 下载帮助类
    /// </summary>
    public class DownloadHelper
    {
        /// <summary>
        /// 下载文件到浏览器
        /// 分块方式
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        /// <param name="chunkSize">分块大小</param>
        public static void DownloadFileToBrowserByChunk(string filePath, long chunkSize = 102400)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            var fileName = Path.GetFileName(filePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("系统消息", "下载过程中出现了错误，文件路径不存在…");
            }

            byte[] buffer = new byte[chunkSize];

            var response = HttpContext.Current.Response;
            response.Clear();                

            using (FileStream iStream = File.OpenRead(filePath))
            {
                long dataLengthToRead = iStream.Length;
                response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));

                while (dataLengthToRead > 0 && response.IsClientConnected)
                {
                    int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                    response.OutputStream.Write(buffer, 0, lengthRead);
                    response.Flush();
                    dataLengthToRead = dataLengthToRead - lengthRead;
                }
                response.Close();
            }

        }

        /// <summary>
        /// 下载文件到浏览器
        /// 流方式
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        public static void DownloadFileToBrowserByBinaryWrite(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            var fileName = Path.GetFileName(filePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("系统消息", "下载过程中出现了错误，文件路径不存在…");
            }

            // 将文件读入缓冲区
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();

            // 将缓冲区写入 HTTP 输出流
            var response = HttpContext.Current.Response;
            response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
            response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            response.BinaryWrite(bytes);

            //向客户端发送当前所有缓冲的输出
            response.Flush();

            response.End();
        }

        /// <summary>
        /// 下载文件到浏览器
        /// WriteFile 方式
        /// 将指定的文件直接写入 HTTP 响应输出流。
        /// 将此方法用于大型文件时，调用方法可能会引发异常。
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        public static void DownloadFileToBrowserByWriteFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            var fileName = Path.GetFileName(filePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("系统消息", "下载过程中出现了错误，文件路径不存在…");
            }

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();

            response.ClearHeaders();
            response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            response.AddHeader("Content-Length", fileInfo.Length.ToString());
            response.AddHeader("Content-Transfer-Encoding", "binary");

            response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            response.WriteFile(fileInfo.FullName);
            response.Flush();
            response.End();
        }

        /// <summary>
        /// 下载文件到浏览器
        /// TransmitFile 方式
        /// 将指定的文件直接写入 HTTP 响应输出流，而不在内存中缓冲该文件
        /// 将此方法用于大型文件时，调用方法可能会引发异常。
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        public static void DownloadFileToBrowserByTransmitFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            var fileName = Path.GetFileName(filePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("系统消息", "下载过程中出现了错误，文件路径不存在…");
            }

            var response = HttpContext.Current.Response;
            response.ContentType = "application/x-zip-compressed";
            response.AddHeader("Content-Disposition", $"attachment;filename={ HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) }");
            response.TransmitFile(filePath);
        }

    }
}
