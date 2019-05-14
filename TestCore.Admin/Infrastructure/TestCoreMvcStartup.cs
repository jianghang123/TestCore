using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestCore.Admin.Infrastructure.Extensions;
using TestCore.Common.Infrastructure;

namespace TestCore.Admin.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring MVC on application startup
    /// </summary>
    public class TestCoreMvcStartup : ITestCoreStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {  
            //add and configure MVC feature
            services.AddTestCoreMvc(); 
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        { 
            //MVC routing
            application.UseTestCoreMvc();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        //TODO MVC should be loaded last
        public int Order => 1000; 
    }
}
