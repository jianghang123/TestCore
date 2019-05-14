using System.ComponentModel;

namespace TestCore.Common.Dapper
{
    /// <summary>
    /// 数据库连接字符串类型枚举
    /// </summary>
    public enum DbConnectionEnum
    {
        [Description("默认数据库连接信息(主要业务库执行读写）")]
        Default = 0,

        [Description("默认数据库连接信息(主要业务库执行读）")]
        DefaultRead = 1,

        [Description("系统管理库连接信息（执行读写）")]
        SysManage = 2,

        [Description("CMS库连接信息（执行读写）")]
        CMS = 3
    }
}
