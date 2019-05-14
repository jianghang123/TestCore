namespace TestCore.Common.Dapper
{
    /// <summary>
    /// 数据库连接集合
    /// </summary>
    public class ConnectionStringList
    {
        /// <summary>
        /// 默认数据库连接信息(主要业务库执行读写）
        /// </summary>
        public string DefaultConn { get; set; }

        /// <summary>
        /// 默认数据库连接信息(主要业务库执行读）
        /// </summary>
        public string DefaultReadConn { get; set; }

        /// <summary>
        /// 系统管理库连接信息（执行读写）
        /// </summary>
        public string SysManageConn { get; set; }

        /// <summary>
        /// CMS库连接信息（执行读写）
        /// </summary>
        public string CMSConn { get; set; }
    }
}
