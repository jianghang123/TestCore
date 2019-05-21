using Microsoft.AspNetCore.Mvc.Filters;
using System;
using TestCore.Common.Helper;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;

namespace TestCore.MvcUtils
{

    public class LogFilter : ActionFilterAttribute
    {

        public ProjectTypeEnum ProjectType
        {
            get; set;
        } = ProjectTypeEnum.Admin;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                //没登录或者登录失败不写日志
                if (!context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.Items[Names.UserName] == null)
                {
                    return;
                }
                ///没有处理的异常，在异常处理里面写日志
                if (context.Exception != null && !context.ExceptionHandled)
                {
                    return;
                }
                if (!context.ModelState.IsValid) return;

                var method = context.HttpContext.Request.Method.ToUpper();

                var action = context.RouteData.Values["action"].ToString();

                ///logout 之外的 get请求全部不写日志
                if (method == "GET" && !action.IsEquals("logout"))
                    return;

                //LogHelper.WriteLog(context, ProjectType, LogTypeEnum.Normal);
            }
            catch (Exception ex)
            {
                //context.Result = new RedirectResult("/Account/Login");
                throw;
            }
        }

    }
}