using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Cache;
using TestCore.Common.Configuration;
using TestCore.IService.SysAdmin;
using static TestCore.Domain.SysEntity.Admin;

namespace TestCore.Services.SysAdmin
{
    /// <summary>
    /// 后台系统用户缓存服务
    /// </summary>
    public class AdminCacheSvc : IAdminCacheSvc
    {
        #region Fields

        private readonly ProjectConfig projectConfig;
        private readonly IStaticCacheManager cacheManager;
        private readonly IAdminSvc _adminSvc;

        #endregion

        #region Ctor

        public AdminCacheSvc(ProjectConfig projectConfig,
            IStaticCacheManager cacheManager,
            IAdminSvc adminSvc)
        {
            this.projectConfig = projectConfig;
            this.cacheManager = cacheManager;
            this._adminSvc = adminSvc;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 用户权限缓存名称
        /// </summary>
        public string UserPermissionCacheName => projectConfig.CacheNamePrefix + "Permission:UserId_";

        #endregion

        #region Methods

        /// <summary>
        /// 设置当前登录用户权限缓存
        /// </summary>
        /// <param name="userId">用户编号</param>
        public void SetPermissionCache(int userId)
        {
            var adminInfo = _adminSvc.GetModel(new { id = userId });
            //TODO 缓存权限值，默认120分钟（若要Redis作为缓存，直接更改appsettings.json中节点CK的RedisCachingEnabled为True 
            cacheManager.Set(UserPermissionCacheName + userId, adminInfo.Limits, projectConfig.CacheExpire);
        }

        /// <summary>
        /// 获取当前登录用户权限缓存
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public UserPermissionCodeEntity GetPermissionCache(int userId)
        {
            var cachedata = cacheManager.Get<UserPermissionCodeEntity>(UserPermissionCacheName + userId);
            if (cachedata == null)
            {
                var adminInfo = _adminSvc.GetModel(new { id = userId });
                var authorizedata = adminInfo.Limits;
                cacheManager.Set(UserPermissionCacheName + userId, authorizedata, projectConfig.CacheExpire);
                cachedata = new UserPermissionCodeEntity() { MpCode = authorizedata };
            }
            return cachedata;
        }

        /// <summary>
        /// 清除当前登录用户缓存
        /// </summary>
        public void Clear()
        {
            //清空用户权限缓存
            cacheManager.RemoveByPattern(UserPermissionCacheName);
        }

        #endregion
    }
}
