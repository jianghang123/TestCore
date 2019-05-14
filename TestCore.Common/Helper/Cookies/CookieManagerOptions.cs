namespace TestCore.Common.Helper.Cookies
{
    /// <summary>
    /// CookieManager参数选项
    /// </summary>
    public class CookieManagerOptions
    {
        /// <summary>
        /// 允许cookie数据在默认情况下进行加密
        /// </summary>
        public bool AllowEncryption { get; set; } = true;

        /// <summary>
        /// 默认Cookie过期时间如果过期时间设置为null Cookie默认时间是1天过期Cookie 
        /// </summary>
        public int DefaultExpireTimeInDays { get; set; } = 1;

        /// <summary>
        /// 将cookie的最大大小发送回客户机。如果一个cookie超过这个大小，它将被分解成多个cookie。将该值设为null以禁用此行为。默认值是4090个字符，这是所有常见浏览器支持的
        //TODO 注意，浏览器可能对每个域的所有cookie的总大小以及每个域的cookie数量有限制
        /// </summary>
        public int? ChunkSize { get; set; } = 4050;

        /// <summary>
        /// 如果不是所有的cookie块都可以在重新组装的请求中使用
        /// </summary>
        public bool ThrowForPartialCookies { get; set; } = true;
    }
}
