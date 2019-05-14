using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using TestCore.Common.Exceptions;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Entity;
using TestCore.Common.Helper;
using OKS.Framework;

namespace TestCore.Api.BaseControllers
{
    /// <summary>
    /// API基类
    /// </summary>
    public class ApiControllers : Controller
    {
        public ResponseResult ResponseResult = new ResponseResult { Result = 1, Message = string.Empty };

        private static readonly ILog logerror = LogManager.GetLogger(Startup.Repository.Name, "logerror");
        private static readonly ILog loginfo = LogManager.GetLogger(Startup.Repository.Name, "loginfo");

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId
        {
            get
            {
                if (User.Identity.Name != null)
                {
                    return Convert.ToInt32(User.Identity.Name);
                }
                else
                {
                    throw new UnauthorizedException("请登录");
                }
            }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public Users UserInfo
        {
            get
            {
                return GetCurUserInfo();
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        private Users GetCurUserInfo()
        {
            Users entity = new Users();

            string token = StringUtils.NotNullStr(CoreHttpContext.Current.Request.Headers["Authorization"]);
            loginfo.Info(string.Format("获取用户token信息---{0}", token));
            var use = Common.Cache.RedisConfig.GetValue(token);
            var res = JsonConvert.DeserializeObject<Users>(use);
            loginfo.Info(string.Format("获取用户信息---{0}", res));
            if (res != null)
            {
                entity.Id = res.Id;
                entity.Username = res.Username;
            }
            else
            {
                loginfo.Info("获取信息失败" + DateTime.Now);
            }
            return entity;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        protected Guid AddCurUserInfo(Users entity)
        {
            var guid = Guid.NewGuid();
            Common.Cache.RedisConfig.SetValue(guid.ToString(), JsonConvert.SerializeObject(entity), new TimeSpan(0, 2, 0));
            return guid;
        }


        #region 全局异常错误记录持久化 
        /// <summary>
        ///  全局异常错误记录持久化 
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void ErrorLog(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【抛出信息】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
            logerror.Error(errorMsg);
        }
        #endregion

        #region 自定义操作记录       
        /// <summary>
        /// 自定义操作记录
        /// </summary>
        /// <param name="throwMsg"></param>
        public static void WriteLog(string throwMsg)
        {
            string strMsg = string.Format("【普通日志信息】：{0} ", new object[] { throwMsg });
            loginfo.Info(strMsg);
        }
        #endregion

        #region HideEmail
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string HideEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return "";
            }
            var pre = email.Substring(0, email.IndexOf("@"));
            var suffix = email.Substring(email.IndexOf("@") - 1);
            return HideString(pre) + suffix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origStr"></param>
        /// <returns></returns>
        public static string HideString(string origStr)
        {
            var newStr = "";

            if (origStr.Length < 3)
            {
                newStr = "****";
            }
            else if (origStr.Length < 6)
            {
                newStr = string.Format("{0}****", origStr.Substring(0, 2));
            }
            else
            {
                newStr = string.Format("{0}****{1}", origStr.Substring(0, 2), origStr.Substring(origStr.Length - 2));
            }
            return newStr;
        }
        #endregion
    }
}
