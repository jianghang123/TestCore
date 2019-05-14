using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Linq;
using TestCore.Admin.Infrastructure.Extensions;
using TestCore.Common.Configuration;
using TestCore.Common.Helper.Cookies;
using TestCore.Common.Infrastructure;

namespace TestCore.Admin.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring common features and middleware on application startup
    /// </summary>
    public class TestCoreCommonStartup : ITestCoreStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            //add response compression
            services.AddResponseCompression();

            //add options feature
            services.AddOptions();

            //add memory cache
            services.AddMemoryCache();

            //add distributed memory cache
            services.AddDistributedMemoryCache();

            //add cookie manager 
            services.AddCookieManager();

            //add HTTP sesion state feature
            services.AddHttpSession();

            //add anti-forgery
            services.AddAntiForgery();

            //add localization
            services.AddLocalization();

            //add UEditor
            //services.AddUEditor();


        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        { 
            var caibaConfig = EngineContext.Current.Resolve<TestCoreConfig>();
            var fileProvider = EngineContext.Current.Resolve<ITestCoreFileProvider>();
            //compression
            if (caibaConfig.UseResponseCompression)
            {
                //gzip by default
                application.UseResponseCompression();
            }
            //static files
            application.UseStaticFiles(new StaticFileOptions
            {
                //TODO duplicated code (below)
                OnPrepareResponse = ctx =>
                {
                    if (!string.IsNullOrEmpty(caibaConfig.StaticFilesCacheControl))
                        ctx.Context.Response.Headers.Append(HeaderNames.CacheControl, caibaConfig.StaticFilesCacheControl);
                }
            }); 
           
            //use HTTP session
            application.UseSession();

            //use request localization
            application.UseRequestLocalization();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        //TODO common services should be loaded after error handlers
        public int Order => 100; 
    }
}
