using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TestCore.Common.Configuration;
using TestCore.Core.Exceptions;
using TestCore.Common.Infrastructure;
using TestCore.Common.Extensions;

namespace TestCore.Common.Helper
{
    /// <summary>
    /// ���ֲ��һ��Web������
    /// </summary>
    public partial class WebHelper : IWebHelper
    {
        #region Const

        private const string NullIpAddress = "::1";

        #endregion

        #region Fields 

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HostingConfig _hostingConfig;
        //private readonly ITestCoreFileProvider _fileProvider;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="hostingConfig">Hosting config</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        public WebHelper(HostingConfig hostingConfig, 
            IHttpContextAccessor httpContextAccessor)
        {
            this._hostingConfig = hostingConfig;
            this.httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// ��鵱ǰHTTP�����Ƿ����
        /// </summary>
        /// <returns>�������</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
                return false;

            try
            {
                if (httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// �Ƿ�ָ��IP��ַ
        /// </summary>
        /// <param name="address">IP address</param>
        /// <returns></returns>
        protected virtual bool IsIpAddressSet(IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        } 

        #endregion

        #region Methods

        /// <summary>
        /// ����������ȡURL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetUrlReferrer()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //URL��������ĳЩ������ǿյģ����磬��IE 8�У�
            return httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
        }

        /// <summary>
        /// ����HTTP�����Ļ�ȡIP��ַ
        /// </summary>
        /// <returns>IP��ַ�ַ���</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            var result = string.Empty;
            try
            {
                //���ȳ��Դ�ת����ͷ���IP��ַ
                if (httpContextAccessor.HttpContext.Request.Headers != null)
                {
                    //X-TunDeD-Of��XFF��HTTP��ͷ�ֶ�������ʶ��ͨ��HTTP������ؾ��������ӵ�Web�������Ŀͻ��˵�ʼ��IP��ַ�İ���FACTO��׼
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";
                    if (!string.IsNullOrEmpty(_hostingConfig.ForwardedHttpHeader))
                    {
                        //����ĳЩ����£������������������ʹ������HTTP��ͷ������Ա����ָ������ת����HTTP��ͷ�����磬����IP��X-NotoDeDr.Pro�ȵ�CF��
                        forwardedHttpHeaderKey = _hostingConfig.ForwardedHttpHeader;
                    }

                    var forwardedHeader = httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (!StringValues.IsNullOrEmpty(forwardedHeader))
                        result = forwardedHeader.FirstOrDefault();
                }

                //����˱��ⲻ���ڣ��볢�Ի�ȡ����Զ��IP��ַ
                if (string.IsNullOrEmpty(result) && httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                    result = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }
             
            if (result != null && result.Equals("::1", StringComparison.InvariantCultureIgnoreCase))
                result = "127.0.0.1";

            //"TryParse"��֧�ִ��ж˿ںŵ�IPv4
            if (IPAddress.TryParse(result ?? string.Empty, out IPAddress ip))
                //IP��ַ��Ч
                result = ip.ToString();
            else if (!string.IsNullOrEmpty(result))
                //�Ƴ��˿�
                result = result.Split(':').FirstOrDefault();

            return result;
        }

        /// <summary>
        /// ��ȡһ��ֵ����ֵָʾ��ǰ�����Ƿ��ѱ�����
        /// </summary>
        /// <returns>������ѱ���������Ϊtrue������Ϊfalse</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
                return false;

            //��������Ƿ�ʹ�ø��ؾ�����ʹ��HTTP_CLUSTER_HTTPS
            if (_hostingConfig.UseHttpClusterHttps)
                return httpContextAccessor.HttpContext.Request.Headers["HTTP_CLUSTER_HTTPS"].ToString().Equals("on", StringComparison.OrdinalIgnoreCase);

            //ʹ��HTTP_X_FORWARDED_PROTO?
            if (_hostingConfig.UseHttpXForwardedProto)
                return httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-Proto"].ToString().Equals("https", StringComparison.OrdinalIgnoreCase);

            return httpContextAccessor.HttpContext.Request.IsHttps;
        }

        /// <summary>
        /// �õ��Ĵ���������λ�� 
        /// </summary>
        /// <param name="useSsl">�Ƿ�ʹ���˰�ȫSSL����</param>
        /// <returns>�洢����λ��</returns>
        public virtual string GetStoreHost(bool useSsl)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //���Դ���������ͷ��ȡ����
            var hostHeader = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;

            //��URL��ӷ���
            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}://{hostHeader.FirstOrDefault()}";

