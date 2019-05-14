using TestCore.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace TestCore.Domain.Singleton
{
    /// <summary>
    /// 3-邮箱配置
    /// </summary>
    public class EmailSettings : ISettings
    {
        public EmailSettings() { }
        public EmailSettings(Dictionary<string, string> dic)
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
        /// SMTP地址
        /// </summary>
        public string SMTPServerAddress { get; set; }
        /// <summary>
        /// SMTP端口号
        /// </summary>
        public string SMTPPort { get; set; }
        /// <summary>
        /// SMTP UserName
        /// </summary>
        public string SMTPUserName { get; set; }
        /// <summary>
        /// SMTP Password
        /// </summary>
        public string SMTPUserPassword { get; set; }
        /// <summary>
        /// Email 发件人
        /// </summary>
        public string MailSender { get; set; }
        /// <summary>
        /// 是否采用SSL连接（true和false)
        /// </summary>
        public string EnableSsl { get; set; }
    }
}
