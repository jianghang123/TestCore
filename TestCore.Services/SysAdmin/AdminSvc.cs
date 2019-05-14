using System;
using System.Threading.Tasks;
using TestCore.Common.Configuration;
using TestCore.Common.Encryption;
using TestCore.Common.Helper;
using TestCore.Domain.SysEntity;
using TestCore.IRepository;
using TestCore.IRepository.SysAdmin;
using TestCore.IService.SysAdmin;

namespace TestCore.Services.SysAdmin
{
    public class AdminSvc : BaseSvc<Admin>, IAdminSvc
   {
        private readonly IAdminRepository _adminRepository;
        private readonly SysManageSecurityConfig _config;
        private readonly IWebHelper _webHelper;
        private readonly IAdminLogsRepository _adminlogsRepository;

        public AdminSvc(IAdminRepository adminRepository, SysManageSecurityConfig config, IWebHelper webHelper, IAdminLogsRepository adminlogsRepository)
        {
            this._config = config;
            this._adminRepository = adminRepository;
            this._webHelper = webHelper;
            this._adminlogsRepository = adminlogsRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param> 
        /// <returns>Task<(bool Succeeded, string Msg)></returns>
        public async Task<(bool Succeeded, string Msg, int UserId)> Login(string userName, string userPwd)
        {
            bool succeeded = false;
            string msg = string.Empty;
            int userId = 0;
            var user = await _adminRepository.GetModelAsync(new { adminname = userName });
            if (user != null && user.Id > 0)
            {
                userId = user.Id;
                int errorTimes = 0;
                string pwd1 = MD5Encrypt.MD5(userPwd).ToUpper();
                if (user.Is_state == 1)
                {
                    //登录错误次数
                    int maxLoginFailedTimes = _config.MaxLoginFailedTimes;
                    if (maxLoginFailedTimes <= 0)
                    {
                        maxLoginFailedTimes = 5;
                    }
                    if (user.ErrorTimes < maxLoginFailedTimes)
                    {
                        if (user.Adminpass == pwd1)
                        {
                            succeeded = true;
                            msg = "登录系统，成功";
                        }
                        else
                        {
                            errorTimes = user.ErrorTimes + 1;
                            int sErrorTimes = maxLoginFailedTimes - errorTimes;
                            if (sErrorTimes > 0)
                            {
                                msg = "密码错误，您今天还可尝试" + sErrorTimes + "次";
                            }
                            else
                            {
                                msg = "您今天登录错误次数过多，今天不可再登录，欢迎明天回来";
                            }
                        }
                    }
                    else
                    {
                        errorTimes = user.ErrorTimes + 1;
                        msg = "您今天登录错误次数过多，今天不可再登录，欢迎明天回来";
                    }
                    //更新用户登录信息
                    await _adminRepository.UpdateAsync(new { errorTimes },new { id = user.Id });
                }
                else
                {
                    msg = "登录系统，该用户状态为禁止登录";
                }
            }
            else
            {
                msg = "用户名不存在";
            }
            //记录登录日志
            Adminlogs logs = new Adminlogs() {
                AdminId = userId.ToString(),
                Ip = _webHelper.GetCurrentIpAddress(),
                Addtime = DateTime.Now
            };
            var a = await _adminlogsRepository.InsertAsync(logs);
            return (succeeded, msg, userId);
        }

    }
}
