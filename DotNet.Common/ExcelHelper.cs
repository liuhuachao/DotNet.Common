using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

namespace DotNet.Common
{
    /// <summary>
    /// Excel导入导出帮助类 
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// DataSet 导出 Excel文件
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="file">导出路径(包括文件名与扩展名)</param>
        public static void DataSetToExcel(DataSet ds, string file)
        {
            //工作簿
            IWorkbook book = CreateIWorkbookByFile(file);

            //工作表
            for (int i = 0; i < ds?.Tables?.Count; i++)
            {
                var dt = ds.Tables[i];
                var sheetName = $"Sheet{i + 1}";
                CreateSheetFromDataTable(book, dt, sheetName);
            }

            //导出文件
            ExportFileFromBook(book,file);
        }

        /// <summary>
        /// DataTable 导出 Excel文件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="file">导出路径(包括文件名与扩展名)</param>
        public static void DataTableToExcel(DataTable dt, string file)
        {
            IWorkbook book = CreateIWorkbookByFile(file);

            CreateSheetFromDataTable(book, dt);

            ExportFileFromBook(book, file);
        }

        /// <summary>
        /// Excel 导入 DataTable
        /// </summary>
        /// <param name="file">导入路径(包括文件名与扩展名)</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string file)
        {
            return ExcelToDataSet(file)?.Tables?[0];
        }

