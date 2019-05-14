using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Configuration
{
    /// <summary>
    /// 表现层启动主机配置参数
    /// </summary>
    public partial class HostingConfig
    {
        /// <summary>
        /// 获取或设置自定义转发的HTTP头（例如CF连接IP、X-PROTEDDE-PROTO等）
        /// </summary>
        public string ForwardedHttpHeader { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用Http集群Https
        /// </summary>
        public bool UseHttpClusterHttps { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用Http XForwarded协议
        /// </summary>
        public bool UseHttpXForwardedProto { get; set; }
    }
}
