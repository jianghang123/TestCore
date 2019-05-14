using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using TestCore.Common.Cache;
using TestCore.Domain.SysEntity;
using TestCore.IRepository.SysAdmin;
using TestCore.Repositories;

namespace Caiba.Repositories.Sys
{
    public class LangRepository : BaseRepository<SysLang>, ILangRepository
    {
        private readonly ICacheService _cacheSvc;
        public LangRepository(ICacheService cacheSvc)
        {
            _cacheSvc = cacheSvc;
        }
        public IEnumerable<SysLang> GetLangListFromCache()
        {
            return _cacheSvc.GetOrAdd("GamePlatform_SysLang", () =>
         {
             return GetList(null, "SortIndex asc");
         });
        }

        #region SysLang List/

        /// <summary>
        /// 从缓存异步获取语言数据字典
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetSelectListFromCache()
        {
            var list = GetLangListFromCache();
            if (list.Any())
            {
                return list.Select(c => new SelectListItem { Value = c.ResTag, Text = c.Name });
            }
            return null;
        }

        public void ClearCache()
        {
            _cacheSvc.Remove("SysLang");
        }

        #endregion

    }


}
