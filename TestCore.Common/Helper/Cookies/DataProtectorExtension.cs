using Microsoft.AspNetCore.DataProtection;
using System;

namespace TestCore.Common.Helper.Cookies
{
    /// <summary>
    /// 数据协议扩展方法类
    /// </summary>
    public static class DataProtectorExtension
    {
        public static bool TryUnprotect(this IDataProtector dataProtector, string protectedData, out string unProtectedData)
        {
            unProtectedData = string.Empty;
            try
            {
                unProtectedData = dataProtector.Unprotect(protectedData);
                return true;
            }
            catch (Exception)
            {
               
            }
            return false;
        }

    }
}
