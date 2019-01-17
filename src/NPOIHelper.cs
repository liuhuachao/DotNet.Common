using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DotNet.Common
{
    /// <summary>
    /// NPOIHelper,Excel导入导出帮助类 
    /// </summary>
    public class NPOIHelper
    {
        #region Excel2003

        /// <summary>  
        /// 将Excel文件中的数据读出到DataTable中(xls)  
        /// </summary>  
        /// <param name="file">文件路径</param>  
        /// <returns>DataTable</returns>  
        public static DataTable ExcelToTableForXLS(string file)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                ISheet sheet = hssfworkbook.GetSheetAt(0);

                //表头  
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    }
                    else
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    columns.Add(i);
                }
                //数据  
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
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
            }
            return dt;
        }

        /// <summary>  
        /// 将Excel文件中的数据读出到DataTable中(xls)  
        /// </summary>  
        /// <param name="file">文件路径</param>  
        /// <returns>DataTable</returns>  
        public static DataTable ExcelToTableForXLS(Stream fs)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
            ISheet sheet = hssfworkbook.GetSheetAt(0);

            //表头  
            IRow header = sheet.GetRow(sheet.FirstRowNum);
            List<int> columns = new List<int>();
            for (int i = 0; i < header.LastCellNum; i++)
            {
                object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                if (obj == null || obj.ToString() == string.Empty)
                {
                    dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                }
                else
                    dt.Columns.Add(new DataColumn(obj.ToString()));
                columns.Add(i);
            }
            //数据  
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                DataRow dr = dt.NewRow();
                bool hasValue = false;
                foreach (int j in columns)
                {
                    dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
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
            return dt;
        }

        /// <summary>  
        /// 将DataTable数据导出到Excel文件中(xls)  
        /// </summary>  
        /// <param name="dt">数据源</param>  
        /// <param name="excelName">excel名称</param>  
        public static void TableToExcelForXLS(DataTable dt, string excelName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("excelName");

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.ContentType = "application/x-excel";
            string filename = HttpUtility.UrlEncode(excelName + DateTime.Now.ToString("_yyyyMMdd_HHmm") + ".xls");
            curContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            hssfworkbook.Write(curContext.Response.OutputStream);
            curContext.Response.End();
        }

        /// <summary>
        /// 将DataSet数据导出到Excel文件中(xls) 
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="excelName">Excel文件名</param>
        public static void DataSetToExcelForXLS(DataSet ds, string excelName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            HSSFCellStyle cellStyleHead = (HSSFCellStyle)GetICellStyle(hssfworkbook, "宋体", 12, 600);
            HSSFCellStyle cellStyleData = (HSSFCellStyle)GetICellStyle(hssfworkbook, "宋体", 11, 0);

            //工作表
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                ISheet sheet = hssfworkbook.CreateSheet(dt.TableName);

                //表头
                IRow rowHead = sheet.CreateRow(0);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = rowHead.CreateCell(j);
                    cell.SetCellValue(dt.Columns[j].ColumnName);
                    cell.CellStyle = cellStyleHead;
                }

                //数据
                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    IRow rowData = sheet.CreateRow(m + 1);
                    for (int n = 0; n < dt.Columns.Count; n++)
                    {
                        ICell cell = rowData.CreateCell(n);
                        cell.SetCellValue(dt.Rows[m][n].ToString());
                        cell.CellStyle = cellStyleData;
                    }
                }
            }
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.ContentType = "application/x-excel";
            string filename = HttpUtility.UrlEncode(excelName + DateTime.Now.ToString("_yyyyMMdd_HHmm") + ".xls");
            curContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            hssfworkbook.Write(curContext.Response.OutputStream);
            curContext.Response.End();
        }

        /// <summary>
        /// 向单元格中插入图片
        /// </summary>
        /// <param name="workbook">工作簿</param>
        /// <param name="sheet">工作表</param>
        /// <param name="row">行序号</param>
        /// <param name="col">列序号</param>
        /// <param name="imgUrl">图片地址</param>
        private static void AddPicture(HSSFWorkbook workbook, HSSFSheet sheet, int row, int col, string imgUrl)
        {
            try
            {
                //将图片数据添加至工作簿
                byte[] bytes = System.IO.File.ReadAllBytes(@imgUrl);
                int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);

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
        /// 获取单元格类型(xls)  
        /// </summary>  
        /// <param name="cell">单元格</param>  
        /// <returns>单元格类型</returns>  
        private static object GetValueTypeForXLS(HSSFCell cell)
        {
            if (cell == null)
                return null;
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
        /// 获取单元格样式
        /// </summary>
        /// <param name="book">工作表</param>
        /// <param name="font">字体</param>
        /// <param name="ha">水平对齐</param>
        /// <param name="va">垂直对齐</param>
        /// <returns></returns>
        private static ICellStyle GetICellStyle(HSSFWorkbook book, string fontname, int fontsize, int boldweight)
        {
            HSSFCellStyle cellstyle = (HSSFCellStyle)book.CreateCellStyle();

            IFont ifont = book.CreateFont();
            ifont.FontName = fontname;
            ifont.Boldweight = (short)boldweight;
            ifont.FontHeightInPoints = (short)fontsize;
            cellstyle.SetFont(ifont);
            cellstyle.Alignment = HorizontalAlignment.Center;
            cellstyle.VerticalAlignment = VerticalAlignment.Center;

            //单元格边框
            cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            return cellstyle;
        }

        #endregion

        #region Excel2007

        /// <summary>  
        /// 将Excel文件中的数据读出到DataTable中(xlsx)  
        /// </summary>  
        /// <param name="fs">文件流</param>  
        /// <returns>DataTable</returns>  
        public static DataTable ExcelToTableForXLSX(Stream fs)
        {
            DataTable dt = new DataTable();
            XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
            ISheet sheet = xssfworkbook.GetSheetAt(0);

            // 表头  
            IRow header = sheet.GetRow(sheet.FirstRowNum);
            List<int> columns = new List<int>();
            for (int i = 0; i < header.LastCellNum; i++)
            {
                object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
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

            // 数据  
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                DataRow dr = dt.NewRow();
                bool hasValue = false;
                foreach (int j in columns)
                {
                    dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
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
            return dt;
        }

        /// <summary>  
        /// 将Excel文件中的数据读出到DataTable中(xlsx)  
        /// </summary>  
        /// <param name="file">文件路径</param>  
        /// <returns>DataTable</returns>  
        public static DataTable ExcelToTableForXLSX(string file)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                ISheet sheet = xssfworkbook.GetSheetAt(0);

                // 表头  
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
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

                // 数据  
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
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
            }
            return dt;
        }

        /// <summary>  
        /// 将DataTable数据导出到Excel文件中(xlsx)  
        /// </summary>  
        /// <param name="dt">数据源</param>  
        /// <param name="excelName">文件名称</param>  
        public static void TableToExcelForXLSX(DataTable dt, string excelName)
        {
            XSSFWorkbook xssfworkbook = new XSSFWorkbook();
            ISheet sheet = xssfworkbook.CreateSheet("excelName");

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.Clear();

            curContext.Response.ContentType = "application/x-excel";
            string filename = HttpUtility.UrlEncode(excelName + DateTime.Now.ToString("_yyyyMMdd_HHmm") + ".xlsx");
            curContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            xssfworkbook.Write(curContext.Response.OutputStream);
            curContext.Response.End();
        }

        /// <summary>  
        /// 获取单元格类型(xlsx)  
        /// </summary>  
        /// <param name="cell">单元格</param>  
        /// <returns>单元格类型</returns>  
        private static object GetValueTypeForXLSX(XSSFCell cell)
        {
            if (cell == null)
                return null;
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

        #endregion
    }
}
