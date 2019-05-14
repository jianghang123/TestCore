using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TestCore.Common.Infrastructure;
using TestCore.Domain.Singleton;

namespace TestCore.IService.Singleton
{
    /// <summary>
    /// 短信配置单实例
    /// </summary>
    public class SMSSettingsSingleton : SMSSettings
    {
        private static SMSSettingsSingleton singleton;
        private static readonly object padlock = new object();
        public static SMSSettingsSingleton Singleton
        {
            get
            {
                if (singleton == null)
                {
                    lock (padlock)
                    {
                        if (singleton == null)
                        {
                            singleton = new SMSSettingsSingleton();
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
        public SMSSettingsSingleton()
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();
            Dictionary<string, string> dic = settingService.GetConfigByGroupId((int)SettingEnum.SMS);
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