using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestCore.Domain.InputEntity
{
    /// <summary>
    /// 注册参数
    /// </summary>
    public class RegisterInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "登录类型必填")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不可为空")]
        [StringLength(16, ErrorMessage = "密码必须8-16位，包含数字+字母组合!", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Userpass { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Compare(nameof(Userpass), ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmUserpass { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string ImgCode { get; set; }

    }
}
