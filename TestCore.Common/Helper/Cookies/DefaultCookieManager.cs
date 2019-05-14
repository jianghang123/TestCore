using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace TestCore.Common.Helper.Cookies
{
    /// <summary>
	/// Implementation of <see cref="ICookieManager" /> 
	/// </summary>
	public class DefaultCookieManager : ICookieManager
    {
        #region Fields

        private readonly ICookie _cookie;

        #endregion

        #region Ctor

        public DefaultCookieManager(ICookie cookie)
        {
            this._cookie = cookie;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 根据键名判断是否在cookie中存在
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return _cookie.Contains(key);
        }

        /// <summary>
        /// 如果指定的键过期，则获取值或设置值
        /// </summary>
        /// <typeparam name="T">TSource</typeparam>
        /// <param name="key">Key</param>
        /// <param name="acquirer">action to execute</param>
        /// <param name="expireTime">Expire time</param>
        /// <returns>TSource object</returns>
        public T GetOrSet<T>(string key, Func<T> acquirer, int? expireTime = default(int?))
        {
            if (_cookie.Contains(key))
            { 
                return GetExisting<T>(key);
            }
            else
            {
                var value = acquirer();
                this.Set(key, value, expireTime);

                return value;
            }
        }

        /// <summary>
        /// 根据键名判断是否存在TSource的Cookie信息
        /// </summary>
        /// <typeparam name="T">TSource</typeparam>
        /// <param name="key">Key</param>
        /// <returns>TSource Object</returns>
        private T GetExisting<T>(string key)
        {
            var value = _cookie.Get(key);

            if (string.IsNullOrEmpty(value))
                return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 根据键名移除该Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        public void Remove(string key)
        {
            _cookie.Remove(key);
        }

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间(默认时间单位分钟)</param>
        public void Set(string key, object value, int? expireTime = default(int?))
        {
            _cookie.Set(key, JsonConvert.SerializeObject(value), expireTime);
        }


        /// <summary>
        /// 获取指定键的值，类型为TSource
        /// </summary>
        /// <typeparam name="T">TSource</typeparam>
        /// <param name="key">Key</param>
        /// <returns>TSource object</returns>
        public T Get<T>(string key)
        {
            return GetExisting<T>(key);
        }

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="option">Cookie选项</param>
        public void Set(string key, object value, CookieOptions option)
        {
            _cookie.Set(key, JsonConvert.SerializeObject(value), option);
        }

        public T GetOrSet<T>(string key, Func<T> acquirer, CookieOptions option)
        {
            if (_cookie.Contains(key))
            {
                //get the existing value
                return GetExisting<T>(key);
            }
            else
            {
                var value = acquirer();
                this.Set(key, value, option);

                return value;
            }
        }

        #endregion

    }
}
