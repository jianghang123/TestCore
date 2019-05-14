using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Configuration
{
    /// <summary>
    /// APP Token配置参数
    /// </summary>
    public class TokensConfig
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { set; get; }
        /// <summary>
        /// 可设置为应用使用的真实域名
        /// </summary>
        public string Issuer { set; get; }
        /// <summary>
        /// 观众
        /// </summary>
        public string Audience { set; get; }
        /// <summary>
        /// 访问Token有效时间分钟
        /// </summary>
        public int AccessTokenExpirationMinutes { set; get; }
        /// <summary>
        /// 刷新Token有效时间分钟
        /// </summary>
        public int RefreshTokenExpirationMinutes { set; get; }
        /// <summary>
        /// 允许来自同一用户的多次登录
        /// </summary>
        public bool AllowMultipleLoginsFromTheSameUser { set; get; }
        /// <summary>
        /// 允许用户退出所有活动的客户端
        /// </summary>
        public bool AllowSignoutAllUserActiveClients { set; get; }
    }
}
