using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Common;
using TestCore.Common.Cache;
using TestCore.Common.Helper;
using TestCore.Data.Dapper;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;
using TestCore.Domain.SysEntity;
using TestCore.IRepository.SysAdmin;
using TestCore.Repositories;

namespace Caiba.Repositories.Sys
{
    public class ResourceRepository : BaseRepository<SysResource>, IResourceRepository
    {

        protected readonly ICacheService _cacheSvc;
        protected readonly ILangRepository _langRepos;

        public ResourceRepository(ILangRepository langRepos, ICacheService cacheSvc)
        {
            _langRepos = langRepos;
            _cacheSvc = cacheSvc;
        }


        #region from database
        /// <summary>
        /// 从数据库异步获取资源的数据字典
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysResource>> GetListAsync(TableEnum table, string fieldName = null, string lang = null)
        {
            return await GetListAsync((int)table, fieldName, lang);
        }

        /// <summary>
        /// 从数据库异步获取资源的数据字典
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysResource>> GetListAsync(int tableId, string fieldName = null, string lang = null)
        {
            if (string.IsNullOrEmpty(lang))
            {
                lang = CoreHttpContext.CurrentCulture.Name;
            }
            return await GetListAsync(new { tableId, Status = 1, fieldName, lang },  "tableId asc, lang asc,sortIndex asc");
        }


        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync(int tableId, string fieldName = null, int[] pkids = null, string lang = null)
        {
            if (string.IsNullOrEmpty(lang))
            {
                lang = CoreHttpContext.CurrentCulture.Name;
            }
            return await GetSelectListAsync<SysResource>(nameof(SysResource.PkId), nameof(SysResource.ResValue), 
                new { tableId, Status=1, fieldName, lang, pkId = pkids }, "tableId asc,lang asc,sortIndex asc");
        }

        #endregion

        #region From Cache
        /// <summary>
        /// 从缓存异步获取资源数据字典
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public IEnumerable<SimpleResource> GetListFromCache(int tableId, string fieldName = null, int[] pkids = null, string lang = null)
        {
            if (string.IsNullOrEmpty(lang))
            {
                lang = CoreHttpContext.CurrentCulture.Name;
            }
            var key = CommonConsts.GetCacheKey_Resource(tableId, lang);

            var list = _cacheSvc.GetOrAdd(key, () =>
                 {
                     return  GetSimpleListAsync<SimpleResource,SysResource>(new { tableId, lang, Status = 1 }, "tableId asc,SortIndex asc,PKId asc").Result;
                 });
            if (!string.IsNullOrEmpty(fieldName))
            {
                var names = fieldName.Split(',');

                list = list.Where(c => names.IsContains(c.FieldName));
            }
            if (pkids != null && pkids.Any())
            {
                list = list.Where(c => pkids.Contains(c.PkId));
            }
            return list;
        }

        public IEnumerable<SelectListItem> GetSelectListFromCache(int tableId, string fieldName, int[] pkids = null, string lang = null)
        {
            var list = GetListFromCache(tableId, fieldName, pkids, lang);

            return list.Select(c => new SelectListItem { Value = c.PkId.ToString(), Text = c.ResValue });
        }

        public string GetResourceFromCache(int tableId, string fieldName, int pkid, string lang = null)
        {
            var list = GetListFromCache(tableId, fieldName, new int[] { pkid }, lang);

            return list.Select(c => c.ResValue).FirstOrDefault();
        }

        #endregion


        /// <summary>
        /// 批量更新和插入数据
        /// </summary>
        /// <param name="del_rid"></param>
        /// <param name="insertList"></param>
        /// <returns></returns>
        public async Task<int> UpdateListAsync(int tableId, string fieldName, int[] del_ids, IEnumerable<SysResource> insertList)
        {
            ///if ((del_ids == null || !del_ids.Any()) && !insertList.Any()) return 0;

            using (var conn = ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                int row = 0;

                var tableName = ((TableEnum)tableId).ToString(); 

                var indexList = await conn.QuerySimpleListAsync<SimpleIndex>(tableName,  null, null, tran);

                var pkIds = await conn.QuerySingleListAsync<int, SysResource>( nameof(SysResource.PkId), new { tableId, fieldName },null,tran);

                var ids = indexList.Select(c => c.Id).ToList();

                ///需要删除的资源
                var delPkIds = pkIds.Where(c => !ids.Contains(c));
                if(delPkIds.Any())
                {
                    row = await conn.DeleteAsync<SysResource>( new { tableId, fieldName, PkId = delPkIds.ToArray() }, tran);
                }
                if (del_ids.Any())
                {
                    row = await conn.DeleteAsync<SysResource>(new { id = del_ids }, tran);
                }
                if(insertList.Any())
                {
                    row += await conn.InsertListAsync(insertList, null, tran);
                }

                foreach (var item in indexList)
                {
                    row += await conn.UpdateAsync<SysResource>(new { item.SortIndex, item.Status }, new { tableId, fieldName, PKId = item.Id }, tran);
                }
                tran.Commit();

                if(row > 0)  ///修改语言资源后 删除资源缓存
                {
                    ClearResourceCache(tableId);
                }
                return row;
            }
        }

        public void ClearResourceCache(int tableId, string lang = null)
        {
            string key = null;
            if (!string.IsNullOrEmpty(lang))
            {
                key = CommonConsts.GetCacheKey_Resource(tableId, lang);
                _cacheSvc.Remove(key);
            }
            else
            {
                var langList = this._langRepos.GetSelectListFromCache();
                foreach (var item in langList)
                {
                    key = CommonConsts.GetCacheKey_Resource(tableId, item.Value);
                    _cacheSvc.Remove(key);
                }
            }
        }

        public void ClearLangCache( )
        {
             this._langRepos.ClearCache();
        }

    }
}



