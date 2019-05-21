using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TestCore.Common.Exceptions;
using TestCore.Common.Helper;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Entity;

namespace TestCore.Admin.Controllers
{
    public class BaseController : Controller
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
                    return 1;
                    //return Convert.ToInt32(User.Identity.Name);
                }
                else
                {
                    return 1;
                    //throw new UnauthorizedException("请登录");
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

        public Task<Users> Users => GetClaiUserInfo();

        /// <summary>
        /// 获取用户信息 redis 方法
        /// </summary>
        /// <returns></returns>
        private Users GetCurUserInfo()
        {
            Users entity = new Users();
            string token = CoreHttpContext.Current.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
            {
                return entity;
            }
            loginfo.Info(string.Format("获取用户token信息---{0}", token));
            var use = "";//Common.Cache.RedisConfig.GetValue(token);
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

        private async Task<Users> GetClaiUserInfo()
        {
            Users model = new Users();
            int userId = 0;
            var auth = await new HttpContextAccessor().HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Succeeded)
            {
                string[] info = auth.Principal.Identity.Name.Split("|||");
                Int32.TryParse(info[0], out userId);
                model.Username = info[1];
            }
            model.Id = userId;
            return model;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        protected Guid AddCurUserInfo(Users entity)
        {
            var guid = Guid.NewGuid();
            // Common.Cache.RedisConfig.SetValue(guid.ToString(), JsonConvert.SerializeObject(entity), new TimeSpan(0, 2, 0));
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
    }
}