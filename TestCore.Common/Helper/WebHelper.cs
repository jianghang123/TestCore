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
    /// 表现层的一个Web辅助类
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
        /// 检查当前HTTP请求是否可用
        /// </summary>
        /// <returns>如果可用</returns>
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
        /// 是否指定IP地址
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
        /// 如果存在则获取URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetUrlReferrer()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //URL引用器在某些情况下是空的（例如，在IE 8中）
            return httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
        }

        /// <summary>
        /// 根据HTTP上下文获取IP地址
        /// </summary>
        /// <returns>IP地址字符串</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            var result = string.Empty;
            try
            {
                //首先尝试从转发的头获得IP地址
                if (httpContextAccessor.HttpContext.Request.Headers != null)
                {
                    //X-TunDeD-Of（XFF）HTTP报头字段是用于识别通过HTTP代理或负载均衡器连接到Web服务器的客户端的始发IP地址的阿德FACTO标准
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";
                    if (!string.IsNullOrEmpty(_hostingConfig.ForwardedHttpHeader))
                    {
                        //但在某些情况下，服务器在这种情况下使用其他HTTP报头，管理员可以指定定制转发的HTTP报头（例如，连接IP、X-NotoDeDr.Pro等的CF）
                        forwardedHttpHeaderKey = _hostingConfig.ForwardedHttpHeader;
                    }

                    var forwardedHeader = httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (!StringValues.IsNullOrEmpty(forwardedHeader))
                        result = forwardedHeader.FirstOrDefault();
                }

                //如果此标题不存在，请尝试获取连接远程IP地址
                if (string.IsNullOrEmpty(result) && httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                    result = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }
             
            if (result != null && result.Equals("::1", StringComparison.InvariantCultureIgnoreCase))
                result = "127.0.0.1";

            //"TryParse"不支持带有端口号的IPv4
            if (IPAddress.TryParse(result ?? string.Empty, out IPAddress ip))
                //IP地址有效
                result = ip.ToString();
            else if (!string.IsNullOrEmpty(result))
                //移除端口
                result = result.Split(':').FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 获取一个值，该值指示当前连接是否已被保护
        /// </summary>
        /// <returns>如果它已被保护，则为true，否则为false</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
                return false;

            //检查主机是否使用负载均衡器使用HTTP_CLUSTER_HTTPS
            if (_hostingConfig.UseHttpClusterHttps)
                return httpContextAccessor.HttpContext.Request.Headers["HTTP_CLUSTER_HTTPS"].ToString().Equals("on", StringComparison.OrdinalIgnoreCase);

            //使用HTTP_X_FORWARDED_PROTO?
            if (_hostingConfig.UseHttpXForwardedProto)
                return httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-Proto"].ToString().Equals("https", StringComparison.OrdinalIgnoreCase);

            return httpContextAccessor.HttpContext.Request.IsHttps;
        }

        /// <summary>
        /// 得到的大型主机的位置 
        /// </summary>
        /// <param name="useSsl">是否使用了安全SSL连接</param>
        /// <returns>存储主机位置</returns>
        public virtual string GetStoreHost(bool useSsl)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //尝试从请求主机头获取主机
            var hostHeader = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;

            //向URL添加方案
            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}://{hostHeader.FirstOrDefault()}";

            //确保主机以斜线结束
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }

        /// <summary>
        /// 如果请求的资源是引擎不需要处理的典型资源之一，则返回TRUE
        /// </summary>
        /// <returns>如果请求针对静态资源文件，则为true</returns>
        public virtual bool IsStaticResource()
        {
            if (!IsRequestAvailable())
                return false;

            string path = httpContextAccessor.HttpContext.Request.Path;
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            return contentTypeProvider.TryGetContentType(path, out string _);
        }

        /// <summary>
        /// 修改请求查询字符串
        /// </summary>
        /// <param name="url">要修改的URL</param>
        /// <param name="queryStringModification">修改查询字符串</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>新URL</returns>
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
        /// 从URL中删除查询字符串
        /// </summary>
        /// <param name="url">要修改的URL</param>
        /// <param name="queryString">移除查询字符串</param>
        /// <returns>没有传递查询字符串的新URL</returns>
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
        /// 按名称获取查询字符串值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="name">查询参数名</param>
        /// <returns>查询字符串值</returns>
        public virtual T QueryString<T>(string name)
        {
            if (!IsRequestAvailable())
                return default(T);

            if (StringValues.IsNullOrEmpty(httpContextAccessor.HttpContext.Request.Query[name]))
                return default(T);

            return CommonHelper.To<T>(httpContextAccessor.HttpContext.Request.Query[name].ToString());
        } 

        /// <summary>
        /// 获取一个值，该值指示是否将客户端重定向到新位置
        /// </summary>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = httpContextAccessor.HttpContext.Response;
                //ASP.NET 4回报response.isrequestbeingredirected风格
                int[] redirectionStatusCodes = { StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found};
                return redirectionStatusCodes.Contains(response.StatusCode);
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示客户端是否使用邮政重定向到新位置
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
        /// 获取当前HTTP请求协议
        /// </summary>
        public virtual string CurrentRequestProtocol => IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

        /// <summary>
        /// 获取指定的HTTP请求URI是否引用本地主机
        /// </summary>
        /// <param name="req">HTTP请求</param>
        /// <returns>如果HTTP请求URI引用到本地主机，则为True</returns>
        public virtual bool IsLocalRequest(HttpRequest req)
        { 
            var connection = req.HttpContext.Connection;
            if (IsIpAddressSet(connection.RemoteIpAddress))
            {
                //我们有一个远程地址设置
                return IsIpAddressSet(connection.LocalIpAddress)
                    //本地是相同的远程，那么我们是本地的
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    //否则，如果远程IP地址不是回送地址，那么我们是远程的
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        /// <summary>
        /// 获取请求的原始路径和完整查询
        /// </summary>
        /// <param name="request">HTTP请求</param>
        /// <returns>原始URL</returns>
        public virtual string GetRawUrl(HttpRequest req)
        {
            //首先尝试从请求特征中获取原始目标
            //注：值未被解码
            var rawUrl = req.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            //或手工编写原始URL
            if (string.IsNullOrEmpty(rawUrl))
                rawUrl = $"{req.PathBase}{req.Path}{req.QueryString}";

            return rawUrl;
        }

        /// <summary>
        /// 判断是否Ajax请求
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
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public void WriteSession<T>(string key, T value)
        {
            if (key.IsNullOrEmpty())
                return;
            httpContextAccessor.HttpContext.Session.Set(key, value);
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <typeparam name="T">转换的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <returns>返回类型T实体</returns>
        public T GetSession<T>(string key)
        {
            if(key.IsNullOrEmpty())
                return default(T);
            return httpContextAccessor.HttpContext.Session.Get<T>(key);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public string GetStrSession(string key)
        {
            if (key.IsNullOrEmpty())
                return string.Empty;
            return httpContextAccessor.HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public void RemoveSession(string key)
        {
            if (key.IsNullOrEmpty())
                return;
            httpContextAccessor.HttpContext.Session.Remove(key);
        }

        #endregion
    }
}