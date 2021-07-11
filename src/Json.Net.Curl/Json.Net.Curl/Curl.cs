using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Json.Net
{
    public static class Curl
    {
        private const long TIMEOUT_MS = (1000 * 60) * 3;
        /// <summary>
        /// Get JSON from local or Web Location.
        /// </summary>
        /// <param name="location">Local file path or Web Url e.g : C:\data\my.json , https://myserver.com/data.json </param>
        /// <param name="metadata">Http Headers</param>
        /// <param name="timeoutMS">Http Timeout in millisecond </param>
        /// <returns></returns>
        public static JObject Get(string location, Dictionary<string, string> metadata = null, long timeoutMS = TIMEOUT_MS)
        {
            return GetAsync(location, metadata, timeoutMS).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get Data from local or Web Location.
        /// </summary>
        /// <param name="location">Local file path or Web Url e.g : C:\data\my.json , https://myserver.com/data.json </param>
        /// <param name="metadata">Http Headers</param>
        /// <param name="timeoutMS">Http Timeout in millisecond </param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string location, Dictionary<string, string> metadata = null, long timeoutMS = TIMEOUT_MS)
        {
            if (IsLocalFile(location))
            {
                if (File.Exists(location))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(location));
                }
                else
                {
                    throw new FileNotFoundException($"Path : {location}");
                }
            }
            else
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };
                    using (HttpClient client = new HttpClient(handler, true))
                    {
                        AddHeaders(client, metadata);
                        client.Timeout = TimeSpan.FromMilliseconds(timeoutMS);
                        var result = await client.GetAsync(location);
                        if (result.IsSuccessStatusCode)
                        {
                            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
                        }
                        else
                        {
                            throw new HttpRequestException($"HttpStatus: {result.StatusCode}");
                        }
                    }
                }
            }
        }
     
        public static async Task<JObject> GetAsync(string location, Dictionary<string, string> metadata = null, long timeoutMS = TIMEOUT_MS)
        {
            return await GetAsync<JObject>(location, metadata,timeoutMS);
        }
        private static void AddHeaders(HttpClient httpClient, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (header.Key.ToLower() == "authorization")
                    {
                        var authValues = header.Value.Split(' ');
                        if (authValues.Length == 2)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authValues[0], authValues[1]);
                        }
                    }
                    else if (header.Key.ToLower() == "content-type") { }
                    else
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }
        }
        public static JArray GetJArray(string location, Dictionary<string, string> metadata = null, long timeoutMS = TIMEOUT_MS)
        {
            return GetJArrayAsync(location, metadata,timeoutMS).GetAwaiter().GetResult();
        }
        public static async Task<JArray> GetJArrayAsync(string location, Dictionary<string, string> metadata = null, long timeoutMS = TIMEOUT_MS)
        {
            return await GetAsync<JArray>(location, metadata, timeoutMS);
        }

        public static void Save(string location, JObject data, Dictionary<string, string> metadata = null, HttpMethod method = null, long timeoutMS = TIMEOUT_MS)
        {
            Save<object>(location, data.ToString(), metadata, method, timeoutMS).GetAwaiter().GetResult();
        }
        public static async Task SaveAsync(string location, JObject data, Dictionary<string, string> metadata = null, HttpMethod method = null, long timeoutMS = TIMEOUT_MS)
        {
            await Save<object>(location, data.ToString(), metadata, method, timeoutMS);
        }
        public static void SaveJArray(string location, JArray data, Dictionary<string, string> metadata = null, HttpMethod method = null, long timeoutMS = TIMEOUT_MS)
        {
            Save<object>(location, data.ToString(), metadata, method, timeoutMS).GetAwaiter().GetResult();
        }
        public static async Task SaveJArrayAsync(string location, JArray data, Dictionary<string, string> metadata = null, HttpMethod method = null, long timeoutMS = TIMEOUT_MS)
        {
            await Save<object>(location, data.ToString(), metadata, method, timeoutMS);
        }
        public static async Task<T> Save<T>(string location, string data, Dictionary<string, string> metadata = null, HttpMethod method = null, long timeoutMS = TIMEOUT_MS)
        {
            if (IsLocalFile(location))
            {


                await File.WriteAllTextAsync(location, data);
                return default(T);

            }
            else
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };
                    using (HttpClient client = new HttpClient(handler, true))
                    {
                        if(method == null)
                        {
                            method = HttpMethod.Post;
                        }

                        AddHeaders(client, metadata);
                        client.Timeout = TimeSpan.FromMilliseconds(timeoutMS);

                        var bytearraycontent = new ByteArrayContent(
                      Encoding.UTF8.GetBytes(data));
                        bytearraycontent.Headers.ContentType = GetContentType(metadata);

                        var request = new HttpRequestMessage(method, location)
                        {
                            Content = bytearraycontent
                        };
                        var result = await client.SendAsync(request);
                        if (!result.IsSuccessStatusCode)
                        {
                            throw new HttpRequestException($"HttpStatus: {result.StatusCode}");
                        }
                        else
                        {
                            if (typeof(T) != typeof(object))
                            {
                                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await request.Content.ReadAsStringAsync());
                            }
                            else
                            {
                                return default(T);
                            }
                        }
                    }
                }
            }
        }


        public static List<FileListModel> List(string location)
        {
            if (IsLocalFile(location))
            {
                if (Directory.Exists(location))
                {
                    var list = Directory.GetFiles(location).Select(f =>
                    {

                        var fi = new FileInfo(Path.Combine(f));

                        return new FileListModel()
                        {
                            FullName = fi.FullName,
                            Name = fi.Name
                        };
                    }).ToList();

                    list.AddRange(Directory.GetDirectories(location).Select(f =>
                   {
                       var di = new DirectoryInfo(Path.Combine(f));
                       return new FileListModel()
                       {
                           IsDirectory = true,
                           FullName = di.FullName,
                           Name = di.Name
                       };
                   }).ToList());
                    return list;
                }
                else
                {
                    throw new DirectoryNotFoundException($"Path: {location}");
                }
            }
            else
            {
                throw new NotSupportedException("List only applicable for local file system");
            }
        }

            private static  MediaTypeHeaderValue GetContentType(Dictionary<string, string> headers)
        {
            var contetType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            if (headers != null)
            {
                var ctype = headers.Where(f => f.Key.ToLower() == "content-type").
                    Select(f =>
                    {
                        contetType = new System.Net.Http.Headers.MediaTypeHeaderValue(f.Value);
                        return true;
                    });
            }
            return contetType;
        }

        private static bool IsLocalFile(string location)
        {
            location = location.Trim().ToLower();
            return !(location.IndexOf("http") == 0 || location.IndexOf("https") == 0);
        }
    }
}
