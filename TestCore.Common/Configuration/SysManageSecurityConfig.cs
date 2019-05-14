using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Configuration
{
    /// <summary>
    /// 系统后台管理配置
    /// </summary>
    public class SysManageSecurityConfig
    {
        /// <summary>
        /// 管理用户初始口令
        /// </summary>
        public string InitPwd { get; set; } = "123456";

        /// <summary>
        /// 启用管理用户操作日志[true和false]
        /// </summary>
        public bool IsLog { get; set; }

        /// <summary>
        /// 管理用户最大登陆失败次数
        /// </summary>
        public int MaxLoginFailedTimes { get; set; } = 5;
    }
}
