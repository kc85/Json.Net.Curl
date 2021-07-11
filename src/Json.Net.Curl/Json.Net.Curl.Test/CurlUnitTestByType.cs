using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Json.Net.CurlTest
{
    public class CurlUnitTestByType
    {

        [SetUp]
        public void Setup()
        {

        }

        #region get 
        [Test]
        public async Task GetJsonByType()
        {
            var json = await Json.Net.Curl.GetAsync<Apple>(@"data\JObjectUnitTest1.json");
            Assert.IsNotNull(json);
        }


        public async Task GetJsonUsingAuth()
        {
            //Get Json from Web using authorization
            var json = await Json.Net.Curl.GetAsync<Apple>(@"http://myserver.com/data.json", new Dictionary<string, string>() {["Authorization"] = "Basic dXNlcjpic2Ux" });
            Assert.IsNotNull(json);
        }

        #endregion

    }


    public class Apple
    {
        public string fruit { get; set; }
        public string size { get; set; }
        public string color { get; set; }
    }

}