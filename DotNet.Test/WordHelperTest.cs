using Xunit;
using DotNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common.Test
{
    public class WordHelperTest
    {
        [Fact()]
        public void TestExportByModelFile()
        {
            string modelFilePath = AppContext.BaseDirectory + "\\Files\\真空断路器出厂检验报告样例.docx";
            string outFilePath = AppContext.BaseDirectory + "\\Files\\真空断路器出厂检验报告样例-new.docx";

            Dictionary<string, string> paramDic = new Dictionary<string, string>();
            paramDic.Add("ProductModel", "NPV12T1231D22E");
            paramDic.Add("ProductCode", "PV181266");
            paramDic.Add("ReportCode", "2018-09-16Z-001");

            var checkedStr = "!";
            var unCheckedStr = Convert.ToChar(0x00A3).ToString(); // "£"
            paramDic.Add("p1.2A", unCheckedStr);
            paramDic.Add("p1.2B", checkedStr);
            paramDic.Add("p1.2C", unCheckedStr);
            paramDic.Add("p1.2D", unCheckedStr);

            WordHelper.ExportByModelFile(modelFilePath, outFilePath, paramDic);

            //Assert.True(false, "This test needs an implementation");
        }
    }
}