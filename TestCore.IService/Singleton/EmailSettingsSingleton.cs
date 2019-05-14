using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TestCore.Common.Infrastructure;
using TestCore.Domain.Singleton;

namespace TestCore.IService.Singleton
{
    /// <summary>
    /// Email配置单实例
    /// </summary>
    public class EmailSettingsSingleton : EmailSettings
    {
        private static EmailSettingsSingleton singleton;
        private static readonly object padlock = new object();
        public static EmailSettingsSingleton Singleton
        {
            get
            {
                if (singleton == null)
                {
                    lock (padlock)
                    {
                        if (singleton == null)
                        {
                            singleton = new EmailSettingsSingleton();
                        }
                    }
                }
                return singleton;
            }
            set
            {
                singleton = value;
            }
        }
        public EmailSettingsSingleton()
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();
            Dictionary<string, string> dic = settingService.GetConfigByGroupId((int)SettingEnum.Email);
            if (dic != null)
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
    }
}