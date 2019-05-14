using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCore.Common.Helper;
using TestCore.Domain.SysEntity;

namespace TestCore.IRepository
{
    public interface IBaseResourceRepository<TEntity> : IRepository<TEntity>
    {

             
        /// <summary>
        /// 获取当前语言环境的实体类数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        Task<TEntity> GetModelAsync(int id, string fieldName = null, string lang = null);

        /// <summary>
        /// 插入数据,同时插入资源 SysResource 表，清除缓存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tableId"></param>
        /// <returns></returns>
        Task<int> InsertWithResourceAsync(TEntity entity, List<SysResource> list, int tableId);


        /// <summary>
        /// 插入数据,同时插入资源 SysResource 表，清除缓存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldNames"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<int> InsertWithResourceAsync(TEntity entity, string fieldNames);


        /// <summary>
        /// 异步的方法,根据条件删除数据,同时会删除 SysResource 数据，清除缓存 
        /// </summary>
        /// <param name="ids">主键Id</param>
        /// <returns></returns>
        Task<int> DeleteWithResourceAsync(string[] ids, int tableId = 0);


        /// <summary>
        /// 更新数据，同时更新 数据字典  SysResource，清除缓存 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="expelFields"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<int> UpdateWithResourceAsync(TEntity model, string expelFields = null, SysResource resource = null);

        /// <summary>
        /// 更新数据状态，同时更新 数据字典  SysResource，清除缓存 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkId"></param>
        /// <returns></returns>
        Task<int> UpdateStatusWithResourceAsync(int status, int tableId, string[] pkId);

        /// <summary>
        /// 交换排序,上升或者下降排序 同时更新 数据字典  SysResource，清除缓存   
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="sortIndex"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        Task<int> ChangeSortIndexAsync(Type type, int id, int sortIndex, int sortType, Condition cond = null, bool updateResource = true);




    }
}
