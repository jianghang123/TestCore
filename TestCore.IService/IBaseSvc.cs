using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;

namespace TestCore.IService
{
    public interface IBaseSvc<TEntity> : ISingletonDependency
    {
        #region 同步的方法

        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetList(object whereParam = null, string orderBy = null);


        /// <summary>
        ///  获取任意的数据的实体列表
        /// </summary>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<TTable> GetList<TTable>(object whereParam, string orderBy = null);


        /// <summary>
        /// 查询数量
        /// </summary>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        int GetCount<TTable>(object whereParam = null);

        /// <summary>
        /// 查询简单类型（包含部分字段的）对象列表
        /// </summary>
        /// <typeparam name="TSimple"> 类型 TSimple 所有属性（字段）必须包含在类型 TEntity 中 </typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<TSimple> GetSimpleList<TSimple>(object whereParam = null, string orderBy = null);

        /// <summary>
        /// 根据条件查询数据列表  20171120 新增
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TTable（字段）中</typeparam>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<TSimple> GetSimpleList<TSimple, TTable>(object whereParam = null, string orderBy = null);


        /// <summary>
        /// 查询单列的数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回类型，</typeparam>
        /// <param name="fieldName">查询的字段名称</param>
        /// <param name="whereParam">条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        IEnumerable<TReturn> GetSingleList<TReturn>(string fieldName, object whereParam = null, string orderBy = null);

        /// <summary>
        /// 查询单列的数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回类型，</typeparam>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="fieldName">查询的字段名称</param>
        /// <param name="whereParam">条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        IEnumerable<TReturn> GetSingleList<TReturn, TTable>(string fieldName, object whereParam = null, string orderBy = null);


        #region page



        /// <summary>
        /// 根据条件查询分页后的数据列表
        /// </summary>
        /// <param name="qpamams"></param>
        /// <returns></returns>
        PageData<TEntity> GetList(QueryParams qparams);


        /// <summary>
        /// 获取分页后的匿名类 
        /// </summary>
        /// <param name="qparams"></param>
        /// <returns></returns>
        PageData<object> GetAnonymousList(QueryParams qparams);



        /// <summary>
        /// 根据条件查询分页后的数据列表
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TEntity 中 </typeparam>
        /// <param name="qparams"></param>
        /// <returns></returns>
        PageData<TSimple> GetSimpleList<TSimple>(QueryParams qparams);

        #endregion


        #region GetModel

        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        TEntity GetModel(object whereParam, string orderBy = null);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TEntity 中</typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        TSimple GetSimpleModel<TSimple>(object whereParam, string orderBy = null);


        /// <summary>
        ///  获取任意的数据的实体
        /// </summary>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        TTable GetModel<TTable>(object whereParam, string orderBy = null);

        #endregion


        #region  GetCountAsync  GetMaxAsync

        /// <summary>
        /// 根据条件获取列和
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        TReturn GetSum<TReturn, TTable>(string fieldName, object whereParam = null, IDbTransaction tran = null);

        #endregion

        #region  Insert Update Delete

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(TEntity entity, string expelFields = null);


        long InsertWithReturnIdentity(TEntity entity, string expelFields = null);

        /// <summary>
        /// 更新数据（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <param name="setParam"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        int Update(object setParam, object whereParam);

        /// <summary>
        /// 更新数据(原数据上加)（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <param name="mparams"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        int UpdateOnOld<TTable>(object mparams, object whereParam, object otherParam = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">要更新的实体</param>
        /// <param name="excludeColumns">不更新的列，多个使用逗号(,)分隔</param>
        /// <returns></returns>
        int Update(TEntity model, string expelFields = null);

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        int Delete(object whereParam);

        #endregion

        #endregion

        #region 异步的方法

        #region GetListAsync

        /// <summary>
        /// 异步的方法,根据条件查询数据列表
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetListAsync(object whereParam = null, string orderBy = null);

        /// <summary>
        ///  获取任意的数据的实体列表
        /// </summary>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<IEnumerable<TTable>> GetListAsync<TTable>(object whereParam, string orderBy = null);


        /// <summary>
        /// 异步的方法,根据条件查询数据列表  20170907 新增
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TTable（字段）中</typeparam>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<IEnumerable<TSimple>> GetSimpleListAsync<TSimple, TTable>(object whereParam = null, string orderBy = null);


        Task<IEnumerable<TSimple>> GetSimpleListAsync<TSimple>(object whereParam = null, string orderBy = null);

