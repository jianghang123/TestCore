using Autofac;
using TestCore.Common;
using TestCore.Common.Helper;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Data.Dapper;
using TestCore.IRepository;
using TestCore.Common.Ioc;

namespace TestCore.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>, IDisposable
    {
        public IConnectionFactory ConnectionFactory = IoCBootstrapper.AutoContainer.Resolve<IConnectionFactory>();

        #region 同步方法
        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetList(object whereParam = null, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryList<TEntity>(whereParam, orderBy);
            }
        }


        public IEnumerable<TTable> GetList<TTable>(object whereParam, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryList<TTable>(whereParam, orderBy);
            }
        }

        public int GetCount<TTable>(object whereParam = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryCount<TTable>(whereParam);
            }
        }

        /// <summary>
        /// 查询单列的数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回类型，</typeparam>
        /// <param name="fieldName">查询的字段名称</param>
        /// <param name="whereParam">条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public IEnumerable<TReturn> GetSingleList<TReturn>(string fieldName, object whereParam = null, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QuerySingleList<TReturn, TEntity>(fieldName, whereParam, orderBy);
            }
        }


        /// <summary>
        /// 查询单列的数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IEnumerable<TReturn> GetSingleList<TReturn, TTable>(string fieldName, object whereParam = null, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QuerySingleList<TReturn, TTable>(fieldName, whereParam, orderBy);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 异步的方法,根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TSimple">返回类型， TSimple 所有属性（字段）必须包含在类型 TEntity属性（字段）中</typeparam>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IEnumerable<TSimple> GetSimpleList<TSimple, TTable>(object whereParam = null, string orderBy = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                return conn.QuerySimpleList<TSimple, TTable>(whereParam, orderBy);
            }
        }

        /// <summary>
        /// 查询简单类型（包含部分字段的）对象列表
        /// </summary>
        /// <typeparam name="TSimple"> 类型 TSimple 所有属性（字段）必须包含在类型 TEntity 中 </typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IEnumerable<TSimple> GetSimpleList<TSimple>(object whereParam = null, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QuerySimpleList<TSimple, TEntity>(whereParam, orderBy);
            }
        }

        /// <summary>
        /// 根据条件查询分页后的数据列表
        /// </summary>
        /// <param name="qpamams"></param>
        /// <returns></returns>
        public PageData<TEntity> GetList(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryList<TEntity>(qparams);
            }
        }

        /// <summary>
        /// 获取分页后的匿名类 
        /// </summary>
        /// <param name="qparams"></param>
        /// <returns></returns>
        public PageData<object> GetAnonymousList(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryAnonymousList<TEntity>(qparams);
            }
        }

        /// <summary>
        /// 根据条件查询分页后的数据列表
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TEntity 中 </typeparam>
        /// <param name="qparams"></param>
        /// <returns></returns>
        public PageData<TSimple> GetSimpleList<TSimple>(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryList<TSimple, TEntity>(qparams);
            }
        }

        /// <summary>
        /// 根据条件查询分页后的数据列表
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TEntity 中 </typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="qparams"></param>
        /// <returns></returns>
        public PageData<TSimple> GetSimpleList<TSimple, TTable>(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryList<TSimple, TTable>(qparams);
            }
        }

        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public TEntity GetModel(object whereParam, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryModel<TEntity>(whereParam, orderBy);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TEntity 中</typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public TSimple GetSimpleModel<TSimple>(object whereParam, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryModel<TSimple, TEntity>(string.Empty, whereParam, orderBy);
            }
        }

        /// <summary>
        /// 根据条件获取列和
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TReturn GetSum<TReturn, TTable>(string fieldName, object whereParam = null, IDbTransaction tran = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                return conn.Sum<TReturn, TTable>(fieldName, whereParam, tran);
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(TEntity entity, string expelFields = null)
        {
            try
            {
                using (var con = this.ConnectionFactory.OpenConnection())
                {
                    return con.Insert(entity, expelFields);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int InsertList(IEnumerable<TEntity> list, string expelFields = null, IDbTransaction tran = null)
        {
            try
            {
                using (var con = this.ConnectionFactory.OpenConnection())
                {
                    return con.InsertList(list, expelFields,tran);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long InsertWithReturnIdentity(TEntity entity, string expelFields = null)
        {
            try
            {
                using (var con = this.ConnectionFactory.OpenConnection())
                {
                    return con.InsertWithReturnIdentity(entity, expelFields);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 更新数据（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <param name="setParam"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public int Update(object setParam, object whereParam)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.Update<TEntity>(setParam, whereParam);
            }
        }

        /// <summary>
        /// 更新数据(原数据上加)（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <param name="mparams"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
       public int UpdateOnOld<TTable>(object mparams, object whereParam, object otherParam = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.UpdateOnOld<TTable>(mparams, whereParam, otherParam);
            }
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">要更新的实体</param>
        /// <param name="excludeColumns">不更新的列，多个使用逗号(,)分隔</param>
        /// <returns></returns>
        public int Update(TEntity model, string expelFields = null)
        {
            try
            {
                using (var con = this.ConnectionFactory.OpenConnection())
                {
                    return con.Update(model, expelFields);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public int Delete(object whereParam)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.Delete<TEntity>(whereParam);
            }
        }

        public IList<T> Query<T>(string sql, object param = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.Query<T>(sql, param).AsList();
            }
        }

        public TTable GetModel<TTable>(object whereParam, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return con.QueryModel<TTable>(whereParam, orderBy);
            }
        }



        #endregion


        #region 异步的方法

        #region GetListAsync

        /// <summary>
        /// 异步的方法,根据条件查询数据列表
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetListAsync(object whereParam = null, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                var list = await con.QueryListAsync<TEntity>(whereParam, orderBy);

                return list;
            }
        }

        /// <summary>
        /// 异步的方法,根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TSimple">返回类型， TSimple 所有属性（字段）必须包含在类型 TEntity属性（字段）中</typeparam>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TSimple>> GetSimpleListAsync<TSimple, TTable>(object whereParam = null, string orderBy = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                return await conn.QuerySimpleListAsync<TSimple, TTable>(whereParam, orderBy);
            }
        }


        public async Task<IEnumerable<TSimple>> GetSimpleListAsync<TSimple>(object whereParam = null, string orderBy = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                return await conn.QuerySimpleListAsync<TSimple, TEntity>(whereParam, orderBy);
            }
        }

        public async Task<IEnumerable<TTable>> GetListAsync<TTable>(object whereParam, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryListAsync<TTable>(whereParam, orderBy);
            }
        }


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
        public async Task<IEnumerable<TReturn>> GetSingleListAsync<TReturn>(string fieldName, object whereParam = null, string orderBy = null)
        {
            return await GetSingleListAsync<TReturn, TEntity>(fieldName, whereParam, orderBy);
        }

        /// <summary>
        /// 查询单列的数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TReturn>> GetSingleListAsync<TReturn, TTable>(string fieldName, object whereParam = null, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QuerySingleListAsync<TReturn, TTable>(fieldName, whereParam, orderBy);
            }
        }

        #endregion

        #region GetListAsync Page


        /// <summary>
        /// 异步的方法,根据条件查询分页后的数据列表
        /// </summary>
        /// <param name="qparams"></param>
        /// <returns></returns>
        public async Task<PageData<TEntity>> GetListAsync(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryListAsync<TEntity>(qparams);
            }
        }

        /// <summary>
        /// 异步的方法，根据条件查询数据
        /// </summary>
        /// <typeparam name="TTable">查询的数据表对应的类型</typeparam>
        /// <param name="qparams"></param>
        /// <returns></returns>
        public async Task<PageData<TTable>> GetListAsync<TTable>(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryListAsync<TTable>(qparams);
            }
        }

        /// <summary>
        /// 获取分页后的匿名类 
        /// </summary>
        /// <param name="qparams"></param>
        /// <returns></returns>
        public async Task<PageData<object>> GetAnonymousListAsync<TTable>(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryAnonymousListAsync<TTable>(qparams);
            }
        }

        /// <summary>
        /// 异步的方法,根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TSimple">类型 TSimple 所有属性（字段）必须包含在类型 TEntity属性（字段）中</typeparam>
        /// <param name="qparams"></param>
        /// <returns></returns>
        public async Task<PageData<TSimple>> GetSimpleListAsync<TSimple, TTable>(QueryParams qparams)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryListAsync<TSimple, TTable>(qparams);
            }
        }

        #endregion

        #region GetModelAsync

        /// <summary>
        /// 异步的方法,根据条件查询单个实体
        /// </summary>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<TEntity> GetModelAsync(object whereParam, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryModelAsync<TEntity>(whereParam, orderBy);
            }
        }


        public async Task<TTable> GetModelAsync<TTable>(object whereParam, string orderBy = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryModelAsync<TTable>(whereParam, orderBy);
            }
        }


        public async Task<TResult> GetModelAsync<TResult, TTable>(string fieldName, object whereParam, string orderBy = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                return await conn.QueryModelAsync<TResult,TTable>(fieldName, whereParam, orderBy);
            }
        }

        #endregion

        #region  GetMaxAsync  GetCountAsync

        /// <summary>
        /// 获取最大的值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<TReturn> GetMaxAsync<TReturn, TTable>(string fieldName, object whereParam = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryModelAsync<TReturn, TTable>(string.Format("MAX({0})", fieldName), whereParam);
            }
        }

        /// <summary>
        /// 获取最小的值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<TReturn> GetMinAsync<TReturn, TTable>(string fieldName, object whereParam = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryModelAsync<TReturn, TTable>(string.Format("MIN({0})", fieldName), whereParam);
            }
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync<TTable>(object whereParam = null, string distinctField = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.QueryCountAsync<TTable>(whereParam, distinctField);
            }
        }

        /// <summary>
        /// 根据条件获取列和
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TTable"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<TReturn> GetSumAsync<TReturn, TTable>(string fieldName, object whereParam = null, IDbTransaction tran = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                return await conn.SumAsync<TReturn, TTable>(fieldName, whereParam, tran);
            }
        }

        #endregion

        #region Insert

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(TEntity entity, string expelFields = null)
        {
            try
            {
                using (var con = this.ConnectionFactory.OpenConnection())
                {
                    return await con.InsertAsync(entity, expelFields);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<int> InsertListAsync(IEnumerable<TEntity> list, string expelFields = null, IDbTransaction tran = null)
        {
            try
            {
                using (var con = this.ConnectionFactory.OpenConnection())
                {
                    return await con.InsertListAsync(list, expelFields, tran);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> InsertWithReturnIdentityAsync(TEntity entity, string expelFields = null)
        {
            try
            {
                using (var con = this.ConnectionFactory.OpenConnection())
                {
                    return await con.InsertWithReturnIdentityAsync(entity, expelFields);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// 异步的方法,更新数据（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <param name="setParam"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(object setParam, object whereParam)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.UpdateAsync<TEntity>(setParam, whereParam);
            }
        }

        /// <summary>
        /// 异步的方法,更新数据（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <param name="setParam"></param>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(TEntity model, string expelFields = null)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.UpdateAsync(model, expelFields);
            }
        }

        #endregion

        #region DeleteAsync

        /// <summary>
        /// 异步的方法,根据条件删除数据
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(object whereParam)
        {
            using (var con = this.ConnectionFactory.OpenConnection())
            {
                return await con.DeleteAsync<TEntity>(whereParam);
            }
        }

        #endregion

        /// <summary>
        /// 查询的主键和名称等数据
        /// </summary>
        /// <typeparam name="TTable">数据库表对应的实体类型</typeparam>
        /// <param name="keyField"></param>
        /// <param name="nameField"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NameItem>> GetNameItemsAsync<TTable>(string keyField, string nameField, object whereParam = null, string orderBy = null)
        {
            return await GetNameItemsAsync(keyField, nameField, typeof(TTable).Name, whereParam, orderBy);
        }

        public async Task<IEnumerable<NameItem>> GetNameItemsAsync(string keyField, string nameField, string tableName, object whereParam = null, string orderBy = null, int count = 0)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = "ORDER BY " + orderBy;
            }
            string top = string.Empty;

            if (count > 0)
            {
                top = string.Format("top ({0})", count);
            }

            var whereAndParam = DapperExtension.GetWhereSqlAndParameters(whereParam);

            var sql = string.Format("select {0} [{1}] as Id ,[{2}] as Name from {3} where {4} {5}", top, keyField, nameField, tableName,
                whereAndParam.Item1, orderBy);

            using (var con = this.ConnectionFactory.OpenConnection())
            {
                var list = await con.QueryAsync<NameItem>(sql, whereAndParam.Item2);

                return list.Distinct();
            }
        }


        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync<TTable>(string valueField, string textField, object whereParam = null, string orderBy = null)
        {
            return await GetSelectListAsync(valueField, textField, typeof(TTable).Name, whereParam, orderBy);
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync(string valueField, string textField, string tableName, object whereParam = null, string orderBy = null)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = "ORDER BY " + orderBy;
            }

            var whereAndParam = DapperExtension.GetWhereSqlAndParameters(whereParam);

            var sql = string.Format("select [{0}] as Value ,[{1}] as Text from {2} where {3} {4}", valueField, textField, tableName,
               whereAndParam.Item1, orderBy);

            IEnumerable<SelectListItem> list = null;

            using (var con = this.ConnectionFactory.OpenConnection())
            {
                list = await con.QueryAsync<SelectListItem>(sql, whereAndParam.Item2);
            }
            return list;
        }





        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    //if (this.DbConnection != null)
                    //{
                    //    this.DbConnection.Close();
                    //    this.DbConnection.Dispose();
                    //}
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }


        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
