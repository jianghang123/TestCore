using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using TestCore.Common;
using TestCore.Common.Captch;
using TestCore.Common.Configuration;
using TestCore.Common.Dapper;
using TestCore.Common.Helper;
using TestCore.Common.Infrastructure;
using TestCore.Common.Ioc;

namespace TestCore.Admin.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfigurationRoot configuration, ILoggerRepository repository)
        {
            //log
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            //add SqlServer connection configuration parameters
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
            //add CaibaConfig configuration parameters
            services.ConfigureStartupConfig<TestCoreConfig>(configuration.GetSection("TestCore"));
            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));
            //add project configuration parameters
            services.ConfigureStartupConfig<ProjectConfig>(configuration.GetSection("Project"));
            //add sysmanage configuration parameters
            services.ConfigureStartupConfig<SysManageSecurityConfig>(configuration.GetSection("SysManageSecurity"));
            //add accessor to HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddHttpContextAccessor();
            //Add session
            services.AddSession();
            //Cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";

            });

            var serviceProvider = IoCBootstrapper.Startup(services);
            return serviceProvider;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// Register DbConnection Provider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapperConnectionProvider<T>(this IServiceCollection services, IConfigurationSection configuration)
            where T : class, IConnectionProvider
        {
            services.Configure<ConnectionStringList>(configuration);
            services.AddSingleton<IConnectionProvider, T>(); //也可以改用Autofac注入
            return services;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            //services.AddAntiforgery(options =>
            //{
            //    var projectConfig = EngineContext.Current.Resolve<ProjectConfig>();
            //    options.Cookie.Name = projectConfig.CookieNamePrefix + "Antiforgery";
            //});
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                var projectConfig = EngineContext.Current.Resolve<ProjectConfig>();
                options.Cookie.Name = projectConfig.CookieNamePrefix + "Session";
                options.IdleTimeout = TimeSpan.FromMinutes(projectConfig.CookieExpire);
            });
        }

        /// <summary>
        /// Add and configure authentication for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddCaibaAuthentication(this IServiceCollection services)
        {
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .AddCookie(options =>
            //        {
            //            options.LoginPath = "/Login";

            //        });
        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddTestCoreMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddMvc();

            //use session temp data provider
            mvcBuilder.AddSessionStateTempDataProvider();

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            return mvcBuilder;
        } 
    }
}