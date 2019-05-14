using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCore.Common.Ioc;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;
using TestCore.Domain.SysEntity;
using TestCore.IRepository;

namespace TestCore.IRepository.SysAdmin
{
    public interface IResourceRepository : IRepository<SysResource>, ISingletonDependency
    {
        ///// <summary>
        ///// 根据语系查询对应列的翻译
        ///// </summary>
        ///// <param name="lang"></param>
        ///// <param name="tableName"></param>
        ///// <param name="fieldName"></param>
        ///// <returns></returns>
        //Dictionary<int, string> GetResourcesFromCache(string lang, TableEnum tableName, string fieldName);
        ///// <summary>
        /////  根据语系，pkId查询对应列的翻译
        ///// </summary>
        ///// <param name="lang"></param>
        ///// <param name="tableName"></param>
        ///// <param name="fieldName"></param>
        ///// <param name="pkId"></param>
        ///// <returns></returns>
        //string GetResourceFromCache(string lang, TableEnum tableName, string fieldName,int pkId);

        /// <summary>
        /// 从数据库异步获取资源的数据字典
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        Task<IEnumerable<SysResource>> GetListAsync(int tableId, string fieldName = null, string lang = null);

        /// <summary>
        /// 从数据库异步获取资源的数据字典
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SysResource>> GetListAsync(TableEnum table, string fieldName = null, string lang = null);


        Task<IEnumerable<SelectListItem>> GetSelectListAsync(int tableId, string fieldName = null, int[] pkids = null, string lang = null);

        #region  SimpleResource


        /// <summary>
        /// 从缓存异步获取资源数据字典
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        IEnumerable<SimpleResource> GetListFromCache(int tableId, string fieldName = null, int[] pkids = null, string lang = null);
    
        /// <summary>
        /// 异步的,从缓存获取资源数据字典
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkids"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetSelectListFromCache(int tableId, string fieldName = null, int[] pkids = null, string lang = null);

        /// <summary>
        /// 异步的,从缓存获取资源值
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        string GetResourceFromCache(int tableId, string fieldName, int pkid, string lang = null);


        /// <summary>
        /// 批量更新和插入数据
        /// </summary>
        /// <param name="del_ids">修改过的数据Id list</param>
        /// <param name="insertList"></param>
        /// <returns></returns>
        Task<int> UpdateListAsync(int tableId, string fieldName, int[] del_ids, IEnumerable<SysResource> insertList);

        /// <summary>
        /// 清除数据资源的缓存
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        void ClearResourceCache(int tableId, string lang = null);

        /// <summary>
        /// 清除语言列表缓存
        /// </summary>
        /// <returns></returns>
        void  ClearLangCache();

        #endregion
    }
}
