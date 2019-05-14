
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestCore.Common.Helper
{
    public  class HttpClientHelper
    {
        public enum HttpMethodEnum
        {
            POST,
            GET
        }
        public async static Task<T> HttpRequest<T>(string Url, HttpMethodEnum HttpMethod, RequestHeaderDto header, string data)
        {
            string result = null;
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    HttpResponseMessage message = null;
                    if (header != null)
                    {
                        http.DefaultRequestHeaders.Add("staffid", header.Staffid); //当前请求用户StaffId
                        http.DefaultRequestHeaders.Add("timestamp", header.Timestamp); //发起请求时的时间戳（单位：毫秒）
                        http.DefaultRequestHeaders.Add("nonce", header.Nonce); //发起请求时的时间戳（单位：毫秒）
                        http.DefaultRequestHeaders.Add("token", header.Token); //发起请求时的时间戳（单位：毫秒）
                        http.DefaultRequestHeaders.Add("signature", header.Signature); //当前请求内容的数字签名
                    }

                    if (HttpMethod == HttpMethodEnum.POST)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        using (Stream dataStream = new MemoryStream(bytes ?? new byte[0]))
                        {
                            using (HttpContent content = new StreamContent(dataStream))
                            {
                                content.Headers.Add("Content-Type", "application/json");
                                message = await http.PostAsync(Url, content);
                            }
                        }
                    }
                    else if (HttpMethod == HttpMethodEnum.GET)
                    {
                        message = await http.GetAsync(Url);
                    }
                    if (message != null && message.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (message)
                        {
                            using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                            {
                                if (responseStream != null)
                                {
                                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);

                                    result = streamReader.ReadToEnd();

                                    //byte[] responseData = new byte[responseStream.Length];
                                    //responseStream.Read(responseData, 0, responseData.Length);
                                    //result = responseData;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<string> GetResponse(string Url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.twitchtv.v5+json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
                //client.DefaultRequestHeaders.Add("Client-ID", "MyId");

                using (var response = await client.GetAsync(Url))
                {
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync(); // here we return the json response, you may parse it
                }
            }
        }

        public static string EncodeBase64(Encoding encode, string source)
        {
            string enstring = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                enstring = Convert.ToBase64String(bytes);
            }
            catch
            {
                enstring = source;
            }
            return enstring;
        }
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }



    }

    public class RequestHeaderDto
    {
        public string Staffid { set; get; }
        public string Timestamp { set; get; }
        public string Nonce { set; get; }

        public string Token { set; get; }
        public string Signature { set; get; }
    }


}
