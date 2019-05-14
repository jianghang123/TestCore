using Caiba.MvcUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;

namespace TestCore.MvcUtils
{
    public class HandleException : ExceptionFilterAttribute
    {
        public ProjectTypeEnum ProjectType { get; set; } = ProjectTypeEnum.Admin;

        public override void OnException(ExceptionContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (context.ExceptionHandled) return;

                LogHelper.WriteLog(context, ProjectType, LogTypeEnum.Exception);

                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    ///context.HttpContext.Response.StatusCode = 500;
                    context.Result = new JsonResult(new ResponseResult { Result = 0, Message = context.Exception.Message });
                }
                else
                {
                    //否则调用原始设置
                    base.OnException(context);
                }
            }
            catch (Exception ex)
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    //context.HttpContext.Response.StatusCode = 500;
                    context.Result = new JsonResult(new ResponseResult { Result = 0, Message = ex.Message });
                }
                else
                {
                    base.OnException(context);
                }
            }
        }

    }
}

