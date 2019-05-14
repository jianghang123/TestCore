using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCore.Common.Ioc;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.SysEntity;

namespace TestCore.IRepository.SysAdmin
{
    public interface IRoleRightRepository : IBaseResourceRepository<SysRoleRight>, ISingletonDependency
    {
        //Task<bool> HasRight(int roleId, int nodeId, RightTypeEnum rightType);


        Task<IEnumerable<RoleRightItem>> GetRightListAsync(int roleId, long userId = 0);

        //Task<IEnumerable<SysRoleRight>> GetListAsync(int roleId, string usreName = null);

        Task<UserRight> GetUserRightAsync(int roleId);


        //Task<UserRight> GetUserRightByUserIdAsync(long userId);

        Task<Tuple<List<RoleRightItem>, List<RoleRightItem>>> GetRoleRightItems(string pname, string userName = null);


        Task<UserRight> GetUserRightAsync(string userName);

        ///RoleRight GetRoleRight(string userName);

        Task<int> UpdateRights(int roleId, IEnumerable<SysRoleRight> list);


        Task<int> UpdateRights(string userName, IEnumerable<SysRoleRight> list);



        #region User Right

        Task<int> InsertUserAndRightsAsync(Admin model, string expelFields, List<SysRoleRight> list);

        Task<int> UpdateUserAndRightsAsync(Admin model, string expelFields, List<SysRoleRight> list);


        #endregion

        /// <summary>
        /// 获取栏目的初始权限
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        //Task<IEnumerable<int>> GetRightIdsAsync(int nodeId);


        #region Node

        ////
        //int GetNodeId(string area, string controller, string action = null, string id = null);

        /// <summary>
        /// 获取 NodeId list
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        int[] GetNodeIds(string area, string controller, string action = null, string id = null);

        IEnumerable<SysAdminNode> GetNodeList(string area, string controller, string action = null, string id = null);

        #endregion
    }
}