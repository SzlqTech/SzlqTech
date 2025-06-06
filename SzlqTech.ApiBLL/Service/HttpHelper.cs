using Newtonsoft.Json;
using NLog;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net;
using System.Text;


namespace SzlqTech.ApiBLL.Service
{

    public class HttpHelper
    {
        private const string Post = "POST";

        private const string Get = "GET";

        private const string ApplicationJson = "application/json";

        private const string ApplicationForm = "application/x-www-form-urlencoded";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static string BuildGetQueryParams(Dictionary<string, object> map)
        {
            if (map == null || map.Count == 0)
            {
                return "";
            }

            using IEnumerator<KeyValuePair<string, object>> enumerator = ((IEnumerable<KeyValuePair<string, object>>)new SortedDictionary<string, object>(map)).GetEnumerator();
            StringBuilder stringBuilder = new StringBuilder("?");
            while (enumerator.MoveNext())
            {
                string key = enumerator.Current.Key;
                object value = enumerator.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    stringBuilder.Append(key).Append("=").Append(value)
                        .Append("&");
                }
            }

            return stringBuilder.ToString().Trim('&');
        }

        private static Stream GZipCompress(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using MemoryStream memoryStream = new MemoryStream();
            using GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true);
            gZipStream.Write(bytes, 0, bytes.Length);
            gZipStream.Close();
            return new MemoryStream(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        private static async Task<Stream> GZipCompressAsync(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using MemoryStream memoryStream = new MemoryStream();
            using GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true);
            await gZipStream.WriteAsync(bytes, 0, bytes.Length);
            gZipStream.Close();
            return new MemoryStream(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        private string GZipDecompress(Stream stream)
        {
            byte[] array = new byte[100];
            int num = 0;
            using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
            using MemoryStream memoryStream = new MemoryStream();
            while ((num = gZipStream.Read(array, 0, array.Length)) != 0)
            {
                memoryStream.Write(array, 0, num);
            }

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static string HttpRequestToken(string url, Dictionary<string, string> requestMap, int timeout = 20)
        {
            using HttpClient httpClient = new HttpClient(new HttpClientHandler
            {
                UseCookies = false
            });
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMap));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            return httpClient.PostAsync(url, httpContent).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
        }

        public static string GetJsonByTokenAuth(string url, Dictionary<string, object>? requestMap, string auth, string token, int timeout = 10)
        {
            string text = string.Empty;
            if (requestMap != null)
            {
                text = BuildGetQueryParams(requestMap);
            }

            using HttpClient httpClient = new HttpClient(new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            });
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            if (!string.IsNullOrEmpty(auth))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", auth);
            }

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Blade-Auth", token);
            }

            return httpClient.GetAsync(url + text).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
        }

        public static string PostFileByTokenAuth(string url, string path, string token)
        {
            using HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            using MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new ByteArrayContent(File.ReadAllBytes(path)), token, Path.GetFileName(path));
            return httpClient.PostAsync(url, multipartFormDataContent).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
        }

        public static string PostFormByTokenAuth(string url, IEnumerable<KeyValuePair<string, string>> requestMap, string auth, string token, int timeout = 10)
        {
            using HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            using HttpContent content = new FormUrlEncodedContent(requestMap);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            if (!string.IsNullOrEmpty(auth))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", auth);
            }

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Blade-Auth", token);
            }

            using HttpResponseMessage httpResponseMessage = httpClient.PostAsync(url, content).Result;
            return httpResponseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
        }

        public static string PostJsonByTokenAuth(string url, string json, string auth, string token, int timeout = 10)
        {
            using HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            if (!string.IsNullOrEmpty(auth))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", auth);
            }

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Blade-Auth", token);
            }

            return httpClient.PostAsync(url, content).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
        }

        public static string PostGzipJsonByTokenAuth(string url, string json, string token, int timeout = 60)
        {
            using HttpClient httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                UseCookies = false,
                PreAuthenticate = true
            });
            using Stream content = GZipCompress(json);
            using StreamContent streamContent = new StreamContent(content);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            return httpClient.PostAsync(url, streamContent).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
        }

        public static string WebHttpRequestToken(string url, IDictionary<string, string> requestParams, Encoding encoding, out HttpStatusCode httpStatusCode, string auth, string token, int timeout = 0, int readWriteTimeout = 0)
        {
            byte[] array = BuildRequestParam(requestParams, Encoding.UTF8);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            if (timeout > 0)
            {
                httpWebRequest.Timeout = 1000 * timeout;
            }

            if (readWriteTimeout > 0)
            {
                httpWebRequest.ReadWriteTimeout = 1000 * readWriteTimeout;
            }

            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Headers.Add("Authorization", auth);
            httpWebRequest.Headers.Add("User-Type", token);
            httpWebRequest.ContentLength = array.Length;
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(array, 0, array.Length);
            }

            using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
            {
                result = streamReader.ReadToEnd();
            }

            httpStatusCode = httpWebResponse.StatusCode;
            return result;
        }

        private static byte[] BuildRequestParam(IDictionary<string, string> requestParams, Encoding encoding)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Enumerable.Select(requestParams, (KeyValuePair<string, string> pair, int index) => new
            {
                Pair = pair,
                Index = index
            }))
            {
                stringBuilder.Append(item.Pair.Key.Trim() + "=" + (item.Pair.Value ?? string.Empty).Trim());
                if (item.Index < requestParams.Keys.Count - 1)
                {
                    stringBuilder.Append("&");
                }
            }

            return encoding.GetBytes(stringBuilder.ToString());
        }

        public static async Task<string> HttpRequestCookieAsync(string url, Dictionary<string, string> requestMap, int timeout = 20)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false
            };
            using HttpClient httpClient = new HttpClient(handler);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMap));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent);
            foreach (string item in Enumerable.SingleOrDefault(httpResponseMessage.Headers, (KeyValuePair<string, IEnumerable<string>> header) => header.Key == "Set-Cookie").Value)
            {
                if (item.Contains("JSESSIONID"))
                {
                    GlobalContext.Cookie = item.Split(';')[0];
                }
            }

            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public static async Task<string> HttpRequestTokenAsync(string url, Dictionary<string, string> requestMap, int timeout = 20)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false
            };
            using HttpClient httpClient = new HttpClient(handler);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMap));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            HttpResponseMessage obj = await httpClient.PostAsync(url, httpContent);
            obj.EnsureSuccessStatusCode();
            return await obj.Content.ReadAsStringAsync();
        }

        public static async Task<string> GetJsonAsync(string url, Dictionary<string, object>? getParams, Dictionary<string, string> requestHeaders, int timeout = 10)
        {
            string text = string.Empty;
            if (getParams != null)
            {
                text = BuildGetQueryParams(getParams);
            }

            HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            foreach (KeyValuePair<string, string> requestHeader in requestHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(requestHeader.Key, requestHeader.Value);
            }

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url + text);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                return HttpStatusCode.Unauthorized.ToString();
            }

            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public static async Task<string> PostJsonAsync(string url, string json, Dictionary<string, string> requestHeaders, int timeout = 10)
        {
            using HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            using HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            foreach (KeyValuePair<string, string> requestHeader in requestHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(requestHeader.Key, requestHeader.Value);
            }

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                return HttpStatusCode.Unauthorized.ToString();
            }

            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public static async Task<string> PostFileAsync(string url, string filePath, Dictionary<string, string> requestHeaders, int timeout = 10)
        {
            using HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            foreach (KeyValuePair<string, string> requestHeader in requestHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(requestHeader.Key, requestHeader.Value);
            }

            using (new MultipartFormDataContent())
            {
                using FileStream fileStream = File.OpenRead(filePath);
                HttpContent content = new StreamContent(fileStream);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, content);
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return HttpStatusCode.Unauthorized.ToString();
                }

                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> PostFormAsync(string url, Dictionary<string, string> form, Dictionary<string, string> requestHeaders, int timeout = 10)
        {
            using HttpClientHandler handler = new HttpClientHandler
            {
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            using HttpContent httpContent = new FormUrlEncodedContent(form);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            foreach (KeyValuePair<string, string> requestHeader in requestHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(requestHeader.Key, requestHeader.Value);
            }

            using HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return HttpStatusCode.Unauthorized.ToString();
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> PostGzipJsonAsync(string url, string json, Dictionary<string, string> requestHeaders, int timeout = 60)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                UseCookies = false,
                PreAuthenticate = true
            };
            using HttpClient httpClient = new HttpClient(handler);
            using Stream stream = await GZipCompressAsync(json);
            using StreamContent httpContent = new StreamContent(stream);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            foreach (KeyValuePair<string, string> requestHeader in requestHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(requestHeader.Key, requestHeader.Value);
            }

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                return HttpStatusCode.Unauthorized.ToString();
            }

            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public static async Task<T?> HttpRequestTokenAsync<T>(string url, Dictionary<string, string> requestMap, int timeout = 20) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await HttpRequestTokenAsync(url, requestMap, timeout));
        }

        public static async Task<T?> GetAsync<T>(string url, Dictionary<string, object>? getParams, Dictionary<string, string> requestHeaders, int timeout = 10) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await GetJsonAsync(url, getParams, requestHeaders, timeout));
        }

        public static async Task<T?> PostJsonAsync<T>(string url, string json, Dictionary<string, string> requestHeaders, int timeout = 10) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await PostJsonAsync(url, json, requestHeaders, timeout));
        }

        public static async Task<T?> PostFileAsync<T>(string url, string filePath, Dictionary<string, string> requestHeaders, int timeout = 10) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await PostFileAsync(url, filePath, requestHeaders, timeout));
        }

        public static async Task<T?> PostFormAsync<T>(string url, Dictionary<string, string> form, Dictionary<string, string> requestHeaders, int timeout = 10) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await PostFormAsync(url, form, requestHeaders, timeout));
        }

        public static async Task<T?> PostGzipJsonAsync<T>(string url, string json, Dictionary<string, string> requestHeaders, int timeout = 60) where T : class
        {
            return JsonConvert.DeserializeObject<T>(await PostGzipJsonAsync(url, json, requestHeaders, timeout));
        }
    }
}
