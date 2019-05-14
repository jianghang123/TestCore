using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Configuration
{
    /// <summary>
    /// 表现层启动配置参数
    /// </summary>
    public partial class TestCoreConfig
    {
        /// <summary>
        /// 获取或设置一个值，该值指示是否在生产环境中显示完整错误。它在开发环境中被忽略（总是启用）
        /// </summary>
        public bool DisplayFullErrorStack { get; set; }

        /// <summary>
        /// 获取或设置静态内容的“Cache控件”标题值
        /// </summary>
        public string StaticFilesCacheControl { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否压缩响应
        /// </summary>
        public bool UseResponseCompression { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应该使用Redis服务器进行缓存（而不是在内存缓存中使用默认值）
        /// </summary>
        public bool RedisCachingEnabled { get; set; }
        /// <summary>
        /// Redis连接字符串。当启用Redis缓存
        /// </summary>
        public string RedisCachingConnectionString { get; set; }
        /// <summary>
        /// 获取或设置一个值，该值指示数据保护系统是否应配置为在Redis数据库中保存密钥
        /// </summary>
        public bool PersistDataProtectionKeysToRedis { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应忽略启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// AES加密密钥
        /// </summary> 
        public string AESEncryptKey { get; set; }
        /// <summary>
        /// DES加密密钥
        /// </summary> 
        public string DESEncryptKey { get; set; }

        /// <summary>
        /// RedisClient连接字符串 
        /// </summary>
        public string RedisClientConnectionString { get; set; }
    }
}
