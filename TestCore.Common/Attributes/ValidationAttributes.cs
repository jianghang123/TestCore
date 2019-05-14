using TestCore.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace TestCore.Common.Helpers
{
    /// <summary>
    /// 手机格式
    /// </summary>
    public class MobileAttribute : RequiredAttribute
    {
        //第一个参数是验证对象的值
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!AllowEmptyStrings && value==null)
            {
                return new ValidationResult("手机号必填");
            }
            //验证
            if (!ValidationHelper.TryRegex(value.ToString(), RegularType.Mobile))
            {
                return new ValidationResult("手机格式不合法");
            }

            return ValidationResult.Success;
        }
    }


    /// <summary>
    /// 登录密码
    /// </summary>
    public class LogInPasswordAttribute : RequiredAttribute
    {
        //第一个参数是验证对象的值
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!AllowEmptyStrings && value == null)
            {
                return new ValidationResult("登录密码必填");
            }
            //验证
            if (!ValidationHelper.TryRegex(value.ToString(), RegularType.LogInPassword))
            {
                return new ValidationResult("登录密码必须8-16位，包含数字+字母组合!");
            }

            return ValidationResult.Success;
        }
    }


    /// <summary>
    /// 安全码
    /// </summary>
    public class WalletPasswordAttribute : RequiredAttribute
    {
        //第一个参数是验证对象的值
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!AllowEmptyStrings && value == null)
            {
                return new ValidationResult("安全码必填");
            }
            //验证
            if (!ValidationHelper.TryRegex(value.ToString(), RegularType.LogInPassword))
            {
                return new ValidationResult("安全码必须8-16位，包含数字+字母组合!");
            }

            return ValidationResult.Success;
        }
    }

}
