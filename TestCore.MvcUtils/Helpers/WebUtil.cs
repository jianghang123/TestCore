using Autofac;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using TestCore.Common;
using TestCore.Common.Cache;
using TestCore.Common.Helper;
using TestCore.Domain.SysEntity;

namespace TestCore.MvcUtils
{
    public class WebUtil
    {
        //private static IWebConfigRepository webConfigRepository;

        //public static IWebConfigRepository WebConfigRepository
        //{
        //    get
        //    {
        //        if (webConfigRepository == null)
        //        {
        //            webConfigRepository = IoCBootstrapper.AutoContainer.Resolve<IWebConfigRepository>();
        //        }
        //        return webConfigRepository;
        //    }
        //}

        ///// <summary>
        ///// 从缓存获取数据字典，返回selectList
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="valueField"></param>
        ///// <param name="textField"></param>
        ///// <param name="param"></param>
        ///// <param name="ordreBy"></param>
        ///// <returns></returns>
        //public static IEnumerable<SelectListItem> GetSelectListFromCache(Type type, string valueField, string textField, object param = null, string ordreBy = null)
        //{
        //    var key = CommonConsts.GetCacheKey(type);

        //    return CacheUtils.CacheService.GetOrAdd(key, () =>
        //    {
        //        return WebConfigRepository.GetSelectListAsync(valueField, textField, type.Name, param, ordreBy).Result;
        //    });
        //}

        ///// <summary>
        ///// 从缓存获取数据字典，返回selectList
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="valueField"></param>
        ///// <param name="textField"></param>
        ///// <param name="param"></param>
        ///// <param name="ordreBy"></param>
        ///// <returns></returns>
        //public static IEnumerable<SelectListItem> GetSelectListFromCache<T>(string valueField, string textField, object param = null, string ordreBy = null)
        //{
        //    return GetSelectListFromCache(typeof(T), valueField, textField, param, ordreBy);
        //}

        ///// <summary>
        ///// 根据站点Id，从缓存获取获取当前站点配置信息
        ///// </summary>
        ///// <param name="siteId"></param>
        ///// <returns></returns>
        //public static SysWebConfig GetWebConfigFromCache(int siteId)
        //{
        //    return WebConfigRepository.GetModelFromCache(siteId);
        //}

        //public static int GetOperId(int siteId)
        //{
        //    var webConfig = WebConfigRepository.GetModelFromCache(siteId);

        //    return webConfig.OperId;
        //}

        /// <summary>
        /// 获取语言简称
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string GetShortLang(string lang = null)
        {
            if (lang == null) lang = CoreHttpContext.CurrentCulture.Name;
            var langArray = lang.Split('-');
            if (langArray == null || langArray.Length < 2) return "cn";
            if ("US".IsEquals(langArray[1]))
            {
                return "en";
            }
            return langArray[1];
        }

        public static string GetLang(string lang = null)
        {
            if (lang == null) lang = CoreHttpContext.CurrentCulture.Name;
            return lang;
        }
    }
}
