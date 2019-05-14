using Autofac;
using Microsoft.Extensions.Options;
using System;
using TestCore.Common.Ioc;

namespace TestCore.Common
{
    public class WebConfig
    {
        public static IOptions<AppSettings> AppSettingsOption = IoCBootstrapper.AutoContainer.Resolve<IOptions<AppSettings>>();
        public static IOptions<ApiSettings> ApiSettingsOption = IoCBootstrapper.AutoContainer.Resolve<IOptions<ApiSettings>>();
        public static IOptions<JWTSettings> JWTSettingsOption = IoCBootstrapper.AutoContainer.Resolve<IOptions<JWTSettings>>();
        public static IOptions<ConnectionStrings> ConnStringsOption = IoCBootstrapper.AutoContainer.Resolve<IOptions<ConnectionStrings>>();
        public static IOptions<RedisSettings> RedisSettingsOption = IoCBootstrapper.AutoContainer.Resolve<IOptions<RedisSettings>>();

        private static RedisSettings redisSettings;
        public static RedisSettings RedisSettings
        {
            get
            {
                try
                {
                    if (redisSettings == null)
                    {
                        redisSettings = RedisSettingsOption.Value ?? new RedisSettings();
                    }
                    return redisSettings;

                }
                catch
                {
                    throw new Exception("RedisSettings config Exception");
                }
            }
        }

        private static AppSettings appSettings;
        public static AppSettings AppSettings
        {
            get
            {
                try
                {
                    if (appSettings == null)
                    {
                        appSettings = AppSettingsOption.Value ?? new AppSettings();
                    }
                    return appSettings;

                }
                catch
                {
                    throw new Exception("AppSettings config Exception");
                }
            }
        }

        private static ApiSettings apiSettings;
        public static ApiSettings ApiSettings
        {
            get
            {
                try
                {
                    if (apiSettings == null)
                    {
                        apiSettings = ApiSettingsOption.Value ?? new ApiSettings();
                    }
                    return apiSettings;

                }
                catch
                {
                    throw new Exception("ApiSettings config Exception");
                }
            }
        }

        private static JWTSettings jwtSettings;
        public static JWTSettings JWTSettings
        {
            get
            {
                try
                {
                    if (jwtSettings == null)
                    {
                        jwtSettings = JWTSettingsOption.Value ?? new JWTSettings();
                    }
                    return jwtSettings;

                }
                catch
                {
                    throw new Exception("JWTSettings config Exception");
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

        private static string gamePlatformConnString;
        /// <summary>
        /// 平台数据库的默认连接字符串
        /// </summary>
        public static string GamePlatformConnString
        {
            get
            {
                try
                {
                    if (gamePlatformConnString == null)
                    {
                        gamePlatformConnString = ConnectionStrings.ConnectionSqlService;
                    }
                    return gamePlatformConnString;
                }
                catch
                {
                    throw new Exception("Conniction String config Exception");
                }
            }
        }
    }
}
