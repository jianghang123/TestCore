using Microsoft.AspNetCore.Http;
using TestCore.Common.Ioc;

namespace TestCore.Common.Helper
{
    /// <summary>
    /// 表现层的一个Web辅助接口
    /// </summary>
    public partial interface IWebHelper : ISingletonDependency
    {
        /// <summary>
        /// 如果存在则获取URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetUrlReferrer();

        /// <summary>
        /// 根据HTTP上下文获取IP地址
        /// </summary>
        /// <returns>IP地址字符串</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// 获取一个值，该值指示当前连接是否已被保护
        /// </summary>
        /// <returns>如果它已被保护，则为true，否则为false</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// 得到的大型主机的位置 
        /// </summary>
        /// <param name="useSsl">是否使用了安全SSL连接</param>
        /// <returns>存储主机位置</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// 如果请求的资源是引擎不需要处理的典型资源之一，则返回TRUE
        /// </summary>
        /// <returns>如果请求针对静态资源文件，则为true</returns>
        bool IsStaticResource();

        /// <summary>
        /// 修改请求查询字符串
        /// </summary>
        /// <param name="url">要修改的URL</param>
        /// <param name="queryStringModification">修改查询字符串</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>新URL</returns>
        string ModifyQueryString(string url, string queryStringModification, string anchor);

        /// <summary>
        /// 从URL中删除查询字符串
        /// </summary>
        /// <param name="url">要修改的URL</param>
        /// <param name="queryString">移除查询字符串</param>
        /// <returns>没有传递查询字符串的新URL</returns>
        string RemoveQueryString(string url, string queryString);

        /// <summary>
        /// 按名称获取查询字符串值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="name">查询参数名</param>
        /// <returns>查询字符串值</returns>
        T QueryString<T>(string name);

        /// <summary>
        /// 获取一个值，该值指示是否将客户端重定向到新位置
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// 获取或设置一个值，该值指示客户端是否使用邮政重定向到新位置
        /// </summary>
        bool IsPostBeingDone { get; set; }

        /// <summary>
        /// 获取当前HTTP请求协议
        /// </summary>
        string CurrentRequestProtocol { get; }

        /// <summary>
        /// 获取指定的HTTP请求URI是否引用本地主机
        /// </summary>
        /// <param name="req">HTTP请求</param>
        /// <returns>如果HTTP请求URI引用到本地主机，则为True</returns>
        bool IsLocalRequest(HttpRequest req);

        /// <summary>
        /// 获取请求的原始路径和完整查询
        /// </summary>
        /// <param name="request">HTTP请求</param>
        /// <returns>原始URL</returns>
        string GetRawUrl(HttpRequest request);

        /// <summary>
        /// 判断是否Ajax请求
        /// </summary>
        /// <returns></returns>
        bool IsAjax();

        #region Session Methods

        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        void WriteSession<T>(string key, T value);

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        void WriteSession(string key, string value);

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <typeparam name="T">转换的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <returns>返回类型T实体</returns>
        T GetSession<T>(string key);

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>   
        string GetStrSession(string key);

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        void RemoveSession(string key);

        #endregion

    }
}
