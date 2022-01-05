using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.XWPF.UserModel;

namespace DotNet.Common
{
    /// <summary>
    /// Word 文档帮助类
    /// </summary>
    public class WordHelper
    {
        /// <summary>
        /// 通过替换模板占位符生成新文档
        /// </summary>
        /// <param name="modelFilePath">模板文件物理路径</param>
        /// <param name="outFilePath">输出文件物理路径</param>
        /// <param name="paramDic">参数字典</param>
        /// <param name="placeholder">占位符</param>
        public static void ExportByModelFile(string modelFilePath, string outFilePath, Dictionary<string, string> paramDic, string placeholder = "#")
        {
            using (FileStream docFile = new FileStream(modelFilePath, FileMode.Open, FileAccess.Read))
            {
                // 通过模板加载文档
                XWPFDocument doc = new XWPFDocument(docFile);

                // 遍历段落                  
                foreach (var para in doc.Paragraphs)
                {
                    ReplaceKey(para, paramDic, placeholder);
                }

                // 遍历表格      
                var tables = doc.Tables;
                foreach (var table in tables)
                {
                    LoopTable(table, paramDic, placeholder);
                }

                // 创建目录
                var path = System.IO.Path.GetDirectoryName(outFilePath);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                // 生成文件
                FileStream outStream = new FileStream(outFilePath, FileMode.CreateNew);
                doc.Write(outStream);
                outStream.Close();
            }
        }

        /// <summary>
        /// 遍历表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="param"></param>
        /// <param name="placeHolder">占位符</param>
        private static void LoopTable(XWPFTable table, Dictionary<string, string> param, string placeHolder)
        {
            foreach (var row in table.Rows)
            {
                foreach (var cell in row.GetTableCells())
                {
                    if (cell.Tables != null && cell.Tables.Any())
                    {
                        foreach (var tab in cell.Tables)
                        {
                            LoopTable(tab, param, placeHolder);
                        }
                    }
                    foreach (var para in cell.Paragraphs)
                    {
                        ReplaceKey(para, param, placeHolder);
                    }
                }
            }
        }

        /// <summary>
        /// 替换占位符
        /// </summary>
        /// <param name="paragraph">段落</param>
        /// <param name="paramDic">参数字典</param>
        /// <param name="placeHolder">占位符</param>
        private static void ReplaceKey(XWPFParagraph paragraph, Dictionary<string, string> paramDic, string placeHolder)
        {
            string text = paragraph.ParagraphText;

            if (null == text || text.Length <= 3) return;

            foreach (var entry in paramDic)
            {
                if (text.Contains(placeHolder + entry.Key + placeHolder))
                {
                    paragraph.ReplaceText(placeHolder + entry.Key + placeHolder, entry.Value ?? "");
                }
            }

            var checkedStr = "!";
            var unCheckedStr = Convert.ToChar(0x00A3).ToString();

            var runs = paragraph.Runs;
            for (int i = 0; i < runs.Count; i++)
            {
                var run = runs[i];
                var runText = run.ToString();

                // 特殊字符  ☑ 0x0052 未选中 0x00A3
                if (runText.Contains(checkedStr) || runText.Contains(unCheckedStr))
                {
                    run.ReplaceText(checkedStr, "R");
                    run.ReplaceText(unCheckedStr, "");
                    run.FontFamily = "Wingdings 2";
                }
            }

        }
    }

}
