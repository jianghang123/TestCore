using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Common.Encryption;
using TestCore.Common.Extensions;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;
using TestCore.Domain.SysEntity;
using TestCore.IRepository.SysAdmin;
using TestCore.MvcUtils;
using TestCore.MvcUtils.Admin;
using TestCore.Repositories;

namespace Caiba.MvcUtils
{
    public class AdminWebUtil
    {
        private static IRoleRightRepository roleRightRepository;

        public static IRoleRightRepository RoleRightRepository
        {
            get
            {
                if (roleRightRepository == null)
                {
                    roleRightRepository = IoCBootstrapper.AutoContainer.Resolve<IRoleRightRepository>();
                }
                return roleRightRepository;
            }
        }

        /// <summary>
        /// 获取action 配置的栏目 Id，权限等配置信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ActionRight GetActionInfo(ActionContext context)
        {
            ActionRight actInfo = context.ActionDescriptor.GetActionAttribute<ActionRight>();
            if (actInfo == null)
            {
                actInfo = new ActionRight();
            }
            if (actInfo.RightType == RightTypeEnum.none)
            {
                var action = context.RouteData.Values["action"].ToString();

                actInfo.RightType = ActionHelper.GetRightType(action);
            }
            if (actInfo.NodeId == 0)
            {
                actInfo.NodeId = GetNodeId(context);
            }
            return actInfo;
        }


        public static async Task<ActionRight> GetActionInfoAsync(ActionContext context)
        {
            return await Task.Run(() =>
            {
                return GetActionInfo(context);
            });
        }

        public static int GetNodeId(ActionContext context)
        {
            var nodeId = context.HttpContext.Request.GetIntParam("node_id");

            var nodeIds = GetNodeIds(context.RouteData);

            if (nodeId != null && nodeId > 0)
            {
                if (!nodeIds.Contains(nodeId.Value)) return 0;

                return nodeId.Value;
            }
            return nodeIds.FirstOrDefault();
        }


        public static int[] GetNodeIds(RouteData routeData)
        {
            var area = routeData.Values["area"] + "";

            var controller = routeData.Values["controller"] + "";

            var action = routeData.Values["action"] + "";

            string id = null;

            if (action.IsEquals("index"))
            {
                id = routeData.Values["id"] + "";
            }
            else
            {
                action = null;
            }
            var nodeIds = RoleRightRepository.GetNodeIds(area, controller, action, id);

            return nodeIds;
        }

        #region Session

        /// <summary>
        /// 当前的用户是否超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool UserIsSupper()
        {
            if (UserRight != null) return UserRight.IsSupper;

            return false;
        }

        /// <summary>
        /// 用户是否代理
        /// </summary>
        /// <returns></returns>
        public static bool UserIsAgent()
        {
            if (UserRight == null) return false;

            return RoleGroup == RoleGroupEnum.Agent;
        }

        /// <summary>
        /// 角色组
        /// </summary>
        public static RoleGroupEnum RoleGroup
        {
            get
            {
                return (RoleGroupEnum)UserRight.GroupId;
            }
        }

