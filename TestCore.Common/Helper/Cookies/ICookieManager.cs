using Microsoft.AspNetCore.Http;
using System;
using TestCore.Common.Ioc;

namespace TestCore.Common.Helper.Cookies
{
    /// <summary>
    /// Cookie管理器是抽象层 <see cref="ICookie" />
    /// </summary>
    public interface ICookieManager : ISingletonDependency
    {
        /// <summary>
        /// 根据键名获取关联的Cookie 
        /// </summary>
        /// <typeparam name="T">TSource</typeparam>
        /// <param name="key">Key</param>
        /// <returns>TSource object</returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取并设置Cookie对象
        /// </summary>
        /// <typeparam name="T">TSource</typeparam>
        /// <param name="key">Key</param>
        /// <param name="acquirer">function</param>
        /// <param name="expireTime">cookie expire time</param>
        /// <returns></returns>
        T GetOrSet<T>(string key, Func<T> acquirer, int? expireTime = null);

        /// <summary>
        /// 获取并设置Cookie对象
        /// </summary>
        /// <typeparam name="T">TSource</typeparam>
        /// <param name="key">Key</param>
        /// <param name="acquirer">如果Cookie超过了它的执行，获取函数并设置cookie</param>
        /// <param name="option">cookie option</param>
        /// <returns></returns>
        T GetOrSet<T>(string key, Func<T> acquirer, CookieOptions option);

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间(默认时间是1天)</param>
        void Set(string key, object value, int? expireTime = null);

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="option">Cookie选项</param>
        void Set(string key, object value, CookieOptions option);

        /// <summary>
        /// 根据键名判断是否在cookie中存在
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// 根据键名移除该Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        void Remove(string key);
    }
}