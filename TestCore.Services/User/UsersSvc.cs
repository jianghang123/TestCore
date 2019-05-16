using System.Collections.Generic;
using System.Threading.Tasks;
using TestCore.Common.Cache;
using TestCore.Common.Encryption;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Entity;
using TestCore.Domain.InputEntity;
using TestCore.IRepository.User;
using TestCore.IService.User;
using TestCore.Repository;

namespace TestCore.Services.User
{
    public class UsersSvc : BaseSvc<Users>, IUsersSvc
    {
        private readonly IUserRepository _userRepository;

        public UsersSvc(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OperationResult GetLoginResult(LogInInput input)
        {

            bool isSuccess;
            Users user;
            MobilePasswordValid(input, out isSuccess, out user);

            if (!isSuccess)
            {
                return new OperationResult { IsSuccess = false, Message = "登录验证失败" };
            }
            else
            {
                return MemberValid(user);
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OperationResult GetRegisterResult(RegisterInput input)
        {
            Users user;
            string code = input.ImgCode.ToUpper();
            if (input.ImgCode.ToLower() != CacheUtils.CacheService.Get<string>(code))
            {
                return new OperationResult { IsSuccess = false, Message = "图形验证码不正确！" };
            }

            if (input.Userpass != input.ConfirmUserpass)
            {
                return new OperationResult { IsSuccess = false, Message = "两次输入密码不一致！" };
            }

            ////获取手机验证码
            //var smsLogInfo = _smsSvc.GetSmsLogInfo(input.Mobile, SMSCodeTypeEnum.Register);
            ////手机验证码
            //if (input.SMSCode != smsLogInfo.VerifyCode)
            //{
            //    return new OperationResult { IsSuccess = false, Message = "手机验证码不正确！" };
            //}

            //是否已注册
            //user = await _userRepository.GetModelAsync(new { input.UserName });

            //if (user != null)
            //{
            //    return new OperationResult { IsSuccess = false, Message = "用户已存在！" };
            //}

            //数据入库
            //if (!RegsiterMember(input))
            //{
            //    CacheUtils.CacheService.Remove(code);
            //    return new OperationResult { IsSuccess = false, Message = "注册失败！" };
            //}
            //else
            //{
            //    CacheUtils.CacheService.Remove(code);
            //}

            return new OperationResult { IsSuccess = true, Message = "注册成功！" };
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<Users> GetUserInfo(int userid)
        {
            return await _userRepository.GetModelAsync<Users>(new { Id = userid });
        }

        /// <summary>
        /// 账号密码验证
        /// </summary>
        /// <param name="input"></param>
        /// <param name="member"></param>
        private void MobilePasswordValid(LogInInput input, out bool isSuccess, out Users user)
        {
            user = _userRepository.GetModel(new { input.UserName });
            if (user == null || !CheckPwd(input.Password, user.Userpass))
            {
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
            }
        }

        /// <summary>
        /// 密码校验
        /// </summary>
        /// <param name="originalPwd">输入的明文</param>
        /// <param name="encryptPwd">存储的密文</param>
        /// <returns></returns>
        public bool CheckPwd(string originalPwd, string encryptPwd)
        {
            return EncryPwd(originalPwd) == encryptPwd;
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="type">1-钱包</param>
        /// <returns></returns>
        public string EncryPwd(string pwd, string type = null)
        {
            return EncryptionUtils.Md5(pwd);
        }

        private OperationResult MemberValid(Users user)
        {
            //TODO 验证
            return new OperationResult { IsSuccess = true, Message = "验证成功", Data = user };
        }

        /// <summary>
        /// 更新用户分成
        /// </summary>
        /// <param name="userprices"></param>
        /// <returns></returns>
        public OperationResult InsertUpriceList(List<Userprice> userprices,int userId)
        {
            var res = _userRepository.InsertList(userprices, userId);
            if (res>0)
            {
               return new OperationResult { IsSuccess = true, Message = "更新成功" };
            }
            return new OperationResult { IsSuccess = false, Message = "更新失败" };
        }

        /// <summary>
        /// 重置用户分成
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OperationResult ResetUprice(int userId)
        {
            var res = _userRepository.ResetUserPrice(userId);
            if (res > 0)
            {
                return new OperationResult { IsSuccess = true, Message = "操作成功" };
            }
            return new OperationResult { IsSuccess = false, Message = "操作失败" };
        }
    }
}