        #endregion

        #region GetSingleListAsync

        /// <summary>
        /// 查询单列的数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回类型，</typeparam>
        /// <param name="fieldName">查询的字段名称</param>
        /// <param name="whereParam">条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        Task<IEnumerable<TReturn>> GetSingleListAsync<TReturn>(string fieldName, object whereParam = null, string orderBy = null);



        /// <summary>
        /// 查询单列的数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回类型，</typeparam>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="fieldName">查询的字段名称</param>
        /// <param name="whereParam">条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        Task<IEnumerable<TReturn>> GetSingleListAsync<TReturn, TTable>(string fieldName, object whereParam = null, string orderBy = null);


        #endregion

        #region GetList Page


        Task<PageData<TTable>> GetListAsync<TTable>(QueryParams qparams);

        /// <summary>
        /// 异步的方法,根据条件查询分页后的数据列表
        /// </summary>
        /// <param name="qparams"></param>
        /// <returns></returns>
        Task<PageData<TEntity>> GetListAsync(QueryParams qparams);


        /// <summary>
        /// 获取分页后的匿名类 
        /// </summary>
        /// <param name="qparams"></param>
        /// <returns></returns>
        Task<PageData<object>> GetAnonymousListAsync<TTable>(QueryParams qparams);


        /// <summary>
        /// 异步的方法,根据条件查询数据列表(类型 TSimple 所有属性（字段）必须包含在类型 TEntity属性（字段）中)
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TEntity属性（字段）中</typeparam>
        /// <param name="qparams"></param>
        /// <returns></returns>
        Task<PageData<TSimple>> GetSimpleListAsync<TSimple, TTable>(QueryParams qparams);


        #endregion

        #region GetModelAsync

        /// <summary>
        /// 异步的方法,根据条件查询单个实体
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<TEntity> GetModelAsync(object whereParam, string orderBy = null);


        /// <summary>
        ///  获取任意的数据的实体
        /// </summary>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<TTable> GetModelAsync<TTable>(object whereParam, string orderBy = null);



        Task<TResult> GetModelAsync<TResult, TTable>(string fieldName, object whereParam, string orderBy = null);

        #endregion

        #region  GetCountAsync  GetMaxAsync

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<TReturn> GetMaxAsync<TReturn, TTable>(string fieldName, object whereParam = null);

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<TReturn> GetMinAsync<TReturn, TTable>(string fieldName, object whereParam = null);


        /// <summary>
        /// 根据条件获取数量
        /// </summary>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="whereParam">條件參數</param>
        /// <param name="distinctField">取消重複的字段</param>
        /// <returns></returns>
        Task<int> GetCountAsync<TTable>(object whereParam = null, string distinctField = null);

        /// <summary>
        /// 根据条件获取列和
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        Task<TReturn> GetSumAsync<TReturn, TTable>(string fieldName, object whereParam = null, IDbTransaction tran = null);


        #endregion

        #region Insert
        /// <summary>
        ///异步的方法, 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(TEntity entity, string expelFields = null);


        Task<long> InsertWithReturnIdentityAsync(TEntity entity, string expelFields = null);

        #endregion

        #region UpdateAsync
        /// <summary>
        /// 异步的方法,更新数据（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <param name="setParam"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(object setParam, object whereParam);


        Task<int> UpdateAsync(TEntity model, string expelFields = null);

        #endregion

        #region DeleteAsync

        /// <summary>
        /// 异步的方法,根据条件删除数据
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(object whereParam);


        #endregion

        #region selectListItem

        /// <summary>
        /// 查询的主键和名称等数据
        /// </summary>
        /// <typeparam name="TTable">数据库表对应的实体类型</typeparam>
        /// <param name="keyField"></param>
        /// <param name="nameField"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<IEnumerable<NameItem>> GetNameItemsAsync<TTable>(string keyField, string nameField, object whereParam = null, string orderBy = null);

        Task<IEnumerable<NameItem>> GetNameItemsAsync(string keyField, string nameField, string tableName, object whereParam = null, string orderBy = null, int count = 0);

        Task<IEnumerable<SelectListItem>> GetSelectListAsync<TTable>(string valueField, string textField, object whereParam = null, string orderBy = null);

        Task<IEnumerable<SelectListItem>> GetSelectListAsync(string valueField, string textField, string tableName, object whereParam = null, string orderBy = null);

        #endregion

        #endregion
    }
}
