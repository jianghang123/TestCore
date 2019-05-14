using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace TestCore.Common.Helper.Cookies
{
    /// <summary>
    /// Implementation of <see cref="ICookie" /> 
    /// </summary>
    public class HttpCookie : ICookie
    {
        #region Fields

        private readonly HttpContext _httpContext;
		private readonly IDataProtector _dataProtector;
		private static readonly string Purpose = "Caiba.Core.Helper.Cookies.Token.";
		private readonly CookieManagerOptions _cookieManagerOptions;
		private readonly ChunkingHttpCookie _chunkingHttpCookie;

        #endregion

        #region Ctor

        /// <summary>
        /// External depedenacy of <see cref="IHttpContextAccessor" /> 
        /// </summary>
        /// <param name="httpAccessor">IHttpAccessor</param>
        /// <param name="dataProtectionProvider">data protection provider</param>
        /// <param name="optionAccessor">cookie manager option accessor</param>
        public HttpCookie(IHttpContextAccessor httpAccessor, 
			IDataProtectionProvider dataProtectionProvider,
			IOptions<CookieManagerOptions> optionAccessor)
        {
            _httpContext = httpAccessor.HttpContext;
			_dataProtector = dataProtectionProvider.CreateProtector(Purpose);
			_cookieManagerOptions = optionAccessor.Value;
			_chunkingHttpCookie = new ChunkingHttpCookie(optionAccessor);
		}

        #endregion

        #region Methods

        /// <summary>
        /// 获取所有Cookie键名
        /// </summary>
        public ICollection<string> Keys
		{
			get
			{
				if(_httpContext == null)
				{
					throw new ArgumentNullException(nameof(_httpContext));
				}

				return _httpContext.Request.Cookies.Keys;
			}
		}

        /// <summary>
        /// 根据键名判断是否在cookie中存在
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public bool Contains(string key)
        {            
			if(_httpContext == null)
			{
				throw new ArgumentNullException(nameof(_httpContext));
			}

			if(key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

            return _httpContext.Request.Cookies.ContainsKey(key);
        }

        /// <summary>
        /// 根据键名获取Cookie值
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>value</returns>
        public string Get(string key)
        {
			if (_httpContext == null)
			{
				throw new ArgumentNullException(nameof(_httpContext));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (Contains(key))
			{
				var encodedValue = _chunkingHttpCookie.GetRequestCookie(_httpContext, key);
				var protectedData = string.Empty;
				if(Base64TextEncoder.TryDecode(encodedValue,out protectedData))
				{
					string unprotectedData;
					if(_dataProtector.TryUnprotect(protectedData, out unprotectedData))
					{
						return unprotectedData;
					}
				}
				return encodedValue;
			}

			return string.Empty;
        }

        /// <summary>
        /// 根据键名移除该Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        public void Remove(string key)
        {
			if (_httpContext == null)
			{
				throw new ArgumentNullException(nameof(_httpContext));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			_chunkingHttpCookie.RemoveCookie(_httpContext, key);
        }

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间(默认时间单位分钟)</param>
        public void Set(string key, string value, int? expireTime)
        { 
			if (_httpContext == null)
			{
				throw new ArgumentNullException(nameof(_httpContext));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			Set(key, value, null, expireTime);
        }

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="option">Cookie选项</param>
        public void Set(string key, string value, CookieOptions option)
		{
			if(_httpContext == null)
			{
				throw new ArgumentNullException(nameof(_httpContext));
			}

			if(key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if(option == null)
			{
				throw new ArgumentNullException(nameof(option));
			}

			Set(key, value, option, null);
		}

        /// <summary>
        /// 设置Cookie信息
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="option">Cookie选项</param>
        /// <param name="expireTime">过期时间</param>
		private void Set(string key, string value, CookieOptions option, int? expireTime)
		{
			if(option == null)
			{
				option = new CookieOptions();

				if (expireTime.HasValue)
					option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
				else
					option.Expires = DateTime.Now.AddDays(_cookieManagerOptions.DefaultExpireTimeInDays);
			}

			//判断是否需要加密
			if(_cookieManagerOptions.AllowEncryption)
			{
				string protecetedData = _dataProtector.Protect(value);
				var encodedValue = Base64TextEncoder.Encode(protecetedData);
				_chunkingHttpCookie.AppendResponseCookie(_httpContext, key, encodedValue, option);
			}
			else
			{ 
				_chunkingHttpCookie.AppendResponseCookie(_httpContext, key, value, option);
			}
			
		}

        #endregion

    }
}
