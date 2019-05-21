using Autofac;
using Microsoft.Extensions.Options;
using System;
using TestCore.Common;
using TestCore.Common.Ioc;

namespace TestCore.MvcUtils.Admin
{
    public class AdminConfig
    {
        public static IOptions<AppSettings> AppSettingsOption = IoCBootstrapper.AutoContainer.Resolve<IOptions<TestCore.MvcUtils.Admin.AppSettings>>();

        public static IOptions<ConnectionStrings> ConnStringsOption = IoCBootstrapper.AutoContainer.Resolve<IOptions<ConnectionStrings>>();

        private static AppSettings appSettings;

        public static AppSettings AppSettings
        {
            get
            {
                try
                {
                    if (appSettings == null)
                    {
                        appSettings = AppSettingsOption.Value ?? new TestCore.MvcUtils.Admin.AppSettings();
                    }
                    return appSettings;
                }
                catch
                {
                    throw new Exception("AppSettings config Exception");
                }
            }
        }
     
        private static ConnectionStrings connectionStrings;

        public static ConnectionStrings ConnectionStrings
        {
            get
            {
                try
                {
                    if (connectionStrings == null)
                    {
                        connectionStrings = ConnStringsOption.Value ?? new ConnectionStrings(); 
                    }
                    return connectionStrings;
                }
                catch
                {
                    throw new Exception("Conniction String config Exception");
                }
            }
        }

        private static string connectionSqlService;

        /// <summary>
        /// 平台数据库的默认连接字符串
        /// </summary>
        public static string ConnectionSqlService
        {
            get
            {
                try
                {
                    if (connectionSqlService == null)
                    {
                        connectionSqlService = ConnectionStrings.ConnectionSqlService;
                    }
                    return connectionSqlService;
                }
                catch
                {
                    throw new Exception("Conniction String config Exception");
                }
            }
        }

    }
}