        /// <summary>
        /// Excel 导入 DataSet
        /// </summary>
        /// <param name="file">导入路径(包括文件名与扩展名)</param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string file)
        {
            if (string.IsNullOrEmpty(file)) return null;

            DataSet ds = new DataSet();

            string fileExt = Path.GetExtension(file).ToLower();

            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                //工作簿
                IWorkbook book = null;
                if (fileExt == ".xls")
                {
                    book = new HSSFWorkbook(fs);
                }
                else if (fileExt == ".xlsx")
                {
                    book = new XSSFWorkbook(fs);
                }
                else
                {
                    book = null;
                }
                if (book == null) return null;

                var count = book.NumberOfSheets;
                for (int a = 0; a < count; a++)
                {
                    //工作表
                    ISheet sheet = book.GetSheetAt(a);
                    DataTable dt = new DataTable(sheet.SheetName);

                    //表头  
                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueType(header.GetCell(i));
                        if (obj == null || obj.ToString() == string.Empty)
                        {
                            dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                        }
                        else
                        {
                            dt.Columns.Add(new DataColumn(obj.ToString()));
                        }
                        columns.Add(i);
                    }

                    //数据
                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        foreach (int j in columns)
                        {
                            dr[j] = GetValueType(sheet.GetRow(i).GetCell(j));
                            if (dr[j] != null && dr[j].ToString() != string.Empty)
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }

                    ds.Tables.Add(dt);
                }                
            }                   
            return ds;
        }

        /// <summary>
        /// 向单元格中插入图片
        /// </summary>
        /// <param name="book">工作簿</param>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行序号</param>
        /// <param name="col">列序号</param>
        /// <param name="imgUrl">图片地址</param>
        public static void AddPicture(HSSFWorkbook book, HSSFSheet sheet, int row, int col, string imgUrl)
        {
            try
            {
                //将图片数据添加至工作簿
                byte[] bytes = System.IO.File.ReadAllBytes(@imgUrl);
                int pictureIdx = book.AddPicture(bytes, PictureType.JPEG);

                //创建一个“绘画器”,这个绘画器用于所有的图片写入,获取存在的Sheet，必须在AddPicture之后 
                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

                //显示图片,设置图片位置
                HSSFClientAnchor anchor = new HSSFClientAnchor(50, 10, 0, 0, col, row, col + 1, row + 1);
                HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                pict.Resize(); //还原原始大小
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据文件路径创建 IWorkbook
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static IWorkbook CreateIWorkbookByFile(string file)
        {
            //校验入参
            if (string.IsNullOrEmpty(file)) return null;

            //工作簿
            IWorkbook book = null;
            string fileExt = Path.GetExtension(file).ToLower();
            if (fileExt == ".xls")
            {
                book = new HSSFWorkbook();
            }
            else if (fileExt == ".xlsx")
            {
                book = new XSSFWorkbook();
            }
            else
            {
                book = null;
            }

            return book;
        }

        /// <summary>
        /// 从 DataTable 创建 ISheet
        /// </summary>
        /// <param name="book"></param>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        private static void CreateSheetFromDataTable(IWorkbook book, DataTable dt, string sheetName = "Sheet1")
        {
            //校验入参
            if (book == null || dt == null) return;

            //工作表
            sheetName = string.IsNullOrEmpty(dt.TableName) ? sheetName : dt.TableName;
            ISheet sheet = book.CreateSheet(sheetName);

            //表头            
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);

                //单元格样式
                var headCellStyle = GetICellStyle(book, "宋体", 14, 600);
                cell.CellStyle = headCellStyle;

                //单元格链接
                //var link = GetIHyperlink(book,HyperlinkType.Url, "http://baidu.com");
                //cell.Hyperlink = link;
            }

            //数据            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());

                    //单元格样式
                    var dataCellStyle = GetICellStyle(book, "宋体", 12, 0);
                    cell.CellStyle = dataCellStyle;

                    //单元格链接
                    //var link = GetIHyperlink(book, HyperlinkType.Document, $"'{sheetName}'!A1");
                    //cell.Hyperlink = link;
                }
            }
        }

        /// <summary>
        /// 从 IWorkbook 导出文件
        /// </summary>
        /// <param name="book"></param>
        /// <param name="file"></param>
        private static void ExportFileFromBook(IWorkbook book, string file)
        {
            // 校验入参
            if (book == null || string.IsNullOrEmpty(file)) return;

            // 转为字节数组
            MemoryStream stream = new MemoryStream();
            book.Write(stream);
            var buf = stream.ToArray();

            // 创建目录
            var path = Path.GetDirectoryName(file);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // 保存为Excel文件  
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }

            // 输出到浏览器
            //System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            //curContext.Response.Clear();
            //curContext.Response.ContentType = "application/x-excel";
            //string filename = HttpUtility.UrlEncode(Path.GetFileName(file) + DateTime.Now.ToString("_yyyyMMdd_HHmm") + $".{fileExt}");
            //curContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            //book.Write(curContext.Response.OutputStream);
            //curContext.Response.End();
        }

        /// <summary>  
        /// 获取单元格类型
        /// </summary>  
        /// <param name="cell">单元格</param>  
        /// <returns>单元格类型</returns>  
        private static object GetValueType(ICell cell)
        {
            if (cell == null) return null;

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                case CellType.Formula:
                default:
                    return "=" + cell.CellFormula;
            }
        }

        /// <summary>
        /// 获取单元格的样式
        /// </summary>
        /// <param name="book">工作簿</param>
        /// <param name="fontname">字体名称</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="boldweight">字体粗细</param>
        /// <param name="color">字体颜色</param>
        /// <param name="underLineType">下划线类型</param>
        /// <param name="borderLeft">左边框</param>
        /// <param name="borderRight">右边框</param>
        /// <param name="borderTop">上边框</param>
        /// <param name="borderBottom">下边框</param>
        /// <returns></returns>
        private static ICellStyle GetICellStyle(IWorkbook book, string fontname, int fontsize, int boldweight, short color = 0, FontUnderlineType underLineType = 0, BorderStyle borderLeft = BorderStyle.Thin, BorderStyle borderRight = BorderStyle.Thin, BorderStyle borderTop = BorderStyle.Thin, BorderStyle borderBottom = BorderStyle.Thin)
        {
            ICellStyle cellstyle = book.CreateCellStyle();

            // 字体
            IFont ifont = book.CreateFont();
            ifont.FontName = fontname;                              // 字体样式：宋体/黑体
            ifont.IsBold = true;                                    // 字体粗细
            ifont.FontHeightInPoints = (short)fontsize;             // 字体大小
            if (color != 0) ifont.Color = color;                    // 颜色
            ifont.Underline = underLineType;                        // 下划线
            cellstyle.SetFont(ifont);

            // 对齐方式
            cellstyle.Alignment = HorizontalAlignment.Center;       //水平居中
            cellstyle.VerticalAlignment = VerticalAlignment.Center; //垂直居中

            // 边框
            cellstyle.BorderLeft = borderLeft;
            cellstyle.BorderRight = borderRight;
            cellstyle.BorderTop = borderTop;
            cellstyle.BorderBottom = borderBottom;

            return cellstyle;
        }

        /// <summary>
        /// 获取单元格的超链接
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="link">超链接</param>
        private static IHyperlink GetIHyperlink(IWorkbook book,HyperlinkType type, string address)
        {
            IHyperlink link = null;

            if (book is HSSFWorkbook)
            {
                link = new HSSFHyperlink(type)
                {
                    Address = address,
                };
            }
            else if (book is XSSFWorkbook)
            {
                link = new XSSFHyperlink(type)
                {
                    Address = address,
                };
            }
            else
            {
                link = null;
            }

            return link;

            /* Example:
             * 
            //创建URL链接
            var link = new HSSFHyperlink(HyperlinkType.Url)
            {
                Address = ("http://www.cnblogs.com/Murray")
            };

            //创建Email链接
            var link = new HSSFHyperlink(HyperlinkType.Email)
            {
                Address = ("mailto:12345678@qq.com?subject=这是Email链接")
            };

            //链接到工作表Sheet2
            var link = new HSSFHyperlink(HyperlinkType.Document)
            {
                Address = ("'Sheet2'!A1")
            };

            //链接到文件（同文件夹内）
            var link = new HSSFHyperlink(HyperlinkType.File)
            {
                Address = ("文件名")
            };

            cell.Hyperlink = link;

            */
        }
    }
}
