using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.InputEntity
{
    public class TokenEntity
    {
        /// <summary>
        /// token
        /// </summary>
        public string AccessToken { get; set; }

        public int ExpireInMinutes { get; set; }

        public int UserId { get; set; }
    }
}
