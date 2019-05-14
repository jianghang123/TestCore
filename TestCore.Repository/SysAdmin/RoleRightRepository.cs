using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Common.Helper;
using TestCore.Data.Dapper;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Entity;
using TestCore.Domain.Enums;
using TestCore.Domain.SysEntity;
using TestCore.IRepository.SysAdmin;

namespace Caiba.Repositories.Sys
{
    public class RoleRightRepository : BaseResourceRepository<SysRoleRight>, IRoleRightRepository
    {

        public RoleRightRepository(IResourceRepository resourceRepos) 
            : base(resourceRepos, TableEnum.None)
        {

        }


        public async Task<IEnumerable<RoleRightItem>> GetRightListAsync(int roleId, long userId = 0)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
                var list = await conn.QuerySimpleListAsync<RoleRightItem, SysRoleRight>(new { roleId, userId });

                return list.Distinct();
            }
        }

        //public async Task<IEnumerable<SysRoleRight>> GetListAsync(int roleId, string usreName = null)
        //{
        //    using (var conn = ConnectionFactory.OpenConnection())
        //    {
        //        return await conn.QueryListAsync<SysRoleRight>(new { roleId, usreName });
        //    }
        //}
        
        public async Task<UserRight> GetUserRightAsync(int roleId)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
                UserRight roleRight = new UserRight { RoleId = roleId };

                roleRight.IsSupper = await conn.QueryModelAsync<bool, SysRole>(nameof(SysRole.IsSupper),
                    new { id = roleId });
                if (!roleRight.IsSupper)
                {
                    var list = await conn.QuerySimpleListAsync<RoleRightItem, SysRoleRight>( new { roleId, UserId = 0 } );
                    roleRight.Rights = list.Distinct().ToArray();
                }
                return roleRight;
            }
        }

        //public async Task<UserRight> GetUserRightByUserIdAsync(long userId)
        //{
        //    using (var conn = ConnectionFactory.OpenConnection())
        //    {
        //        var userRole = await conn.QueryModelAsync<ViewUserRole>(new { userId });

        //        UserRight userRight = new UserRight
        //        {
        //            RoleId = userRole.RoleId,
        //            IsSupper = userRole.IsSupper,
        //            GroupId = userRole.GroupId,
        //            SiteIds = userRole.SiteIds,
        //            OperId = userRole.OperId,
        //            IsChild = userRole.IsChild,
        //            ParentName = userRole.ParentName
        //        };
        //        if (!userRole.IsSupper)
        //        {
        //            object where = new { userRole.RoleId };
        //            if (userRole.IsChild)
        //            {
        //                where = new { userId };
        //            }
        //            var list = await conn.QuerySimpleListAsync<RoleRightItem, SysRoleRight>(where);
        //            userRight.Rights = list.Distinct().ToArray();
        //        }
        //        return userRight;
        //    }
        //}

        public async Task<Tuple<List<RoleRightItem>, List<RoleRightItem>>> GetRoleRightItems(string pname, string userName = null)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
 
                var pRole = await conn.QueryModelAsync<ViewUserRole>(new { UserName = pname });
 
                var parentList = (await conn.QuerySimpleListAsync<RoleRightItem, SysRoleRight>(new { RoleId = pRole.RoleId, UserId = 0 })).ToList();
              
                var list = new  List<RoleRightItem>();

                if (!string.IsNullOrEmpty(userName))
                {
                    var role = await conn.QueryModelAsync<ViewUserRole>(new { userName });
                    list = (await conn.QuerySimpleListAsync<RoleRightItem, SysRoleRight>(new { UserId = role.UserId })).ToList();
                }
                return Tuple.Create(parentList, list);
            }
        }

        public async Task<UserRight> GetUserRightAsync(string userName)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
                var userRole = await conn.QueryModelAsync<ViewUserRole>(new { userName });

                UserRight userRight = new UserRight
                {
                    UserId = userRole.UserId,
                    UserName = userRole.UserName,
                    RoleId = userRole.RoleId,
                    IsSupper = userRole.IsSupper,
                    GroupId = userRole.GroupId,
                    SiteId = userRole.SiteId,
                    SiteIds = userRole.SiteIds,
                    OperId = userRole.OperId,
                    IsChild = userRole.IsChild,
                    ParentName = userRole.IsChild ?  userRole.ParentName : userRole.UserName
                };
                if (!userRole.IsSupper)
                {
                    object where = new { userRole.RoleId, UserId = 0 };
                    if(userRole.IsChild)
                    {
                        where = new { userRole.UserId };
                    }
                    var list = await conn.QuerySimpleListAsync<RoleRightItem, SysRoleRight>(where);
                    userRight.Rights = list.Distinct().ToArray();
                }
                return userRight;
            }
        }
 

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> UpdateRights(int roleId, IEnumerable<SysRoleRight> list)
        {
            if (roleId == 0) return 0;

            using (var conn = ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                var where = new { roleId, UserId = 0 };

                var oldList = (await conn.QuerySimpleListAsync<RoleRightItem, SysRoleRight>( where, null,tran )).ToList();
                var newList = list.Select(c => new RoleRightItem { NodeId = c.NodeId, RightId = c.RightId }).ToList();
                var surList = this.GetSurplusList(oldList, newList);

                ///删除角色的权限
                int row = await conn.DeleteAsync<SysRoleRight>(new { roleId, UserId = 0 }, tran);
                 //插入新的权限
                row += await conn.InsertListAsync(list, null, tran);

                //删除该角色下用户多餘的权限
                if(surList.Any())
                {
                    foreach(var item in surList)
                    {
                        row += await conn.DeleteAsync<SysRoleRight>(new { roleId, NodeId = item.NodeId, RightId = item.RightId }, tran);
                    }
                }
                tran.Commit();

                return row;
            }
        }


        #region User Right
        public async Task<int> InsertUserAndRightsAsync(Admin model, string expelFields, List<SysRoleRight> list)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                long userId = await conn.InsertWithReturnIdentityAsync<Admin>(model, expelFields, tran);

                int row = 1;

                row += await conn.DeleteAsync<SysRoleRight>(new { userId }, tran);

                if (list != null && list.Any())
                {
                    list.ForEach(  (item) => {
                        item.UserId = userId;
                    });
                    row += await conn.InsertListAsync(list, null, tran);
                }
                tran.Commit();

                return row;
            }
        }

        public async Task<int> UpdateUserAndRightsAsync(Admin model, string expelFields, List<SysRoleRight> list)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                int row = await conn.UpdateAsync<Admin>(model, expelFields, tran);

                row += await conn.DeleteAsync<SysRoleRight>(new { model.Id }, tran);

                if (list != null && list.Any())
                {
                    row += await conn.InsertListAsync(list, null, tran);
                }
                tran.Commit();

                return row;
            }
        }

        #endregion

        /// <summary>
        /// 两个列表比较，找出不一样的数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        //private  Tuple<List<RoleRightItem>, List<RoleRightItem>> GetSurplusList(List<RoleRightItem> list1, List<RoleRightItem> list2)
        //{
        //    var surList1 = new List<RoleRightItem>();
        //    var surList2 = new List<RoleRightItem>();

        //    if (list1 == null || !list1.Any()) return Tuple.Create(surList1, list2);
        //    if (list2 == null || !list2.Any()) return Tuple.Create(list1, surList2);

        //    foreach (var item in list1)
        //    {
        //        if(!list2.Where(c=> c.NodeId == item.NodeId && c.RightId == item.RightId).Any())
        //        {
        //            surList1.Add(item);
        //        }
        //    }
        //    foreach (var item in list2)
        //    {
        //        if (!list1.Where(c => c.NodeId == item.NodeId && c.RightId == item.RightId).Any())
        //        {
        //            surList2.Add(item);
        //        }
        //    }
        //    return Tuple.Create(surList1, surList2);
        //}

        
        /// <summary>
        /// 两个列表比较，获取第一个列表比第二个列表多的数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        private List<RoleRightItem> GetSurplusList(List<RoleRightItem> list1, List<RoleRightItem> list2)
        {
            var surList = new List<RoleRightItem>();
            if (list1 == null || !list1.Any()) return surList;
            if (list2 == null || !list2.Any()) return list1;

            foreach (var item in list1)
            {
                if (!list2.Where(c => c.NodeId == item.NodeId && c.RightId == item.RightId).Any())
                {
                    surList.Add(item);
                }
            }
            return surList;
        }



        public async Task<int> UpdateRights(string userName, IEnumerable<SysRoleRight> list)
        {
            if (string.IsNullOrEmpty(userName)) return 0;

            using (var conn = ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                int row = await conn.DeleteAsync<SysRoleRight>(new { userName }, tran);

                row += await conn.InsertListAsync(list, null, tran);

                tran.Commit();

                return row;
            }
        }

        #region Node


        //public IEnumerable<int> GetNodeIds(string controller, string id = null, string queryString = null)
        //{
        //    var nodes = this.GetNodeList(controller, id, queryString);

        //    return nodes.Select(c => c.Id );
        //}

        //public int GetNodeId(string area, string controller,string action = null, string id = null )
        //{
        //    using (var conn = ConnectionFactory.OpenConnection())
        //    {
        //        var list = conn.QueryList<SysAdminNode>(new { area, controller });

        //        if (!string.IsNullOrEmpty(action))
        //        {
        //            list = list.Where(c => action.Equals(c.Action, StringComparison.CurrentCultureIgnoreCase));
        //        }
        //        if (!string.IsNullOrEmpty(id))
        //        {
        //            list = list.Where(c => c.RouteId == id);
        //        }
        //        return list.Select(c=>c.Id).FirstOrDefault();
        //    }
        //}


        public int[] GetNodeIds(string area, string controller, string action = null, string id = null)
        {
            var list = GetNodeList(area, controller, action, id);

            return list.Select(c => c.Id).ToArray();
        }





        public IEnumerable<SysAdminNode> GetNodeList(string area, string controller, string action = null, string id = null)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
                var list = conn.QueryList<SysAdminNode>(new { area, controller });

                if (!string.IsNullOrEmpty(action))
                {
                    list = list.Where(c => action.IsEquals(c.Action));
                }

                if (!string.IsNullOrEmpty(id))
                {
                    list = list.Where(c => id.IsEquals(c.RouteId));
                }
                return list;
            }
        }

        #endregion

    }
}
