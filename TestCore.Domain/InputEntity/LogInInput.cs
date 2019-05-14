using TestCore.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestCore.Domain.InputEntity
{
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LogInInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "登录类型必填")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "登录类型必填")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string ImgCode { get; set; }
    }
}
