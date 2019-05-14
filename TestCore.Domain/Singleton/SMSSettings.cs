using TestCore.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace TestCore.Domain.Singleton
{
    /// <summary>
    /// 4-短信配置
    /// </summary>
    public class SMSSettings : ISettings
    {
        public SMSSettings() { }
        public SMSSettings(Dictionary<string, string> dic)
        {
            if (dic != null && dic.Count > 0)
            {
                foreach (string key in dic.Keys)
                {
                    string value = dic[key];
                    PropertyInfo property = GetType().GetProperty(key);
                    if (property == null)
                    {
                        continue;
                    }
                    else
                    {
                        property.SetValue(this, Convert.ChangeType(value, property.PropertyType, CultureInfo.CurrentCulture), null);
                    }
                }
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string SignName { get; set; }
        /// <summary>
        /// 有效时长（分钟）
        /// </summary>
        public int VerifyExpireMinutes { get; set; }
        /// <summary>
        /// 注册短信内容
        /// </summary>
        public string RegBody { get; set; }
        /// <summary>
        /// 绑定手机号短信内容
        /// </summary>
        public string BindMobileBody { get; set; }
        /// <summary>
        /// 找回密码短信内容
        /// </summary>
        public string FindPwdBody { get; set; }
        /// <summary>
        /// 更改密码短信内容
        /// </summary>
        public string UpdatePwdBody { get; set; }
        /// <summary>
        /// 更换手机号码第一步短信内容
        /// </summary>
        public string UpdateMobileStep1Body { get; set; }
        /// <summary>
        /// 更换手机号码第二步短信内容
        /// </summary>
        public string UpdateMobileStep2Body { get; set; }
    }
}
