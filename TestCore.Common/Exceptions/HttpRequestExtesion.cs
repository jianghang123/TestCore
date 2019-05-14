using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Mvc
{
    public static class HttpRequestExtesion
    {

        public static void SetHeaderValue(this HttpRequest request, string name, string value)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
            {
                request.Headers.Add(name,  new StringValues(value));
            }
        }


        public static string GetHeaderValue(this HttpRequest request, string name)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            StringValues val = new StringValues("");
            if (request.Headers != null)
            {
                request.Headers.TryGetValue(name, out val);
            }
            return val.ToString();
        }


        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        /// <summary>
        /// 获取 ? 后面的查询字符串
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetQueryString(this HttpRequest request)
        {
            if (!request.QueryString.HasValue) return null;

            return request.QueryString.Value.Substring(1);
        }


        public static string GetParam(this HttpRequest request,string key)
        {
            try
            {
                if (request.Query.Keys.Contains(key))
                {
                    return request.Query[key].ToString();
                }
                else if (request.Form.Keys.Contains(key))
                {
                    return request.Form[key].ToString();
                }
                return string.Empty;
            }catch (Exception ex)
            {
                return string.Empty;
            }
        }



        public static int? GetIntParam(this HttpRequest request, string key)
        {
            try
            {

                if (request.Query.Keys.Contains(key))
                {
                    return int.Parse( request.Query[key].ToString());
                }
                else if (request.Form.Keys.Contains(key))
                {
                    return int.Parse(request.Form[key].ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




    }
}