        /// <summary>
        /// 当前用戶权限
        /// </summary>
        public static UserRight UserRight
        {
            get
            {
                try
                {
                    var roleRight = CoreHttpContext.Current.Session.GetObjectFromJson<UserRight>(Names.UserRight);
                    if (roleRight == null)
                    {
                        roleRight = RoleRightRepository.GetUserRightAsync(CoreHttpContext.Current.User.Identity.Name).Result;
                        CoreHttpContext.Current.Session.SetObjectAsJson(Names.UserRight, roleRight);
                    }
                    return roleRight;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        #region Sites

        public static int[] GetSiteIds()
        {
            if (UserRight.GroupId == (int)RoleGroupEnum.System)
            {
                return RoleRightRepository.GetSingleList<int, SysWebConfig>(nameof(SysWebConfig.Id), new { Status = 1 }, "SortIndex").ToArray();
            }
            else
            {
                return new int[] { UserRight.SiteId };
                //return UserRight.SiteIdList;
            }
        }

        public static IEnumerable<SelectListItem> GetSiteSelectList()
        {
            if (UserRight.GroupId == (int)RoleGroupEnum.System)
            {
                return ResourceHelper.GetSelectList(TableEnum.SysWebConfig, nameof(SysWebConfig.SiteTitle));
            }
            else
            {
                if (!UserRight.SiteIdList.Any())
                    return ResourceHelper.GetSelectList(TableEnum.SysWebConfig, nameof(SysWebConfig.SiteTitle), new int[] { UserRight.SiteId });
                //return new List<SelectListItem>();    
            }
            return ResourceHelper.GetSelectList(TableEnum.SysWebConfig, nameof(SysWebConfig.SiteTitle), UserRight.SiteIdList);
        }

        /// <summary>
        /// 从缓存获取当前用户的站点的数据字典
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSiteSelectListFromCache()
        {
            if (UserRight.GroupId == (int)RoleGroupEnum.System)
            {
                return ResourceHelper.GetSelectListFromCache((int)TableEnum.SysWebConfig, nameof(SysWebConfig.SiteTitle));
            }
            else
            {
                if (!UserRight.SiteIdList.Any())
                    return ResourceHelper.GetSelectListFromCache((int)TableEnum.SysWebConfig, nameof(SysWebConfig.SiteTitle), new int[] { UserRight.SiteId });
                //return new List<SelectListItem>();
            }
            return ResourceHelper.GetSelectListFromCache((int)TableEnum.SysWebConfig, nameof(SysWebConfig.SiteTitle), UserRight.SiteIdList);
        }

        #endregion



        #region Operator


        public static int[] GetGroupIds()
        {
            return EnumHelper.GetValues(typeof(RoleGroupEnum))
                   .Where(c => c >= UserRight.GroupId)
                   .ToArray();
        }

        public static int[] GetOperIds()
        {
            if (RoleGroup == RoleGroupEnum.System)
            {
                return RoleRightRepository.GetSingleList<int, SysOperator>(nameof(SysOperator.Id), new { Status = 1 }, "SortIndex").ToArray();
            }
            else if (RoleGroup == RoleGroupEnum.Operator)
            {
                return new int[] { UserRight.OperId };
            }
            else
            {
                return new int[0];
            }
        }

        /// <summary>
        /// 获取当前用户可以管理的角色
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetRoleSelectList()
        {
            if (RoleGroup == RoleGroupEnum.System)
            {
                return ResourceHelper.GetSelectList(TableEnum.SysRole, nameof(SysOperator.Name));
            }
            else
            {
                var groupIds = GetGroupIds();

                if (!groupIds.Any()) return new List<SelectListItem>();

                var list = RoleRightRepository.GetSelectListAsync<SysRole>(nameof(SysRole.Id), nameof(SysRole.Name),
                    new { GroupId = groupIds, status = 1 }, "SortIndex,Id").Result;

                return list;
            }

        }


        public static IEnumerable<SelectListItem> GetOperSelectList()
        {
            if (RoleGroup == RoleGroupEnum.System)
            {
                return ResourceHelper.GetSelectList(TableEnum.SysOperator, nameof(SysOperator.Name));
            }
            else
            {
                if (UserRight.OperId <= 0) return new List<SelectListItem>();
            }
            return ResourceHelper.GetSelectList(TableEnum.SysOperator, nameof(SysOperator.Name), new int[] { UserRight.OperId });
        }

        /// <summary>
        /// 从缓存获取当前用户的站点的数据字典
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetOperSelectListFromCache()
        {
            if (RoleGroup == RoleGroupEnum.System)
            {
                return ResourceHelper.GetSelectListFromCache(TableEnum.SysOperator, nameof(SysOperator.Name));
            }
            else
            {
                if (UserRight.OperId <= 0) return new List<SelectListItem>();
            }
            return ResourceHelper.GetSelectListFromCache(TableEnum.SysOperator, nameof(SysOperator.Name), new int[] { UserRight.OperId });
        }

        #endregion

        #region Provider

        #endregion

        /// <summary>
        /// Md5 加密密码,如果没有参数使用配置的默认密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string Md5(string pwd = null)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                pwd = AdminConfig.AppSettings.DefaultPwd;
            }
            return EncryptionUtils.Md5(pwd + AdminConfig.AppSettings.LoginPwdSecretKey);
        }

        #endregion

        private static List<SelectListItem> rightSelectList;

        /// <summary>
        /// 系统的权限
        /// </summary>
        public static List<SelectListItem> RightSelectList
        {
            get
            {
                if (rightSelectList == null)
                {
                    rightSelectList = EnumHelper.GetSelectListFromCache<RightTypeEnum>(false).Where(c => c.Value != "0").ToList();
                }
                return rightSelectList;
            }
        }


        public static IActionResult GetNoRightResult(HttpRequest request)
        {
            if (request.IsAjaxRequest())
            {
                return new JsonResult(new ResponseResult { Result = 0, Message = "没有权限！" });
            }
            else
            {
                return new RedirectResult("Account/NoRight");
            }
        }



        ///// <summary>
        ///// 获取活动：系统商获取全部、其他根据站点获取
        ///// <param name="siteId">用户站点</param>
        ///// </summary>
        ///// <returns></returns>
        //public static List<SelectListItem> GetActivitySelectList(int? siteId = null)
        //{

        //    var dateTime = DateTime.Now;

        //    var condition = new Condition();

        //    condition.LessThanEquals(new { BeginTime = dateTime });
        //    condition.GreaterThanEquals(new { EndTime = dateTime });
        //    condition.And(new { Status = 1 });

        //    if (siteId != null)
        //    {
        //        condition.And(new { SiteId = siteId.Value });
        //        goto query;
        //    }

        //    //系统商，当前开启的活动
        //    if (UserRight.GroupId == (int)RoleGroupEnum.System)
        //    {
        //    }
        //    else
        //    {
        //        //根据用户站点来查询，当前开启的活动
        //        condition.And(new { SiteId = UserRight.SiteId });
        //    }

        //    query:
        //    var models = roleRightRepository.GetList<SysActvityConfig>(condition);

        //    if (models.Any())
        //    {
        //        return models.Select(c => new SelectListItem
        //        {
        //            Text = c.ActivityName,
        //            Value = c.Id.ToString()
        //        }).ToList();
        //    }
        //    else
        //    {
        //        return new List<SelectListItem>();
        //    }

        //}

        ///// <summary>
        ///// 获取活动：系统商获取全部、其他根据站点获取
        ///// <param name="siteId">用户站点</param>
        ///// </summary>
        ///// <returns></returns>
        //public static List<SysActvityConfig> GetActivities(int? siteId = null)
        //{

        //    var dateTime = DateTime.Now;

        //    var condition = new Condition();

        //    condition.LessThanEquals(new { BeginTime = dateTime });
        //    condition.GreaterThanEquals(new { EndTime = dateTime });
        //    condition.And(new { Status = (int)StatusEnum.Enable });

        //    if (siteId != null)
        //    {
        //        condition.And(new { SiteId = siteId.Value });
        //        goto query;
        //    }

        //    //系统商，当前开启的活动
        //    if (UserRight.GroupId == (int)RoleGroupEnum.System)
        //    {
        //    }
        //    else
        //    {
        //        //根据用户站点来查询，当前开启的活动
        //        condition.And(new { SiteId = UserRight.SiteId });
        //    }

        //    query:
        //    var models = roleRightRepository.GetList<SysActvityConfig>(condition);

        //    if (models.Any())
        //    {
        //        return models.ToList();
        //    }
        //    else
        //    {
        //        return new List<SysActvityConfig>();
        //    }

        //}

        ///// <summary>
        ///// 获取游戏列表： 系统商全部、其他根据站点查询
        ///// <param name="siteId">用户站点</param>
        ///// </summary>
        ///// <returns></returns>
        //public static List<SelectListItem> GetGamesSelectList(int? siteId = null)
        //{

        //    var condition = new Condition();

        //    if (siteId != null)
        //    {
        //        condition.And(new { SiteId = siteId });
        //        goto query;
        //    }

        //    //系统商，全部站点游戏
        //    if (UserRight.GroupId == (int)RoleGroupEnum.System)
        //    {

        //    }
        //    else
        //    {
        //        //根据用户站点，查询游戏
        //        condition.And(new { SiteId = UserRight.SiteId });
        //    }

        //    query:
        //    var gameIds = roleRightRepository.GetSingleList<int, SiteGameConfig>(nameof(SiteGameConfig.GameId), condition).ToList();

        //    var list = roleRightRepository.GetList<GameInfo>(new { Id = gameIds.ToArray(), Status = 1 }, "SortIndex");

        //    list = list.ChangeLang<GameInfo>(nameof(GameInfo.Name), TableEnum.GameInfo);

        //    if (list.Any())
        //    {
        //        return list.Select(c => new SelectListItem
        //        {
        //            Text = c.Name,
        //            Value = c.Id.ToString()
        //        }).ToList();
        //    }
        //    else
        //    {
        //        return new List<SelectListItem>();
        //    }
        //}


        ///// <summary>
        ///// 获取游戏列表
        ///// <param name="isProvider">是否在GameInfo内查询</param>
        ///// <param name="isBaseAll">是否在GameInfo内查询</param>
        ///// <param name="isContainWallet">是否包含中心钱包</param>
        ///// <param name="siteId">用户站点，系统商全部站点、其他根据站点查询</param>
        ///// </summary>
        ///// <returns></returns>
        //public static List<SelectListItem> GetGameInfoSelectList(bool isProvider, bool isBaseAll = false, bool isContainWallet = false, int? siteId = null)
        //{
        //    var res = new List<SelectListItem>();

        //    var condition = new Condition();

        //    IEnumerable<GameInfo> games;

        //    if (isBaseAll)
        //    {
        //        condition.And(new { IsProvider = isProvider });
        //        games = roleRightRepository.GetList<GameInfo>(condition).ToList();
        //    }
        //    else
        //    {
        //        if (siteId != null)
        //        {
        //            condition.And(new { SiteId = siteId });
        //        }
        //        else
        //        {
        //            //系统商，全部站点游戏
        //            if (UserRight.GroupId == (int)RoleGroupEnum.System)
        //            {

        //            }
        //            else
        //            {
        //                //根据用户站点，查询游戏
        //                condition.And(new { SiteId = UserRight.SiteId });
        //            }
        //        }

        //        var gameIds = roleRightRepository.GetSingleList<int, ViewSiteGameConfig>(nameof(ViewSiteGameConfig.GameId), condition).Distinct().ToList();
        //        var providerIds = roleRightRepository.GetSingleList<int, ViewSiteGameConfig>(nameof(ViewSiteGameConfig.ProviderId), condition).Distinct().ToList();

        //        if (isContainWallet)
        //        {
        //            gameIds.Add(0);
        //            providerIds.Add(0);
        //        }
        //        if (isProvider)
        //        {
        //            games = roleRightRepository.GetList<GameInfo>(new { Id = providerIds.ToArray() }).ToList();
        //        }
        //        else
        //        {
        //            games = roleRightRepository.GetList<GameInfo>(new { Id = gameIds.ToArray() }).ToList();
        //        }

        //        games = games.ChangeLang<GameInfo>(nameof(GameInfo.Name), TableEnum.GameInfo);
        //    }
        //    return games.Select(c => new SelectListItem
        //    {
        //        Text = c.Name,
        //        Value = c.Id.ToString()
        //    }).ToList();

        //}

        ///// <summary>
        ///// 获取游戏商（钱包）
        ///// </summary>
        ///// <param name="isBaseAll"></param>
        ///// <param name="siteId"></param>
        ///// <returns></returns>
        //public static List<SelectListItem> GetProvidersSelectList(bool isBaseAll=false,int? siteId=null)
        //{
        //    return GetGameInfoSelectList(true, isBaseAll, true, siteId);
        //}

        ///// <summary>
        ///// 获取游戏列表(不包含游戏钱包)
        ///// </summary>
        ///// <param name="isBaseAll"></param>
        ///// <param name="siteId"></param>
        ///// <returns></returns>
        //public static List<SelectListItem> GetGameSelectList(bool isBaseAll=false,int? siteId=null)
        //{
        //    return GetGameInfoSelectList(false, isBaseAll, false, siteId);
        //}

        ///// <summary>
        ///// 游戏转账类型
        ///// </summary>
        ///// <returns></returns>
        //public static List<int> GetGameTradeFlowType()
        //{
        //    return new List<int>
        //    {
        //        (int)FundFlowTypeEnum.GameToGame, (int)FundFlowTypeEnum.GameToWallet, (int)FundFlowTypeEnum.WalletToGame,
        //        (int)FundFlowTypeEnum.OperToGame,(int)FundFlowTypeEnum.GameToOper,(int)FundFlowTypeEnum.BackToWallet,
        //        (int)FundFlowTypeEnum.ManualDiscountToGame,(int)FundFlowTypeEnum.ManualDiscountToOper
        //    };
        //}

        //public static List<int> GetManualFlowType()
        //{
        //    return new List<int>
        //    {
        //        (int)FundFlowTypeEnum.ManualDiscount,(int)FundFlowTypeEnum.ManualDiscountBack,
        //        (int)FundFlowTypeEnum.ManualPayment,(int)FundFlowTypeEnum.ManualDraw
        //    };
        //}


        ///// <summary>
        ///// 获取其他的红利类型
        ///// </summary>
        ///// <returns></returns>
        //public static int[] GetOtherBonusFlowType()
        //{
        //    return new int[]
        //    {
        //        (int)FundFlowTypeEnum.ManualDiscount,(int)FundFlowTypeEnum.ManualDiscountBack,
        //        (int)FundFlowTypeEnum.ManualDiscountToGame,(int)FundFlowTypeEnum.ManualDiscountToOper
        //    };
        //}

        //public static IEnumerable<SelectListItem> GetGameTradeTypeSelectList()
        //{
        //    var flowType = EnumHelper.GetSelectList<FundFlowTypeEnum>();
        //    var gameFlows = GetGameTradeFlowType();


        //    return flowType.Where(c => gameFlows.Contains(int.Parse(c.Value)));
        //}
    }
}