using System.Collections.Generic;
using TestCore.Common.Ioc;
using static TestCore.Domain.SysEntity.Admin;

namespace TestCore.IService.SysAdmin
{
    /// <summary>
    /// 后台系统用户缓存服务接口
    /// </summary>
    public interface IAdminCacheSvc : ISingletonDependency
    {
        /// <summary>
        /// 设置当前登录用户权限缓存
        /// </summary>
        /// <param name="userId">用户编号</param>
        void SetPermissionCache(int userId);

        /// <summary>
        /// 获取当前登录用户权限缓存
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        UserPermissionCodeEntity GetPermissionCache(int userId);

        /// <summary>
        /// 清除当前登录用户缓存
        /// </summary>
        void Clear();
    }
}
