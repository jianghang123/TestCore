using TestCore.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common
{
    public  class CommonConsts
    {
        //public static string AuthenticationScheme = "Cookie";
        public static string AuthenticationType = "SuperSecureLogin";
        public static List<string> langArray = new List<string>{ "cn","zh","en"};
        public static string CurrentLanguage = "language";
        public static int SqucenceID = 123456;
        public static DateTime DefaultTime = new DateTime(1900,1,1);


        public const string CacheKey_SysResource = "Caiba.Repositories.Sys.SysResource";
        public const string Cache_Catalog_Default = "";
        public const string Cache_CheckMemOnline_Key = "memonlinesvc_checkmemonline_";
        public const string MemberRegisterValidateCodeKey = "MemberRegisterValidateCode";
        public const string MemberLoginValidateCodeKey = "MemberLoginValidateCode";
        public const string MemberQLoginValidateCodeKey = "MemberQLoginValidateCode";
        public const string ForgotPasswordValidateCodeKey = "ForgotPasswordValidateCodeKey";
        public const string SmsRegisterCodeKey = "SmsRegisterCodeKey";
        public const string AffValidateCodeKey = "AffValidateCodeKey";

        public const string SMSCodeLogin = "SMSCodeLoginKey";

        #region CacheKey

        /// <summary>
        /// 资源缓存的 key, 分每个表、语言进行缓存
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string GetCacheKey_Resource(int tableId, string lang)
        {
            return string.Format("{0}_{1}_{2}", CommonConsts.CacheKey_SysResource, tableId, lang).ToLower();
        }

        /// <summary>
        ///根据类型获取缓存的 key
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEnumCacheKey(Type type, bool displayName = true)
        {
            return string.Format("{0}_{1}_{2}", type.FullName, CoreHttpContext.CurrentCulture.Name, displayName);
        }


        public static string GetCacheKey(Type type)
        {
            return string.Format("{0}_{1}", type.FullName, CoreHttpContext.CurrentCulture.Name);
        }


        public static string GetGameCacheKey( string siteId )
        {
            return string.Format("Caiba.Models.Entities.GameInfo_{0}", siteId, CoreHttpContext.CurrentCulture.Name);
        }

        #endregion
    }
}
