using TestCore.Common.Log;
using TestCore.Common.PayCommon.Wxpay;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TestCore.Common.Helper
{
    /// <summary>
    /// 网络工具类。
    /// </summary>
    public sealed class NetUtils
    {
        private static ILog _log = LogUtils.GetLogger(typeof(NetUtils));
        private int _timeout = 100000;

        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }


        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    string encodedValue = WebUtility.UrlEncode(value);

                    postData.Append(encodedValue);
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

        public static string Post(string url, string data, string charset, ContentType type, int timeout, bool isFlterEndTag = false)
        {
            // System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            try
            {
                request = WebRequest.CreateHttp(new Uri(url));
                if (type == ContentType.application_json)
                {
                    request.ContentType = "application/json;charset=" + charset;
                }
                else if (type == ContentType.text_xml)
                {
                    request.ContentType = "text/xml;charset=" + charset;
                }
                else
                {
                    request.ContentType = "application/x-www-form-urlencoded;charset=" + charset;
                }

                var encoding = Encoding.GetEncoding(charset);
                request.Method = "POST";
                //req.Accept = "text/xml,text/javascript";
                request.ContinueTimeout = timeout * 1000;

                byte[] postData = encoding.GetBytes(data);
                reqStream = request.GetRequestStreamAsync().Result;
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Dispose();

                response = (HttpWebResponse)request.GetResponseAsync().Result;
                return isFlterEndTag ? GetResponseAsStringFlterEndTag(response, encoding) : GetResponseAsString(response, encoding);
            }
            catch (WebException e)
            {
                _log.Error("HttpService", e);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _log.Error("StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    _log.Error("StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new WxPayException(e.ToString());
            }
            catch (Exception e)
            {
                _log.Error("HttpService", e);
                throw new WxPayException(e.ToString());
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Dispose();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
        }


        private static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (rsp != null) rsp.Dispose();
            }
        }

        private static string GetResponseAsStringFlterEndTag(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (rsp != null) rsp.Dispose();
            }
        }
    }

    public enum ContentType
    {
        application_json,
        text_xml,
        application_form_urlencoded,
    }
}
