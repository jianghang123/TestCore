using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;

namespace TestCore.Common.Extensions
{
    public static class SessionExtensions
    {
        /// <summary>
        /// 保存自定义类型到 session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            try
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 从session 获取自定义类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            try
            {
                var jsonString = session.GetString(key);

                return jsonString == null ? default(T) : JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch
            {
                throw;
            }
        }
    }
}
