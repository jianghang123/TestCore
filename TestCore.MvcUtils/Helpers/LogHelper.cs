using Autofac;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using TestCore.Common;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;
using TestCore.Common.Logging;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;
using TestCore.Domain.SysEntity;
using TestCore.IRepository.SysAdmin;

namespace TestCore.MvcUtils
{
    public class LogHelper
    {
        private static IUserLogRepository UserLogRepository = IoCBootstrapper.AutoContainer.Resolve<IUserLogRepository>();

        //private static IMemActionLogRepository MemberLogRepository = IoCBootstrapper.AutoContainer.Resolve<IMemActionLogRepository>();

        private static ILog log = LoggingUtils.GetLogger(typeof(LogHelper));
        
        private static string path;
        private static ActionTypeEnum actType;
        private static int nodeId ;
        private static int gameId = 0;
        private static int result = 1;
        //private static LogTypeEnum logType = LogTypeEnum.Normal;

        private static ClientTypeEnum clientType = ClientTypeEnum.Web;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void WriteLog(FilterContext context, ProjectTypeEnum projectType, LogTypeEnum logType = LogTypeEnum.Normal)
        {
            try
            {
                ActionType actInfo = context.ActionDescriptor.GetActionAttribute<ActionType>();

                string action = context.RouteData.Values["action"] + "";

                if (actInfo != null)
                {
                    actType = actInfo.ActType;
                }
                else
                {
                    actType = ActionHelper.GetActionType(action);
                }
                result = 1;
                path = context.HttpContext.Request.Path.Value;

                if (actType == ActionTypeEnum.None  && logType == LogTypeEnum.Normal)
                {
                    throw new Exception(string.Format("请在控制器({0})的方法({1})上指定操作类型!",
                        context.RouteData.Values["controller"], action));
                }
                var userName = context.HttpContext.User.Identity.Name;

                if(string.IsNullOrEmpty(userName))
                {
                    userName = (string)context.HttpContext.Items[Names.UserName];
                    //userName = context.HttpContext.Request.GetHeaderValue(Names.UserName);
                }

                if (string.IsNullOrEmpty(userName))
                    return;


                result = GetResult(context);

                ///自定义日志内容
                string custLogText = null;

                if(logType == LogTypeEnum.Normal)
                {
                    custLogText = (string)context.HttpContext.Items[Names.LogContent];
                }
                else
                {
                    custLogText = GetExceptionMessage(context);
                }
                var logText = GetLogText(custLogText);

                string fileLogContent = string.Format("Result:{0}; LogType:{1};{2};",  result, logType.ToString(), logText);

                if (projectType == ProjectTypeEnum.Admin)
                {
                    nodeId = TypeHelper.TryParse(context.HttpContext.Items[Names.NodeId] + "", 0);
                }
                else
                {
                    gameId = TypeHelper.TryParse(context.HttpContext.Items[Names.GameId] + "", 0);
                    fileLogContent  = string.Format("GameId:{0};", gameId) + fileLogContent;
                } 
                string ip = CoreHttpContext.GetUserIP();

                WriteFileLog(userName, fileLogContent, ip);
         
                if (projectType == ProjectTypeEnum.Admin)
                {
                    WriteUserDbLog(userName, logText, actType, logType, nodeId, context,ip);
                }
                else
                {
                    //WriteMemDbLog( userName,  logText, actType, logType, gameId, context, projectType,ip);
                } 
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string GetExceptionMessage(FilterContext context)
        {
            try
            {
                Exception exception = null;
                if (context is ExceptionContext)
                {
                    exception =((ExceptionContext)context).Exception;
                }
                else
                {
                    if (context is ActionExecutedContext)
                    {
                        exception = ((ActionExecutedContext)context).Exception;
                    }
                    else if (context is ActionExecutingContext)
                    {
                        exception = ((ActionExecutedContext)context).Exception;
                    }
                }
                if(exception != null)
                {
                    return exception.Message;
                }
                return null;
            }catch
            {
                return   null;
            }
        }

        private static int GetResult(FilterContext context)
        {
            BaseController baseController = null;

            int actionResult = 0;

            if (context is ActionExecutedContext)
            {
                baseController = (((ActionExecutedContext)context).Controller) as BaseController;
            }
            else if (context is ActionExecutingContext)
            {
                baseController = (((ActionExecutingContext)context).Controller) as BaseController;
            }
            if (baseController != null)
            {
                actionResult = baseController.ResponseResult.Result;
            }
            return actionResult;
        }

        private static string GetLogText(string custLogText )
        {
            if(string.IsNullOrWhiteSpace(custLogText))
            {
                return string.Format("{0}:{1}", actType.ToString(), path);
            }
            return string.Format("{0}:{1};{2}", actType.ToString(), path, custLogText);
        }


        /// <summary>
        /// 写文件日志
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="actionDesc"></param>
        public static void WriteFileLog( string userName, string actionDesc, string ip = null)
        {
            if(string.IsNullOrEmpty(ip))
            {
                ip = CoreHttpContext.GetIP();
            }
            log.InfoFormat("{0} -> IP:{1};{2}", userName, ip, actionDesc);
        }

        /// <summary>
        /// 写用户行为到数据库日志
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="logText"></param>
        private static void WriteUserDbLog(string userName,  string logText, ActionTypeEnum actType, LogTypeEnum logType, int nodeId, FilterContext context,string ip = null )
        {
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    ip = CoreHttpContext.GetIP();
                }
                //判断是否为手机
                if (context.HttpContext.Request.IsMobileBrowser())
                {
                    clientType = ClientTypeEnum.Wap;
                }
                var log = new SysUserLog
                {
                    ActionType = (int)actType,
                    ActionDesc = logText,
                    LogType = (int)logType,
                    Result =  result,
                    NodeId = nodeId,
                    ClientType = (int)clientType,  ///暂时默认
                    UserName = userName,
                    Ip = ip,
                };
                WriteDbLog(log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        ///// <summary>
        ///// 会员的操作日志
        ///// </summary>
        ///// <param name="memId"></param>
        ///// <param name="logText"></param>
        ///// <param name="actType"></param>
        ///// <param name="gameId"></param>
        ///// <param name="ip"></param>
        //private static void WriteMemDbLog(string memId, string logText, ActionTypeEnum actType, LogTypeEnum logType, int gameId, FilterContext context,ProjectTypeEnum projectType, string ip = null)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(ip))
        //        {
        //            ip = CoreHttpContext.GetIP();
        //        }
        //        ////判断是否为手机
        //        //if (context.HttpContext.Request.IsMobileBrowser())
        //        //{
        //        //    clientType = ClientTypeEnum.Wap;
        //        //}
        //        var log = new MemActionLog
        //        {
        //            ActionType = (int)actType,
        //            ActionDesc = logText,
        //            LogType = (int)logType,
        //            Result = result,
        //            GameId = gameId,
        //            ClientType = (int)projectType,  ///暂时默认
        //            SiteId = WebConfig.AppSettings.SiteId,  /// 待测试
        //            MemId = memId,
        //            Ip = ip,
        //        };
        //        WriteDbLog(log);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}



        /// <summary>
        /// 写用户的操作日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="storeType"></param>
        public static void WriteDbLog(SysUserLog log)
        {
            try
            {
                if(string.IsNullOrEmpty(log.UserName))
                {
                    log.UserName = CoreHttpContext.Current.User.Identity.Name;
                }
                if (string.IsNullOrEmpty(log.UserName))
                {
                    throw new Exception("登录已经超时，请重新登录");
                }

                if (string.IsNullOrEmpty(log.Ip))
                {
                    log.Ip = CoreHttpContext.GetIP();
                }

                log.LogTime = DateTime.Now;
                log.LogTimeEst = DateTime.UtcNow.ConvertToEstTimeSummer();
                int row = UserLogRepository.Insert(log);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        ///// <summary>
        ///// 写会员的操作日志
        ///// </summary>
        ///// <param name="log"></param>
        //public static void WriteDbLog(MemActionLog log)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(log.MemId))
        //        {
        //            log.MemId = CoreHttpContext.Current.User.Identity.Name;
        //        }
        //        if (string.IsNullOrEmpty(log.MemId))
        //        {
        //            throw new Exception("登录已经超时，请重新登录");
        //        }
        //        if (string.IsNullOrEmpty(log.Ip))
        //        {
        //            log.Ip = CoreHttpContext.GetIP();
        //        }
        //        log.LogTime = DateTime.Now;
        //        log.LogTimeEst = DateTime.UtcNow.ConvertToEstTimeSummer();

        //        int row = MemberLogRepository.Insert(log);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

    }
}
