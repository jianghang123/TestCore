using TestCore.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Primitives;
using System;
using System.Globalization;
using System.Linq;

namespace TestCore.Common.Helper
{
    public static class CoreHttpContext
    {
        public static IServiceProvider ServiceProvider;

        static CoreHttpContext()
        {
        }

        //public IOptions<ConnectionStrings> ConnStrings = IoCBootstrapper.AutoContainer.Resolve<IOptions<ConnectionStrings>>();

        public static IHttpContextAccessor contextFactory;

        public static IHttpContextAccessor ContextFactory
        {
            get
            {
                if (contextFactory == null)
                {
                    contextFactory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor)) as IHttpContextAccessor;
                }
                return contextFactory;
            }
        }


        public static HttpContext Current
        {
            get
            {
                return contextFactory.HttpContext;
            }
        }



        public static IRequestCultureFeature RequestCultureFeature
        {
            get
            {
                return contextFactory.HttpContext.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>();
            }
        }


        /// <summary>
        /// 当前上下文的区域文化信息
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get
            {
                try
                {
                    return RequestCultureFeature.RequestCulture.Culture;
                }
                catch (Exception ex)
                {
                    return new CultureInfo("zh-CN");
                }
            }
        }


        /// <summary>
        /// 获取用户IP地址  
        /// </summary>
        /// <returns></returns>
        private static string GetRealIP()
        {
            var request = CoreHttpContext.Current.Request;

            if (request.Headers == null)
            {
                return string.Empty;
            }
            StringValues ipVal = new StringValues("");

            if (request.Headers.TryGetValue("CF-CONNECTING-IP", out ipVal))
            {
                return ipVal.ToString();
            }
            if (request.Headers.TryGetValue("HTTP_LX_IP", out ipVal))
            {
                return ipVal.ToString();
            }
            if (request.Headers.TryGetValue("HTTP_X_FORWARDED_FOR", out ipVal))
            {
                return ipVal.ToString();
            }
            if (request.Headers.TryGetValue("X_FORWARDED_FOR", out ipVal))
            {
                return ipVal.ToString();
            }
            if (request.Headers.TryGetValue("REMOTE_ADDR", out ipVal))
            {
                return ipVal.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            var ip = GetRealIP();

            if (string.IsNullOrEmpty(ip))
            {
                ip = CoreHttpContext.Current.Connection.RemoteIpAddress.ToString();
            }
            return TypeHelper.GetSubString(ip, 20);
        }

        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP()
        {
            var ips = CoreHttpContext.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ips) && ips.Contains(","))
            {
                ips = ips.Split(',')[0];
            }

            if (string.IsNullOrEmpty(ips))
            {
                ips = CoreHttpContext.Current.Connection.RemoteIpAddress.ToString();
            }
            return ips;
        }

        /// <summary>
        /// 获取当前玩家Id
        /// </summary>
        /// <returns></returns>
        public static int MemId
        {
            get
            {
                if (Current.User.Identity.Name!=null)
                {
                    return Convert.ToInt32(Current.User.Identity.Name);
                }
                else
                {
                    throw new UnauthorizedException("请登录");
                }
                
            }
        }

    }
}
