
using Caiba.MvcUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using TestCore.Common.Helper;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;

namespace TestCore.MvcUtils
{

    /// 如果不需要验证用户是否登录就可以浏览，则给MVC中的控制器类名或者Action方法上面打上[AllowAnonymous]标签
    /// 如果只需要登录，不检查权限 ,给控制器类名或者Action方法上面打上[RightInfo(NoAudit =true)]标签
    //[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class AdminAuthorize :  AuthorizeAttribute, IAuthorizationFilter 
    {
        public AdminAuthorize( )
        {
        }
        public ActionRight RightInfo { get; set; }

        private string controller;

        private string action;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Account/Login");
                return;
            }
            #region  set NodeId 

            var rightInfo = AdminWebUtil.GetActionInfo(context);

            int nodeId = rightInfo.NodeId;

            if(nodeId > 0 && !context.HttpContext.Request.IsAjaxRequest())
            {
                context.HttpContext.Request.SetHeaderValue(Names.NodeId, nodeId.ToString());
            }
            #endregion

            ///不需要验证
            if (rightInfo.NoAudit) return;

            action = (context.RouteData.Values["action"] + "").ToLower();
            string method = context.HttpContext.Request.Method.ToUpper();

            if (method == "GET" ) 
            {    ///暂不检查 ajax get 请求的权限
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    return;
                }
                var actions = new string[] { "index", "details","edit"};
                ///暂不检查 index,details,edit 以外的action  get请求权限
                if (!actions.IsContains(action)) 
                {
                    return;
                }
            }
            var roleRight = AdminWebUtil.UserRight;
            if (roleRight == null)
            { 
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.Result = new JsonResult(new ResponseResult { Result = 0, Message="会话已经结束！" });
                    
                }
                else
                {
                    context.Result = new RedirectResult("/Account/Login");
                }
                return;
            }

            if (roleRight.IsSupper) return;

            controller = (context.RouteData.Values["controller"] + "").ToLower();

            if (rightInfo.RightType == RightTypeEnum.none)
            {
                throw new Exception(string.Format("请在控制器({0})的方法({1})上指定权限类型!", controller, action));
            }
            var hasRight = roleRight.HasRight((int)rightInfo.RightType, rightInfo.NodeId);

            if (!hasRight)
            {
                context.Result = AdminWebUtil.GetNoRightResult(context.HttpContext.Request);
            }
        }

        public bool MemberAuthorization(AuthorizationFilterContext context)
        {
            string id = context.RouteData.Values["id"] + "";

            var ids = id.Split(',');

            //如果是代理需要判断权限
            if(AdminWebUtil.RoleGroup == RoleGroupEnum.Agent )
            {
                ///待完善
            }

            return true;
        }

    }

}
