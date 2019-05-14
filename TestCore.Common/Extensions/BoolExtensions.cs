using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Extensions
{
    public static class BoolExtentions
    {
        /// <summary>
        /// false的时候抛出异常，配合事务try catch用于回滚
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        public static void ThrowException(this bool flag,string exceptionMessage,int hResult=600)
        {
            if (!flag)
            {
                var ex = new CustomException(exceptionMessage, hResult);
                
                throw ex;
            }
        }

        public static void ThrowExceptionTrue(this bool flag, string exceptionMessage, int hResult = 600)
        {
            if (flag)
            {
                var ex = new CustomException(exceptionMessage, hResult);

                throw ex;
            }
        }

    }
    public class CustomException : Exception
    {
        public CustomException(string message = "服务器内部错误", int hResult = 600) : base(message)
        {
            HResult = hResult;
        }
    }
}
