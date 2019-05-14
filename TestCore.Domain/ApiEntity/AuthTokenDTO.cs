using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.ApiEntity
{
    /// <summary>
    /// 授权的Token
    /// </summary>
    public class AuthTokenDTO
    {
        /// <summary>
        /// token
        /// </summary>
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInMinutes { get; set; }

        public int MemId { get; set; }
    }
}
