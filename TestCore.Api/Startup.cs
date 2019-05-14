using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestCore.Common;
using TestCore.Common.Exceptions;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;
using TestCore.Common.Log;

namespace TestCore.Api
{
    public class Startup
    {

        public static ILoggerRepository Repository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //跨域
            services.AddCors(option =>
            {
                if (!string.IsNullOrEmpty(WebConfig.AppSettings.CorsOrigins))
                {
                    var urls = WebConfig.AppSettings.CorsOrigins.Split(',');
                    option.AddPolicy("AllowSameDomain", builder => builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
                }
                else
                {
                    option.AddPolicy("AllowSameDomain", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
                }
            });

            //*注入全局异常捕获*/
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(GlobalExceptions));
            });

            //HttpContext服务
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //配置文件json注册
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<JWTSettings>(Configuration.GetSection("Authentication").GetSection("JwtSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSetting"));

            //jwt服务         
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(option =>
                    {
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = WebConfig.JWTSettings.Issuer,
                            ValidAudience = WebConfig.JWTSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebConfig.JWTSettings.Secret))
                        };
                    });
            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "My API",
                    Version = "v1",
                });


                var filePath = Path.Combine(AppContext.BaseDirectory, "TestCore.API.xml");
                c.IncludeXmlComments(filePath);
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "请输入带有Bearer的Token,例如: \"Bearer {token}\"", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                {
                     "Bearer",
                     Enumerable.Empty<string>()
                }});
            });

            //批量注册dll
            return IoCBootstrapper.Startup(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="serviceProvider"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (WebConfig.AppSettings.DisplayFullErrorStack || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //异常处理
                app.UseExceptionHandler(option => option.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = feature.Error;
                    var message = "";
                    if (exception is ErrorException)
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        message = exception.Message;
                    }
                    else if (exception is UnauthorizedException)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        message = "Unauthorized";
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        message = "Server Internal Error";
                    }

                    var result = JsonConvert.SerializeObject(new { result = 0, message });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }));

                app.UseHsts();
            }

            //设置服务处理器
            CoreHttpContext.contextFactory = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            app.UseCors("AllowSameDomain");
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                //c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
