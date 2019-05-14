using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TestCore.Common.Helper
{
    public static class ObjectHelper
    {

        /// <summary>
        /// 从 SelectList 中获取文本
        /// </summary>
        /// <param name="items"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetText(this IEnumerable<SelectListItem> items, object id)
        {
            if (!items.Any() || id == null) return string.Empty;

            return items.Where(c => c.Value.IsEquals(id.ToString())).Select(c => c.Text).FirstOrDefault();
        }

        /// <summary>
        /// 获取枚举值和显示名称的列表
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="dispalyName"></param>
        /// <returns></returns>
        public static List<NameItem> GetNameItems<TEnum>( bool dispalyName = true) where TEnum : struct
        {
            var list = new List<NameItem>();

            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                var item = new NameItem { Id = (int)value,
                     Name = value.ToString()
                };
                if (dispalyName)
                {
                    DisplayAttribute attr = value.GetType().GetField(value.ToString()).GetCustomAttribute<DisplayAttribute>();
                    if (attr != null)
                    {
                        item.Name = attr.Name;
                    }
                }
                list.Add(item);
            }
            return list;
        }

        public static string GetName(this IEnumerable<NameItem> items, int id)
        {
            if (items.Count() == 0) return string.Empty;

            return items.Where(c => c.Id == id).Select(c => c.Name).FirstOrDefault();
        }

 
    }
}
