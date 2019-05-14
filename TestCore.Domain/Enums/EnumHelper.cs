using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TestCore.Common;
using TestCore.Common.Cache;
using TestCore.Common.Helper;

namespace System
{
    public static class EnumHelper
    {

        #region fromCache

        /// <summary>
        /// 从缓存中获取枚举值和显示名称,返回 selectlist 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="displayName">true ： 从 Display 属性中获取名称， false : 显示属性名称 </param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectListFromCache<T>(bool displayName = true) where T : struct
        {
            return GetSelectListFromCache(typeof(T), displayName);
        }

        /// <summary>
        /// 从缓存中获取枚举值和显示名称,返回 selectlist 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectListFromCache(Type type, bool displayName = true)
        {
            var key = CommonConsts.GetEnumCacheKey(type, displayName);

            return CacheUtils.CacheService.GetOrAdd(key, () =>
            {
                return GetSelectList(type, displayName);
            });
        }





        /// <summary>
        /// 从缓存中获取枚举值显示名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDisplayName(Type type, object id)
        {
            var list = GetSelectListFromCache(type);

            return list.GetText(id);
        }

        /// <summary>
        /// 从缓存中获取枚举值显示名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDisplayName<T>(object id)
        {
            var list = GetSelectListFromCache(typeof(T));

            return list.GetText(id);
        }

        /// <summary>
        /// 从缓存中获取枚举的显示值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(object enumValue)
        {
            return GetDisplayName(enumValue.GetType(), (int)enumValue);
        }

        #endregion

        /// <summary>
        /// 获取枚举值和显示名称,返回 selectlist 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="displayName"> true： 返回 displayName ; false : 返回属性名称 </param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectList(Type type, bool displayName = true)
        {
            var list = new List<SelectListItem>();
            DisplayAttribute attr = null;

            Array array = Enum.GetValues(type);

            foreach (var value in array)
            {
                var intVal = (int)value;

                //if ( !IsAdd(ids,intVal)) continue;
                var strVal = value.ToString();
                var item = new SelectListItem { Value = intVal.ToString(), Text = strVal };

                if (displayName)
                {
                    attr = value.GetType().GetField(strVal).GetCustomAttribute<DisplayAttribute>();
                    if (attr != null)
                    {
                        PropertyInfo prop = (typeof(Caiba.Models.I18N.Admin.Resource)).GetProperty(attr.Name);
                        if (prop != null)
                        {
                            item.Text = prop.GetValue(null) + "";
                        }
                        else
                        {
                            item.Text = attr.Name;
                        }
                    }
                }
                list.Add(item);
            }
            return list;
        }


        public static List<SelectListItem> GetSelectList<TEnum>(bool displayName = true)
        {
            return GetSelectList(typeof(TEnum), displayName);
        }



        public static List<int> GetValues(Type type)
        {
            var array = Enum.GetValues(type);

            var list = new List<int>();

            foreach (var a in array)
            {
                list.Add((int)a);
            }
            return list;
        }

    }
}
