using Autofac;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;
using TestCore.IRepository.SysAdmin;

namespace TestCore.Repositories
{
    public static class ResourceHelper
    {
        private static IResourceRepository resourceRepository = IoCBootstrapper.AutoContainer.Resolve<IResourceRepository>();

        private static ILangRepository _langRepository;



    


        /// <summary>
        /// 从缓存中获取数据字典的当前语言资源 返回 SelectListItem list, 可以用于绑定下拉列表 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkids"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectList(TableEnum table, string fieldName = null, int[] pkids = null)
        {
            return GetSelectList((int)table, fieldName, pkids);
        }

        /// <summary>
        /// 从缓存中获取数据字典的当前语言资源 返回 SelectListItem list，可以用于绑定下拉列表
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkids"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectList(int tableId, string fieldName = null, int[] pkids = null)
        {
            return resourceRepository.GetSelectListAsync(tableId, fieldName, pkids).Result;
        }

        /// <summary>
        /// 从缓存中获取数据字典的当前语言资源 返回 SimpleResource list
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkids"></param>
        /// <returns></returns>
        public static IEnumerable<SimpleResource> GetListFromCache(int tableId, string fieldName, int[] pkids = null)
        {
            return resourceRepository.GetListFromCache(tableId, fieldName, pkids);
        }


        /// <summary>
        /// 从缓存中获取数据字典的当前语言资源 返回 SelectListItem list, 可以用于绑定下拉列表 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkids"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectListFromCache(TableEnum table, string fieldName = null, int[] pkids = null)
        {
            return resourceRepository.GetSelectListFromCache((int)table, fieldName, pkids);
        }

        /// <summary>
        /// 从缓存中获取数据字典的当前语言资源 返回 SelectListItem list，可以用于绑定下拉列表
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkids"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectListFromCache(int tableId, string fieldName = null, int[] pkids = null)
        {
            return resourceRepository.GetSelectListFromCache(tableId, fieldName, pkids);
        }

        /// <summary>
        ///  从 sysResource 表中,切换语言
        /// </summary>
        /// <typeparam name="T"> 在 sysResource 表 里面有配置资源的数据表对应的类型 </typeparam>
        /// <param name="list"></param>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IEnumerable<T> ChangeLang<T>(this IEnumerable<T> list, string fieldName = null, TableEnum table = TableEnum.None, int[] pkIds = null)
        {
            if (!list.Any()) return list;

            if (table == TableEnum.None)
            {
                table = TypeHelper.TryParse<TableEnum>(typeof(T).Name);
            }
            var suorList = resourceRepository.GetListFromCache((int)table, fieldName, pkIds);

            return list.ChangeLangAsync(suorList, fieldName);
        }

        /// <summary>
        ///  从 sysResource 表中,切换语言
        /// </summary>
        /// <typeparam name="T">在 sysResource 表 里面有配置资源的数据表对应的类型</typeparam>
        /// <param name="list"></param>
        /// <param name="sourceList"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static IEnumerable<T> ChangeLangAsync<T>(this IEnumerable<T> list, IEnumerable<SimpleResource> sourceList, string fieldName)
        {
            if (!list.Any() || !sourceList.Any()) return list;

            var type = typeof(T);

            var pros = GetKeyValueProperty(type, fieldName);

            var targetList = list.ToList();

            int id = 0;
            string text = null;
            foreach (var item in targetList)
            {
                id = (int)(pros.Item1.GetValue(item));  /// 获取主键的值
                foreach(var p in pros.Item2)
                {
                    text = sourceList.Where(c=>c.PkId == id)
                        .Where(c => c.FieldName.IsEquals(p.Name))
                        .Select(c => c.ResValue).FirstOrDefault();
                    if (!string.IsNullOrEmpty(text))
                    {
                        p.SetValue(item, text);
                    }
                }
            }
            return targetList;
        }


        public static string GetNameFromCache(TableEnum table, string fieldName, int pkid, string lang = null)
        {
            return resourceRepository.GetResourceFromCache((int)table, fieldName, pkid, lang);
        }

        /// <summary>
        /// 從緩存中取數據
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string GetDisplayName(TableEnum table, string fieldName, int pkid, string lang = null)
        {
            return resourceRepository.GetResourceFromCache((int)table, fieldName, pkid, lang);
        }

        /// <summary>
        /// 获取主键和指定名称的 属性信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static Tuple<PropertyInfo, PropertyInfo[]> GetKeyValueProperty(Type type, string fieldName)
        {
            string pkName = "Id";

            var pkPro = type.GetProperties().Where(c => c.Name.IsEquals(pkName)).FirstOrDefault();

            if (pkPro == null)  //如果主键名称不是 Id
            {
                pkName = type.GetPkName();
                pkPro = type.GetProperties().Where(c => c.Name.IsEquals(pkName)).FirstOrDefault();
            }
            if (pkPro == null)
            {
                throw new Exception(string.Format("类型{0}没有设置主键", type.Name));
            }
            var names = fieldName.Split(',');
 
            var namePros = type.GetProperties().Where(c => names.IsContains( c.Name )).ToArray();

            if (!namePros.Any())
            {
                throw new Exception(string.Format("属性名称{0}不正确", fieldName));
            }
            return Tuple.Create(pkPro, namePros);
        }


        #region  Lang

        /// <summary>
        /// 从缓存获取语言列表，返回 SelectListItem 列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetLangSelectListFromCache()
        {
            try
            {
                if (_langRepository == null)
                {
                    _langRepository = IoCBootstrapper.AutoContainer.Resolve<ILangRepository>();
                }
                return _langRepository.GetSelectListFromCache();
            }
            catch
            {
                throw;
            }
        }

        #endregion



       public static void ClearResourceCache(int tableId, string lang = null)
       {
            resourceRepository.ClearResourceCache(tableId, lang);
       }


    }
}
