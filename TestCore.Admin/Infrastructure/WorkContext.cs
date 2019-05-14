using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TestCore.Admin.Models;
using TestCore.Common.Configuration;
using TestCore.IService.SysAdmin;
using ICookieManager = TestCore.Common.Helper.Cookies.ICookieManager;

namespace TestCore.Admin.Infrastructure
{
    public class WorkContext : IWorkContext
    {
        #region Fields

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICookieManager cookieManager;
        private readonly IAdminSvc _adminSvc;
        private readonly ProjectConfig projectConfig;

        #endregion

        #region Ctor

        public WorkContext(IHttpContextAccessor httpContextAccessor,
            ICookieManager cookieManager,
            IAdminSvc adminSvc,
            ProjectConfig projectConfig)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cookieManager = cookieManager;
            this._adminSvc = adminSvc;
            this.projectConfig = projectConfig;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 用户Key[唯一标识]
        /// </summary>
        // TODO 设置Cookie过期时间为默认的1天
        public virtual string UserKey => cookieManager.GetOrSet<string>(projectConfig.CookieNamePrefix + "UserKey", () => Guid.NewGuid().ToString(), projectConfig.CookieExpire);

        /// <summary>
        /// 获取当前登录用户编号
        /// </summary>
        /// <returns></returns>
        public async Task<UserClaimModel> GetCurrentUserClaim()
        {
            UserClaimModel userClaimModel = new UserClaimModel();
            int userId = 0;
            var auth = await httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Succeeded)
            {
                string[] info = auth.Principal.Identity.Name.Split("|||");
                Int32.TryParse(info[0], out userId);
                userClaimModel.UserName = info[1];
            }
            userClaimModel.UserId = userId;
            return userClaimModel;
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<TestCore.Domain.SysEntity.Admin> GetCurrentUser()
        {
            var userClaim = await GetCurrentUserClaim();
            //return new Domain.SysEntity.Admin() { };
            return await _adminSvc.GetModelAsync(new { adminname = userClaim.UserName});
        }

        #endregion

    }
}
