using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Configuration
{
    /// <summary>
    /// 运行项目相关配置
    /// </summary>
    public partial class ProjectConfig
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Cookie名称前置
        /// </summary>
        public string CookieNamePrefix { get; set; }

        /// <summary>
        /// Cookie有效期时间（分钟）
        /// </summary>
        public int CookieExpire { get; set; }

        /// <summary>
        /// Cache名称前置
        /// </summary>
        public string CacheNamePrefix { get; set; }

        /// <summary>
        /// Cache有效期时间（分钟）
        /// </summary>
        public int CacheExpire { get; set; }
    }
}
