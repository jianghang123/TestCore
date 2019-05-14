using System;

namespace TestCore.Common.Exceptions
{
    /// <summary>
    /// 自定义错误异常 200
    /// </summary>
    public class ErrorException : Exception
    {
        
        public ErrorException(string message):base(message)
        {
          
        }
    }
}
