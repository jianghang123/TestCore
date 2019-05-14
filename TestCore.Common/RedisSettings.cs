using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common
{
    public class RedisSettings
    {
        /// <summary>
        /// 端口号
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// SSL 连接字符串
        /// </summary>
        public string RedisSSLConnectionString { get; set; }
    }
}
