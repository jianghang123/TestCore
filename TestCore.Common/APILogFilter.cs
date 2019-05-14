using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
namespace TestCore.Common
{
    /// <summary>
    /// 日志记录  
    /// </summary>
    public class APILogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                //没授权不记录日志
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    return;
                }
                ///没有处理的异常，在异常处理里面写日志
                if (context.Exception != null && !context.ExceptionHandled)
                {
                    return;
                }

                //ActionType actInfo = context.ActionDescriptor.GetActionAttribute<ActionType>();

                //if (actInfo == null) return;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
