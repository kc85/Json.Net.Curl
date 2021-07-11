using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Json.Net.CurlTest
{
    public class CurlTests
    {
        const string GET_JSON_URL = "https://mocki.io/v1/35587ebf-59a2-41f7-9b93-aedec64af78a";
        const string GET_JSON_ARRAY_URL = "https://mocki.io/v1/d4867d8b-b5d5-4a48-a4ab-79131b5809b8";

        [SetUp]
        public void Setup()
        {
          
        }

        #region get 
        [Test]
        public void GetJson()
        {
            var json = Json.Net.Curl.Get(@"data\JObjectUnitTest1.json");
            Assert.IsNotNull(json);
        }
        [Test]
        public void GetJsonAsync()
        {
            var json = Json.Net.Curl.GetAsync(@"data\JObjectUnitTest1.json").GetAwaiter().GetResult();
            Assert.IsNotNull(json);
        }
        [Test]
        public void GetJsonFromRemote()
        {
            var json = Json.Net.Curl.Get(GET_JSON_URL);
            Assert.IsNotNull(json);
        }
        [Test]
        public void GetJsonFromRemoteAsync()
        {
            var json = Json.Net.Curl.GetAsync(GET_JSON_URL).GetAwaiter().GetResult();
            Assert.IsNotNull(json);
        }
        [Test]
        public void GetJsonArray()
        {
            var json = Json.Net.Curl.GetJArray(@"data\JArrayUnitTest1.json");
            Assert.IsNotNull(json);
        }
        [Test]
        public void GetJsonArrayFromRemote()
        {
            var json = Json.Net.Curl.GetAsync(GET_JSON_ARRAY_URL);
            Assert.IsNotNull(json);
        }
        [Test]
        public void GetJsonArrayFromRemoteAsync()
        {
            var json = Json.Net.Curl.GetJArrayAsync(GET_JSON_ARRAY_URL).GetAwaiter().GetResult();
            Assert.IsNotNull(json);
        }
        #endregion

        #region save 
        [Test]
        public void Save()
        {
            Json.Net.Curl.Save(@"data\JObjectSave1.json", new Newtonsoft.Json.Linq.JObject() { 
            ["id"] = "1",
            ["user"] = "Khanin",
            ["pass"] = "AAA@@@!!!!"
            });
            Assert.Pass();
        }
        [Test]
        public void SaveAsync()
        {
            Json.Net.Curl.SaveAsync(@"data\JObjectSaveAsync1.json", new Newtonsoft.Json.Linq.JObject()
            {
                ["id"] = "1",
                ["user"] = "Khanin",
                ["pass"] = "AAA@@@!!!!"
            }).GetAwaiter().GetResult();
            Assert.Pass();
        }
        [Test]
        public void SaveJArray()
        {
            Json.Net.Curl.SaveJArray(@"data\JObjectSaveJArray1.json", new JArray(){ new JObject()
            {
                ["id"] = "1",
                ["user"] = "Khanin",
                ["pass"] = "AAA@@@!!!!"
            } });
            Assert.Pass();
        }
        [Test]
        public void SaveJArrayAsync()
        {
            Json.Net.Curl.SaveJArrayAsync(@"data\JObjectSaveJArrayAsync1.json", new JArray(){ new JObject()
            {
                ["id"] = "1",
                ["user"] = "Khanin",
                ["pass"] = "AAA@@@!!!!"
            } }).GetAwaiter().GetResult();
            Assert.Pass();
        }
        [Test]
        public void SaveRemote()
        {
            Json.Net.Curl.Save(@"https://jsonnetcurl.free.beeceptor.com/add", new Newtonsoft.Json.Linq.JObject()
            {
                ["id"] = "1",
                ["user"] = "Khanin",
                ["pass"] = "AAA@@@!!!!"
            });
            Assert.Pass();
        }
        #endregion

        #region  List
        [Test]
        public void ListFiles()
        {
            var list = Json.Net.Curl.List(@"data");
            Assert.Pass();
        }

        #endregion
    }
}