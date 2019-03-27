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
    }
}
