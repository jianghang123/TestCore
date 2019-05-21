using log4net.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TestCore.Common;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;
using TestCore.MvcUtils;
using TestCore.MvcUtils.Admin;

namespace TestCore.Admin
{
    public class Startup
    {
        public static ILoggerRepository Repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            ///mvc 加过滤器
            services.AddMvc(cfg =>
            {
                cfg.Filters.Add(typeof(HandleException));

            }).AddViewLocalization().AddDataAnnotationsLocalization();
            services.AddOptions();
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(60);

            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Login/Index";
                        options.LogoutPath = "/Login/Logout";
                        options.AccessDeniedPath = "/Login/NoRight";
                    });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024 * 1024 * 2;
            });
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<TestCore.MvcUtils.Admin.AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));

            ///httpContext 使用
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var ServiceProvider = IoCBootstrapper.Startup(services);
            return ServiceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svrProvider)
        {
            if (AdminConfig.AppSettings.DisplayFullErrorStack || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            ///设置服务处理器
            CoreHttpContext.contextFactory = svrProvider.GetRequiredService<IHttpContextAccessor>();
        
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                // Areas support
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
