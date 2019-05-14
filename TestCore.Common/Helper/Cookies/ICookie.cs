using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TestCore.Common.Ioc;

namespace TestCore.Common.Helper.Cookies
{
    /// <summary>
    /// ICookie is absstraction layer on top of ASP.Net Core Cookie API
    /// </summary>
    public interface ICookie :ISingletonDependency
    {

        /// <summary>
        /// 获取所有Cookie键名
        /// </summary>
        ICollection<string> Keys { get; }

        /// <summary>
        /// 根据键名获取Cookie值
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>值</returns>
        string Get(string key);

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间(默认时间是1天)</param>
        void Set(string key, string value, int? expireTime);

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="option">Cookie选项</param>
        void Set(string key, string value, CookieOptions option);

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
