using System;
using System.Collections.Generic;
using System.Linq;
using TestCore.Common;
using TestCore.Common.Cache;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class HtmlHelperExtensions
    {

        /// <summary>
        ///  获取枚举显示的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDisplayName<TEnum>(this IHtmlHelper html, object id) where TEnum : struct
        {
            return GetDisplayName(html, typeof(TEnum), id);
        }

        /// <summary>
        /// 获取枚举显示的名称
        /// </summary>
        /// <param name="html"></param>
        /// <param name="type"> 枚举类型</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDisplayName(this IHtmlHelper html, Type enumType, object id)
        {
            if(id == null) return string.Empty;

            var selectList = html.GetSelectListFromCache(enumType);

            if (!selectList.Any()) return string.Empty;

            var name = selectList.Where(c => c.Value == id.ToString()).Select(c => c.Text).FirstOrDefault();

            if (!string.IsNullOrEmpty(name)) return name;

            return id.ToString();
        }

        /// <summary>
        ///  从缓存中获取 list ，如果没有，获取后加入缓存
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectListFromCache<TEnum>(this IHtmlHelper html) where TEnum : struct
        {
            return GetSelectListFromCache(html,typeof(TEnum));
        }

        /// <summary>
        /// 从缓存中获取 SelectListItem list ，如果没有，获取后加入缓存
        /// </summary>
        /// <param name="html"></param>
        /// <param name="type">枚举类型</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectListFromCache(this IHtmlHelper html, Type enumType)
        {
            var key = CommonConsts.GetEnumCacheKey(enumType);

            return CacheUtils.CacheService.GetOrAdd(key, () =>
            {
                return html.GetEnumSelectList(enumType);
            });
        }


    }
}
