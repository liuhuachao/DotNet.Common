using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Common;

namespace DotNet.Test
{
    [TestClass]
    public class XMLHelperTest
    {
        [TestMethod]
        public void TestXMLToDataSet()
        {
            var ds = DataTableHelper.XMLToDataSet("c:/BOM.xml");
        }

        [TestMethod]
        public void TestCreateXml()
        {
            XMLHelper.Create("c:/students.xml","school","class","student","id","001");
        }
        
    }
}
