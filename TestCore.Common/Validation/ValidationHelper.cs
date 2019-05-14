using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TestCore.Common.Validation
{
    public class ValidationHelper
    {
        public const string AccountRegularExpression = @"^[a-zA-Z0-9_]{6,15}$";
        public const string PhoneRegukarExpression = @"^1[1|2|3|4|5|6|7|8|9]\d{9}$";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryRegex(string input, RegularType type)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regularExpression = string.Empty;

            switch (type)
            {
                case RegularType.Int:
                    regularExpression = @"^\d+$";
                    break;
                case RegularType.PositiveInt:
                    regularExpression = @"^\+?[1-9][0-9]*$";
                    break;
                case RegularType.Money:
                    regularExpression = @"^[0-9]+(.[0-9]{2})?$";
                    break;
                case RegularType.Mail:
                    regularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    break;
                case RegularType.Postalcode:
                    regularExpression = @"^[0-9]\d{5}$";
                    break;
                case RegularType.Phone:
                    regularExpression = @"^(\d{3,4}|\d{3,4}-)?\d{7,8}$";
                    break;
                case RegularType.Mobile:
                    regularExpression = @"^1[1|2|3|4|5|6|7|8|9]\d{9}$";
                    break;
                case RegularType.InternetUrl:
                    regularExpression = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
                    break;
                case RegularType.IdCard: //身份证号(15位或18位数字)
                    regularExpression = @"^((1[1-5])|(2[1-3])|(3[1-7])|(4[1-6])|(5[0-4])|(6[1-5])|71|(8[12])|91)\d{4}((19\d{2}(0[13-9]|1[012])(0[1-9]|[12]\d|30))|(19\d{2}(0[13578]|1[02])31)|(19\d{2}02(0[1-9]|1\d|2[0-8]))|(19([13579][26]|[2468][048]|0[48])0229))\d{3}(\d|X|x)?$";
                    break;
                case RegularType.Date:   //日期范围:1900-2099;简单验证1-12月,1-31日
                    regularExpression = @"^(19|20)\d{2}-(0?\d|1[012])-(0?\d|[12]\d|3[01])$";
                    break;
                case RegularType.Ip:
                    regularExpression = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
                    break;
                case RegularType.QQ:
                    regularExpression = @"^[1-9]\d{5,12}$";
                    break;
                case RegularType.ChineseName:
                    regularExpression = @"[\u4e00-\u9fa5]{2,4}";
                    break;
                case RegularType.NickName:
                    regularExpression = @"^[^\d_+]([^x00-xff]|[\S]){1,14}$";
                    break;
                case RegularType.Account:
                    regularExpression = @"^(?![^a-zA-Z]+$).{6,15}$";
                    break;
                case RegularType.SystemAccount:
                    regularExpression = @"^(v|V)[0-9]+$";
                    break;
                case RegularType.LogInPassword:
                    regularExpression = @"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{8,16}$";
                    break;
                case RegularType.WalletPassword:
                    regularExpression = @"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{8,16}$";
                    break;
                default:
                    break;
            }

            Regex regex = new Regex(regularExpression);

            return regex.Match(input).Success;
        }

        /// <summary>
        /// 替换特殊字符串
        /// </summary>
        /// <param name="hexData"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacter(string hexData)
        {
            return Regex.Replace(hexData, "[ \\[ \\] \\^ \\-_*×――(^)$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", "").ToLower();
        }
    }

    /// <summary>
    /// RegularType
    /// </summary>
    public enum RegularType
    {
        Int = 1,
        PositiveInt = 2,
        Money = 3,
        Mail = 4,
        /// <summary>
        /// 邮编
        /// </summary>
        Postalcode = 5,
        Phone = 6,
        Mobile = 7,
        InternetUrl = 8,
        IdCard = 9,
        Date = 10,
        Ip = 11,
        QQ = 12,
        ChineseName = 13,
        /// <summary>
        /// 不以数字开头的昵称,长度为5-20位
        /// </summary>
        NickName = 14,
        /// <summary>
        /// 账号（6-16位英文和字母）
        /// </summary>
        Account = 15,
        /// <summary>
        /// 系统产生的Account
        /// </summary>
        SystemAccount = 16,
        /// <summary>
        /// 登录密码
        /// </summary>
        LogInPassword = 17,
        /// <summary>
        /// 钱包密码
        /// </summary>
        WalletPassword = 18,
    }


    public static class RegType
    {
        public const string Mobile = "";
        public const string LoginPassword = "";
        public const string WalletPassword = "";
    }
}
