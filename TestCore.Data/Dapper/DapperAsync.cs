using TestCore.Common.Helper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace TestCore.Data.Dapper
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static partial class DapperExtension
    {

        #region query List




        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TEntity">返回列表的数据类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TEntity>> QueryListAsync<TEntity>(this IDbConnection conn, object whereParam = null,
            string orderBy = null, IDbTransaction tran = null)
        {
            return await QueryListAsync<TEntity>(conn, typeof(TEntity).Name, "*", whereParam, orderBy, tran);
        }


        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TReturn"> 返回的实体类型， 类型 TReturn 所有字段必须包含在 表 tableName的字段中</typeparam>
        /// <param name="conn"></param>
        /// <param name="tableName"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TReturn>> QuerySimpleListAsync<TReturn>(this IDbConnection conn, string tableName,
            object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        {
            return await QueryListAsync<TReturn>(conn, tableName, null, whereParam, orderBy, tran);
        }

        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回的实体类型， 类型 TReturn 所有属性（字段）必须包含在类型 TEntity属性（字段）中</typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TReturn>> QuerySimpleListAsync<TReturn, TEntity>(this IDbConnection conn, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        {
            return await QueryListAsync<TReturn>(conn, typeof(TEntity).Name, null, whereParam, orderBy, tran);
        }


        /// <summary>
        /// 根据条件查询单列数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回预定义原始类型 </typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TReturn>> QuerySingleListAsync<TReturn, TEntity>(this IDbConnection conn, string fieldName, object whereParam = null,
            string orderBy = null, IDbTransaction tran = null)
        {

            var type = typeof(TReturn);

            if (!type.IsPredefinedType())  /// 是否是原始预定义类型 如： int, datatime,string 等
            {
                throw new Exception("返回类型必须为原始类型");
            }
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new Exception("请指定查询的字段：fieldName");
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = string.Format("ORDER BY {0}", fieldName);
            }

            var whereAndParam = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("SELECT Distinct {0} FROM [{1}] {2} WHERE {3} {4}", fieldName, typeof(TEntity).Name, WITH_NO_LOCK,
                whereAndParam.Item1, orderBy).Trim();

            return await conn.QueryAsync<TReturn>(sql, whereAndParam.Item2, tran);

        }



        /// <summary>
        /// 异步的方法，根据条件查询数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型可以是预定义类型或者自定义类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldNames"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private static async Task<IEnumerable<TReturn>> QueryListAsync<TReturn>(this IDbConnection cnn, string tableName, string fieldNames,
           object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        {
            var type = typeof(TReturn);

            if (type.IsPredefinedType())  /// 是否是原始预定义类型 如： int, datatime,string 等
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new Exception("返回类型为原始类型时，请指定查询表名：tableName");
                }
                if (string.IsNullOrWhiteSpace(fieldNames))
                {
                    throw new Exception("返回类型为原始类型时，请指定查询字段：fieldNames");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    tableName = type.Name;
                }
                if (string.IsNullOrWhiteSpace(fieldNames))
                {
                    fieldNames = type.GetPropertyNames();
                }
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "ORDER BY " + orderBy;
            }

            var whereAndParam = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("SELECT {0} FROM [{1}] {2} WHERE {3} {4}", fieldNames, tableName, WITH_NO_LOCK,
                whereAndParam.Item1, orderBy).Trim();

            return await cnn.QueryAsync<TReturn>(sql, whereAndParam.Item2, tran);
        }


        #endregion

        #region query list 分页

        /// <summary>
        /// 异步的方法，查询分页数据
        /// </summary>
        /// <typeparam name="T">返回列表类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="pqp">分页查询参数</param>
        /// <param name="tran">事务</param>
        /// <returns></returns>
        public static async Task<PageData<TEntity>> QueryListAsync<TEntity>(this IDbConnection conn, QueryParams queryParams, IDbTransaction tran = null)
        {
            var returnType = typeof(TEntity);

            if (string.IsNullOrWhiteSpace(queryParams.TableName))
            {
                queryParams.TableName = returnType.Name;
            }
            if (returnType.IsPredefinedType())  /// 是原始预定义类型 如： int, datatime,string 等
            {
                if (queryParams.TableName.IsEquals(returnType.Name))
                {
                    throw new Exception("返回类型为原始类型时，请指定查询表名：TableName");
                }
                if (queryParams.FieldNames.Trim().IsEquals("*"))
                {
                    throw new Exception("返回类型为原始类型时，请指定查询字段：FieldNames");
                }
            }
            else
            {
                if (!queryParams.TableName.IsEquals(returnType.Name))
                {
                    if (queryParams.FieldNames.Trim().IsEquals("*"))
                    {
                        queryParams.FieldNames = returnType.GetPropertyNames();
                    }
                }
            }
            var Parameters = queryParams.ParamList.ToParameters();

            PageData<TEntity> pageData = new PageData<TEntity>();

            pageData.Total = await conn.ExecuteScalarAsync<int>(queryParams.CountSqlString, Parameters, tran);

            pageData.Result = await conn.QueryAsync<TEntity>(queryParams.ListSqlString, Parameters, tran);

            return pageData;
        }


        /// <summary>
        /// 异步的方法，根据条件查询数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型可以是预定义类型或者自定义类型</typeparam>
        /// <typeparam name="TEntity">查询的数据表对应的类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="queryParams"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<PageData<TReturn>> QueryListAsync<TReturn, TEntity>(this IDbConnection conn, QueryParams queryParams, IDbTransaction tran = null)
        {
            queryParams.TableName = typeof(TEntity).Name;

            return await QueryListAsync<TReturn>(conn, queryParams, tran);

        }


        /// <summary>
        /// 异步的方法， 查询分页数据,返回匿名实体列表
        /// </summary>
        /// <typeparam name="T">查询的表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="queryParams"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<PageData<object>> QueryAnonymousListAsync<T>(this IDbConnection cnn, QueryParams queryParams, IDbTransaction tran = null)
        {
            if (string.IsNullOrWhiteSpace(queryParams.TableName))
            {
                queryParams.TableName = typeof(T).Name;
            }
            PageData<object> pageData = new PageData<object>();

            DynamicParameters dparams = ToParameters(queryParams.ParamList);

            pageData.Total = await cnn.ExecuteScalarAsync<int>(queryParams.CountSqlString, dparams, tran);

            pageData.Result = await cnn.QueryAsync(queryParams.ListSqlString, dparams, tran);

            return pageData;
        }

        #endregion

        #region query Model

        /// <summary>
        /// 根据条件查询单条数据实体,或者某几列的数据
        /// </summary>
        /// <typeparam name="TRetrun">返回的实体类型,类型 TRetrun 所有属性（字段）必须包含在类型 TEntity 中</typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<TRetrun> QueryModelAsync<TRetrun, TEntity>(this IDbConnection conn, string fieldNames, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        {
            var tableName = typeof(TEntity).Name;

            if (string.IsNullOrWhiteSpace(fieldNames))
            {
                fieldNames = typeof(TRetrun).GetPropertyNames();
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "ORDER BY " + orderBy;
            }

            var sqlAndParams = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("SELECT TOP(1) {0} FROM [{1}] WITH(NOLOCK) WHERE {2} {3}", fieldNames, tableName,
                 sqlAndParams.Item1, orderBy).Trim();

            //conn.ExecuteScalarAsync

            return await conn.QueryFirstOrDefaultAsync<TRetrun>(sql, sqlAndParams.Item2, tran);
        }


        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tableName"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<TEntity> QueryModelAsync<TEntity>(this IDbConnection conn, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        {
            var tableName = typeof(TEntity).Name;

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "ORDER BY " + orderBy;
            }
            var sqlAndParams = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("SELECT TOP(1) * FROM [{0}] WITH(NOLOCK) WHERE {1} {2}", tableName,
                 sqlAndParams.Item1, orderBy).Trim();

            return await conn.QueryFirstOrDefaultAsync<TEntity>(sql, sqlAndParams.Item2, tran);
        }



        /// <summary>
        /// 根据条件查询数据实体
        /// </summary>
        /// <typeparam name="TRetrun">返回的实体类型,类型 TRetrun 所有属性（字段）必须包含在类型 TEntity 中</typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        //public static async Task<TRetrun> QueryModelAsync<TRetrun, TEntity>(this IDbConnection conn, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        //{
        //    var tableName = typeof(TEntity).Name;

        //    if (!string.IsNullOrWhiteSpace(orderBy))
        //    {
        //        orderBy = "ORDER BY " + orderBy;
        //    }

        //    var sqlAndParams = GetWhereSqlAndParameters(whereParam);

        //    string sql = string.Format("SELECT TOP(1) {0} FROM [{1}] WITH(NOLOCK) WHERE {2} {3}", fieldNames, tableName,
        //         sqlAndParams.Item1, orderBy).Trim();


        //    //conn.ExecuteScalarAsync

        //    return await conn.QueryFirstOrDefaultAsync<TRetrun>(sql, sqlAndParams.Item2, tran);
        //}


        #endregion

        #region QueryCount

        /// <summary>
        ///异步的方法， 查询数据行数
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<int> QueryCountAsync<TEntity>(this IDbConnection cnn, object whereParam = null, string distinctField = null, IDbTransaction tran = null)
        {
            var tableName = typeof(TEntity).Name;

            var sqlAndParams = GetWhereSqlAndParameters(whereParam);

            var countField = "1";

            if (!string.IsNullOrWhiteSpace(distinctField))
            {
                countField = string.Format("DISTINCT {0}", distinctField);
            }
            string sql = string.Format("SELECT COUNT({0}) FROM [{1}] WITH(NOLOCK) WHERE {2}", countField, tableName, sqlAndParams.Item1).Trim();

            return await cnn.ExecuteScalarAsync<int>(sql, sqlAndParams.Item2, tran);
        }

        public static async Task<TReturn> SumAsync<TReturn, TEntity>(this IDbConnection cnn, string fieldName, object whereParam = null, IDbTransaction tran = null)
        {
            var tableName = typeof(TEntity).Name;

            var whereAndParams = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("SELECT Sum({0}) FROM [{1}] WITH(NOLOCK) WHERE {2}", fieldName, tableName, whereAndParams.Item1);

            return await cnn.ExecuteScalarAsync<TReturn>(sql, whereAndParams.Item2, tran);
        }

        #endregion

        /// <summary>
        /// 查詢是否存在記錄
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<bool> ExitsAsync<TEntity>(this IDbConnection cnn, object whereParam = null, IDbTransaction tran = null)
        {
            var count = await QueryCountAsync<TEntity>(cnn, whereParam, null, tran);

            return count > 0;
        }

        #region insert

        /// <summary>
        ///异步的方法， 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="param"></param>
        /// <param name="tran"></param>
        /// <param name="excludeColumns">不包含的列，多个使用逗号(,)分隔</param>
        /// <returns></returns>
        public static async Task<int> InsertAsync<TEntity>(this IDbConnection cnn, TEntity model, string expelFields = null, IDbTransaction tran = null)
        {
            CommandDefinition cmd = GetInsertCommand<TEntity>(model, expelFields, tran);

            return await cnn.ExecuteAsync(cmd);
        }

        /// <summary>
        /// 异步的方法， 批量插入数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="list"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<int> InsertListAsync<TEntity>(this IDbConnection cnn, IEnumerable<TEntity> list, string expelFields = null, IDbTransaction tran = null)
        {
            CommandDefinition cmd = GetInsertListCommand<TEntity>(list, expelFields, tran);

            return await cnn.ExecuteAsync(cmd);
        }


        /// <summary>
        /// 插入数据 并返回自增长列的数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="model"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<long> InsertWithReturnIdentityAsync<TEntity>(this IDbConnection cnn, TEntity model, string expelFields = null, IDbTransaction tran = null)
        {
            CommandDefinition cd = GetInsertCommand<TEntity>(model, expelFields, tran, true);

            return await cnn.ExecuteScalarAsync<long>(cd);
        }


        #endregion

        #region update

        /// <summary>
        ///异步的方法， 更新数据（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        public static async Task<int> UpdateAsync<T>(this IDbConnection cnn, object setParam, object whereParam, IDbTransaction tran = null)
        {
            return await UpdateAsync(cnn, typeof(T), setParam, whereParam, tran);
        }

        public static async Task<int> UpdateAsync(this IDbConnection cnn, Type type, object setParam, object whereParam, IDbTransaction tran = null)
        {
            try
            {
                if (whereParam == null || !whereParam.HasParameters())
                {
                    throw new Exception("更新数据必须带有条件参数");
                }
                CommandDefinition cmd = GetUpdateCommand(type, setParam, whereParam, null, tran);

                int row = await cnn.ExecuteAsync(cmd);

                return row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static async Task<int> UpdateAsync<T>(this IDbConnection cnn, T model, string expelFields = null, IDbTransaction tran = null)
        {
            try
            {
                CommandDefinition cmd = GetUpdateCommand<T>(model, null, expelFields, tran);

                int row = await cnn.ExecuteAsync(cmd);

                return row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入或者更新数据，存在则更新，不存在则插入，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="model"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static async Task<int> InsertOrUpdateAsync<T>(this IDbConnection conn, T model, object whereParam, IDbTransaction tran = null)
        {
            try
            {
                if (whereParam == null)
                {
                    throw new Exception("更新数据没有条件参数");
                }
                int row = await QueryCountAsync<T>(conn, whereParam, null, tran);

                if (row <= 0)
                {
                    return await InsertAsync<T>(conn, model, null, tran);
                }
                else
                {
                    return await UpdateAsync<T>(conn, model, null, tran);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region delete

        /// <summary>
        /// 异步的方法，根据条件删除数据
        /// </summary>
        public static async Task<int> DeleteAsync<T>(this IDbConnection cnn, object whereParam, IDbTransaction transacton = null)
        {
            if (whereParam == null || !whereParam.HasParameters())
            {
                throw new Exception("删除数据必须带有条件参数");
            }
            var sqlAndParams = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("DELETE FROM [{0}] WHERE {1}", typeof(T).Name, sqlAndParams.Item1);

            return await cnn.ExecuteAsync(sql, sqlAndParams.Item2, transacton);
        }

        #endregion

        /// <summary>
        ///异步的方法， 插入一行数据，并返回新行指定的字段值。
        /// </summary>
        //public static async Task<TOutput> InsertWithOutputAsync<TOutput, TEntity>(this IDbConnection cnn, object param, string outputColumn = null, IDbTransaction tran = null)
        //{
        //    var type = typeof(TEntity);

        //    var names = GetFieldsNames(type, param, false);

        //    string sql = string.Format("INSERT INTO {0} ({1}) OUTPUT INSERTED.{2} VALUES(@{3})", typeof(TEntity).Name, string.Join(",", names), outputColumn, string.Join(",@", names));

        //    var list = await cnn.QueryAsync<TOutput>(sql, param, tran);

        //    return list.FirstOrDefault();
        //}
    }
}