using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Exceptions
{
    /// <summary>
    /// 自定义异常 401
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {

        }
    }
}
