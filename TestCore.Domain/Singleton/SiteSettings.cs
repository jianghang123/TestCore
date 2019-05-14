using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using TestCore.Common.Configuration;

namespace TestCore.Domain.Singleton
{
    /// <summary>
    /// 1-站点配置
    /// </summary>
    public class SiteSettings : ISettings
    {
        public SiteSettings() { }
        public SiteSettings(Dictionary<string, string> dic)
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
        /// 网站英文名称
        /// </summary>
        public string EnglishName { get; set; }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 备案号
        /// </summary>
        public string Icp { get; set; }
        /// <summary>
        /// 版权
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// 网站SEO标题
        /// </summary>
        public string MateTitle { get; set; }
        /// <summary>
        /// 网站SEO关键字
        /// </summary>
        public string MateKeywords { get; set; }
        /// <summary>
        /// 网站SEO描述
        /// </summary>
        public string MateDescription { get; set; }
    }
}
