using Caiba.MvcUtils;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using TestCore.Common;
using TestCore.Common.Helper;
using TestCore.Domain.CommonEntity;

namespace TestCore.MvcUtils
{

    public class WebLogFilter : LogFilter
    {

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if(ProjectType != ProjectTypeEnum.Admin )
        //    {
        //        LogHelper.WriteLog(context, ProjectType);
        //    }
        //}


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var action = context.RouteData.Values["action"].ToString();
                var controller = context.RouteData.Values["controller"].ToString();

                if (action != "Maintain" || controller != "Home")
                {
                    if (WebConfig.AppSettings.IsMaintain == "1")
                    {
                        if (WebConfig.AppSettings.MaintainIp.IndexOf(CoreHttpContext.GetUserIP()) < 0)
                        {
                            context.HttpContext.Response.Redirect("/Home/Maintain");
                        }
                    }
                }


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
                //验证失败不写日志
                if (!context.ModelState.IsValid) return;

                ActionType actInfo = context.ActionDescriptor.GetActionAttribute<ActionType>();

                if (actInfo == null) return;

               LogHelper.WriteLog(context, this.ProjectType);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}