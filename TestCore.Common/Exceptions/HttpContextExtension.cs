using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TestCore.Common.Extensions
{
    public static class HttpContextExtension
    {

        public static string GetUserIp(this HttpContext context)
        {
            var ips = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ips) && ips.Contains(","))
            {
                ips = ips.Split(',')[0];
            }

            if (string.IsNullOrEmpty(ips))
            {
                ips = context.Connection.RemoteIpAddress.ToString();
            }
            return ips;
        }

        //public static string GetUser(this HttpContext context)
        //{
        //    context.Session.SetString
        //    var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        //    if (string.IsNullOrEmpty(ip))
        //    {
        //        ip = context.Connection.RemoteIpAddress.ToString();
        //    }
        //    return ip;
        //}




    }
}
