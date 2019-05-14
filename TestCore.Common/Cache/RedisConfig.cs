using StackExchange.Redis;
using System;

namespace TestCore.Common.Cache
{
    public class RedisConfig
    {
        private static ConnectionMultiplexer Redis { get; set; } = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        private static IDatabase Db { get; set; } = Redis.GetDatabase();

        /// <summary>
        /// 增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue(string key, string value,TimeSpan expiresTime)
        {
            return Db.StringSet(key, value, expiresTime);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Db.StringGet(key);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteKey(string key)
        {
            return Db.KeyDelete(key);
        }
    }
}
