using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCore.Api.BaseControllers;
using TestCore.Domain.Entity;
using TestCore.Domain.InputEntity;
using TestCore.IService.Common;
using TestCore.IService.User;

namespace TestCore.Api.Controllers
{
    /// <summary>
    /// 公共类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ApiControllers
    {
        private readonly IUsersSvc _usersSvc;
        private readonly ITokenSvc _tokenSvc;


        public ValuesController(IUsersSvc usersSvc, ITokenSvc tokenSvc)
        {
            _usersSvc = usersSvc;
            _tokenSvc = tokenSvc;
        }

        #region LogIn  登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="logInInput"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetToken")]
        [ProducesResponseType(typeof(TokenEntity), 200)]
        public IActionResult LogIn([FromBody]LogInInput logInInput)
        {
            var res = _usersSvc.GetLoginResult(logInInput);
            if (res.IsSuccess)
            {
                //redis 存储
                //string data = JsonConvert.SerializeObject(res.Data);
                //Guid guid = AddCurUserInfo(JsonConvert.DeserializeObject<Domain.entity.Users>(data));
                //Dictionary<string, string> dicRes = new Dictionary<string, string>
                //{
                //    { "userToken", guid.ToString() }
                //};

                //ClaimsIdentity 授权
                var authResult = _tokenSvc.CreateToken(res.Data as Users);
                ResponseResult.Result = 1;
                ResponseResult.Data = authResult;
            }
            else
            {
                ResponseResult.Result = 0;
                ResponseResult.Message = res.Message;
            }

            return Ok(ResponseResult);
        }

        #endregion

        #region GetMemberInfo 获取用户信息

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var memberInfo = await _usersSvc.GetUserInfo(this.UserId);
            if (memberInfo == null)
            {
                ResponseResult.Result = 0;
                ResponseResult.Message = "用户信息获取失败！";
                return BadRequest(ResponseResult);
            }
            ResponseResult.Result = 1;
            ResponseResult.Message = "数据获取成功！";
            ResponseResult.Data = memberInfo;
            return Ok(ResponseResult);
        }

        #endregion

    }
}
