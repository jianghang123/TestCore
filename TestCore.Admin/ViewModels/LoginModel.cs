using System.ComponentModel.DataAnnotations;

namespace TestCore.Admin.ViewModels
{
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>  
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [DataType(DataType.Password)]
        public string UserPwd { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataType(DataType.Password)]
        public string SafetyCode { get; set; }

        /// <summary>
        /// 用户验证码
        /// </summary>
        [MaxLength(4)]
        public string UserCode { get; set; }
    }
}
