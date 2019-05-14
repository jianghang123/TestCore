using System.Data;

namespace TestCore.Common.Dapper
{
    /// <summary>
    /// 数据库连接驱动接口
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="enum">数据库连接枚举</param>
        /// <returns></returns>
        IDbConnection CreateConn(DbConnectionEnum @enum = DbConnectionEnum.Default);
    }
}
