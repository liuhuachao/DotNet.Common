using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace DotNet.Test
{
    [TestClass]
    public class RestSharpTests
    {
        [TestMethod]
        public async Task TestHttpGet()
        {
            string expectedStr = "200";
            string actualStr = "";

            var client = new RestClient("http://api.chsgw.com/");
            var request = new RestRequest("v1/Homes/GetDetail", Method.GET);

            request.AddParameter("id", 300);
            request.AddParameter("showType", 3);

            IRestResponse<ResultMsg> response = await client.ExecuteTaskAsync<ResultMsg>(request);
            actualStr = response.Data.Code.ToString();

            Assert.AreEqual(expectedStr, actualStr);
        }

        public class ResultMsg
        {
            /// <summary>
            /// 状态码
            /// </summary>
            public int Code { get; set; }

            /// <summary>
            /// 返回消息
            /// </summary>
            public string Msg { get; set; }

            /// <summary>
            /// 返回数据
            /// </summary>
            public object Data { get; set; }
        }
    }
}
