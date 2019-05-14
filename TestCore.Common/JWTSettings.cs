using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common
{
    public class JWTSettings
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 观众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// token期限:s
        /// </summary>
        public int AccessExpiration { get; set; }

        /// <summary>
        /// 刷新token期限:s
        /// </summary>
        public int RefreshExpiration { get; set; }

    }
}