            //ȷ��������б�߽���
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }

        /// <summary>
        /// ����������Դ�����治��Ҫ����ĵ�����Դ֮һ���򷵻�TRUE
        /// </summary>
        /// <returns>���������Ծ�̬��Դ�ļ�����Ϊtrue</returns>
        public virtual bool IsStaticResource()
        {
            if (!IsRequestAvailable())
                return false;

            string path = httpContextAccessor.HttpContext.Request.Path;
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            return contentTypeProvider.TryGetContentType(path, out string _);
        }

        /// <summary>
        /// �޸������ѯ�ַ���
        /// </summary>
        /// <param name="url">Ҫ�޸ĵ�URL</param>
        /// <param name="queryStringModification">�޸Ĳ�ѯ�ַ���</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>��URL</returns>
        public virtual string ModifyQueryString(string url, string queryStringModification, string anchor)
        {
            if (url == null)
                url = string.Empty;

            if (queryStringModification == null)
                queryStringModification = string.Empty;

            if (anchor == null)
                anchor = string.Empty;

            var str = string.Empty;
            var str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    foreach (var str3 in str.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            var strArray = str3.Split(new[] { '=' });
                            if (strArray.Length == 2)
                            {
                                if (!dictionary.ContainsKey(strArray[0]))
                                {
                                    dictionary[strArray[0]] = strArray[1];
                                }
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (var str4 in queryStringModification.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            var strArray2 = str4.Split(new[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }
                    var builder = new StringBuilder();
                    foreach (var str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }
            if (!string.IsNullOrEmpty(anchor))
            {
                str2 = anchor;
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2)));
        }

        /// <summary>
        /// ��URL��ɾ����ѯ�ַ���
        /// </summary>
        /// <param name="url">Ҫ�޸ĵ�URL</param>
        /// <param name="queryString">�Ƴ���ѯ�ַ���</param>
        /// <returns>û�д��ݲ�ѯ�ַ�������URL</returns>
        public virtual string RemoveQueryString(string url, string queryString)
        {
            if (url == null)
                url = string.Empty;

            if (queryString == null)
                queryString = string.Empty;

            var str = string.Empty;
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    foreach (var str3 in str.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            var strArray = str3.Split(new[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    dictionary.Remove(queryString);

                    var builder = new StringBuilder();
                    foreach (var str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        /// <summary>
        /// �����ƻ�ȡ��ѯ�ַ���ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        /// <param name="name">��ѯ������</param>
        /// <returns>��ѯ�ַ���ֵ</returns>
        public virtual T QueryString<T>(string name)
        {
            if (!IsRequestAvailable())
                return default(T);

            if (StringValues.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Query[name]))
                return default(T);

            return CommonHelper.To<T>(httpContextAccessor.HttpContext.Request.Query[name].ToString());
        } 

        /// <summary>
        /// ��ȡһ��ֵ����ֵָʾ�Ƿ񽫿ͻ����ض�����λ��
        /// </summary>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = httpContextAccessor.HttpContext.Response;
                //ASP.NET 4�ر�response.isrequestbeingredirected���
                int[] redirectionStatusCodes = { StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found};
                return redirectionStatusCodes.Contains(response.StatusCode);
            }
        }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�ͻ����Ƿ�ʹ�������ض�����λ��
        /// </summary>
        public virtual bool IsPostBeingDone
        {
            get
            {
                if (httpContextAccessor.HttpContext.Items["Caiba.IsPOSTBeingDone"] == null)
                    return false;

                return Convert.ToBoolean(httpContextAccessor.HttpContext.Items["Caiba.IsPOSTBeingDone"]);
            }
            set
            {
                httpContextAccessor.HttpContext.Items["Caiba.IsPOSTBeingDone"] = value;
            }
        }

        /// <summary>
        /// ��ȡ��ǰHTTP����Э��
        /// </summary>
        public virtual string CurrentRequestProtocol => IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

        /// <summary>
        /// ��ȡָ����HTTP����URI�Ƿ����ñ�������
        /// </summary>
        /// <param name="req">HTTP����</param>
        /// <returns>���HTTP����URI���õ�������������ΪTrue</returns>
        public virtual bool IsLocalRequest(HttpRequest req)
        { 
            var connection = req.HttpContext.Connection;
            if (IsIpAddressSet(connection.RemoteIpAddress))
            {
                //������һ��Զ�̵�ַ����
                return IsIpAddressSet(connection.LocalIpAddress)
                    //��������ͬ��Զ�̣���ô�����Ǳ��ص�
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    //�������Զ��IP��ַ���ǻ��͵�ַ����ô������Զ�̵�
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        /// <summary>
        /// ��ȡ�����ԭʼ·����������ѯ
        /// </summary>
        /// <param name="request">HTTP����</param>
        /// <returns>ԭʼURL</returns>
        public virtual string GetRawUrl(HttpRequest req)
        {
            //���ȳ��Դ����������л�ȡԭʼĿ��
            //ע��ֵδ������
            var rawUrl = req.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            //���ֹ���дԭʼURL
            if (string.IsNullOrEmpty(rawUrl))
                rawUrl = $"{req.PathBase}{req.Path}{req.QueryString}";

            return rawUrl;
        }

        /// <summary>
        /// �ж��Ƿ�Ajax����
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAjax()
        {
            bool result = false;
            var reqHeaders = httpContextAccessor.HttpContext.Request.Headers;
            if (reqHeaders != null)
            {
                var xreq = reqHeaders.ContainsKey("x-requested-with");
                if (xreq)
                {
                    result = reqHeaders["x-requested-with"] == "XMLHttpRequest";
                }
            }
            return result;
        }

        #endregion

        #region Session Methods

        /// <summary>
        /// дSession
        /// </summary>
        /// <typeparam name="T">Session��ֵ������</typeparam>
        /// <param name="key">Session�ļ���</param>
        /// <param name="value">Session�ļ�ֵ</param>
        public void WriteSession<T>(string key, T value)
        {
            if (key.IsNullOrEmpty())
                return;
            httpContextAccessor.HttpContext.Session.Set(key, value);
        }

        /// <summary>
        /// дSession
        /// </summary>
        /// <param name="key">Session�ļ���</param>
        /// <param name="value">Session�ļ�ֵ</param>
        public void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// ��ȡSession��ֵ
        /// </summary>
        /// <typeparam name="T">ת��������</typeparam>
        /// <param name="key">Session�ļ���</param>
        /// <returns>��������Tʵ��</returns>
        public T GetSession<T>(string key)
        {
            if(key.IsNullOrEmpty())
                return default(T);
            return httpContextAccessor.HttpContext.Session.Get<T>(key);
        }

        /// <summary>
        /// ��ȡSession��ֵ
        /// </summary>
        /// <param name="key">Session�ļ���</param>        
        public string GetStrSession(string key)
        {
            if (key.IsNullOrEmpty())
                return string.Empty;
            return httpContextAccessor.HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// ɾ��ָ��Session
        /// </summary>
        /// <param name="key">Session�ļ���</param>
        public void RemoveSession(string key)
        {
            if (key.IsNullOrEmpty())
                return;
            httpContextAccessor.HttpContext.Session.Remove(key);
        }

        #endregion
    }
}