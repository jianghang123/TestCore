using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Entity;
using TestCore.Domain.InputEntity;

namespace TestCore.IService.User
{
    public interface IUsersSvc : IBaseSvc<Users>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="logInInput"></param>
        /// <returns></returns>
        OperationResult GetLoginResult(LogInInput logInInput);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        OperationResult GetRegisterResult(RegisterInput input);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<Users> GetUserInfo(int userid);

        /// <summary>
        /// 更改用户分成
        /// </summary>
        /// <returns></returns>
        OperationResult InsertUpriceList(List<Userprice> userprices, int userId);

        /// <summary>
        /// 重置用户分成
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        OperationResult ResetUprice(int userId);
    }
}
