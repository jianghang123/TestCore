using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TestCore.Admin.ViewModels;
using TestCore.Common;
using TestCore.Common.Captch;
using TestCore.Common.Encryption;
using TestCore.Common.Extensions;
using TestCore.Common.Helper;
using TestCore.Common.ResponseResult;
using TestCore.IService.SysAdmin;

namespace TestCore.Admin.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly IVerifyCode verifyCode;
        private readonly IWebHelper webHelper;
        private readonly IAdminSvc adminSvc;
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="workContext"></param>
        /// <param name="verifyCode"></param>
        /// <param name="webHelper"></param>
        /// <param name="messages"></param>
        /// <param name="adminSvc"></param>
        public LoginController(IVerifyCode verifyCode,IWebHelper webHelper,IAdminSvc adminSvc)
        {  
            this.verifyCode = verifyCode;
            this.webHelper = webHelper;
            this.adminSvc = adminSvc;  
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetImgCode()
        {
            byte[] captch = verifyCode.GetCaptch();
            return File(captch, MimeTypes.ImageGif);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckLogin(LoginModel loginModel)
        {
            var messages = new Messages();
            if (loginModel != null &&
                loginModel.UserName.IsNotNullOrEmpty() &&
                loginModel.UserPwd.IsNotNullOrEmpty() &&
                loginModel.UserCode.IsNotNullOrEmpty())
            {
                if (MD5Encrypt.MD5By16(loginModel.UserCode.ToLower()) != webHelper.GetStrSession(loginModel.UserCode.ToUpper()))
                {
                    messages.Msg = "验证码错误，请重新输入";
                }
                else
                {
                    var result = await adminSvc.Login(loginModel.UserName, loginModel.UserPwd);
                    if (result.Succeeded)
                    {
                        //记住登录凭证
                        var claims = new List<Claim>
                                    {
                                        //用户编号||用户名
                                        new Claim(ClaimTypes.Name, result.UserId.ToString()+"|||"+loginModel.UserName)
                                    };
                        ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        //添加身份认证
                        await HttpContext.SignInAsync(principal);
                        //缓存权限
                        //userCacheService.SetPermissionCache(result.UserId);
                    }
                    messages.Success = result.Succeeded;
                    messages.Msg = result.Msg;
                }
            }
            else
            {
                messages.Msg = "请填写完整";
            }
            return Json(messages);
        }
    }
}