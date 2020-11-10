using DotNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet.Common.Test
{
    [TestClass()]
    public class NPOIHelperTest
    {
        [TestMethod()]
        public void TestDataSetToExcel()
        {
            DataSet ds = new DataSet();

            DataTable dt1 = new DataTable("人员");
            dt1.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn("Id"),
                    new DataColumn("Name"),
                });
            dt1.Rows.Add(1,"张三");
            dt1.Rows.Add(2, "李四");
            dt1.Rows.Add(3, "王五");

            DataTable dt2 = new DataTable("成绩");
            dt2.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn("Id"),
                    new DataColumn("Score"),
                });
            dt2.Rows.Add(1, "80");
            dt2.Rows.Add(2, "70");
            dt2.Rows.Add(3, "90");

            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);

            NPOIHelper.DataSetToExcel(ds,"c:/test.xls");            
        }

        [TestMethod()]
        public void TestDataTableToExcel()
        {
            DataTable dt1 = new DataTable("人员");
            dt1.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn("Id"),
                    new DataColumn("Name"),
                });
            dt1.Rows.Add(1, "张三");
            dt1.Rows.Add(2, "李四");
            dt1.Rows.Add(3, "王五");

            NPOIHelper.DataTableToExcel(dt1, "c:/test.xlsx");
        }


        [TestMethod()]
        public void TestExcelToDataSet()
        {
            var ds = NPOIHelper.ExcelToDataSet("c:/test.xls");
        }

        
    }
}