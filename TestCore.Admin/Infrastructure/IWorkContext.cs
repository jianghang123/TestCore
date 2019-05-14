using System.Threading.Tasks;
using TestCore.Admin.Models;
using TestCore.Common.Ioc;

namespace TestCore.Admin.Infrastructure
{
    public interface IWorkContext : ISingletonDependency
    {
        /// <summary>
        /// 用户Key[唯一标识]
        /// </summary>
        string UserKey { get; }

        /// <summary>
        /// 获取当前登录用户编号
        /// </summary>
        /// <returns></returns>
        Task<UserClaimModel> GetCurrentUserClaim();

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        Task<TestCore.Domain.SysEntity.Admin> GetCurrentUser();
    }
}
