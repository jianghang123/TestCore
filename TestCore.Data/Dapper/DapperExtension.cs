using TestCore.Common.Helper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TestCore.Data.Dapper
{
    /// <summary>
    /// Dapper 扩展
    /// </summary>
    public static partial class DapperExtension
    {

        #region  helpers

        /// <summary>
        /// 使用 WITH(NOLOCK)，如果不使用，将此值设置为空字符串
        /// </summary>
        private static string WITH_NO_LOCK = "WITH(NOLOCK)";

        /// <summary>
        /// 追加参数
        /// </summary>
        /// <param name="dparam"></param>
        /// <param name="parms"></param>
        public static void AppendParameters(this DynamicParameters dparam, object parms)
        {
            if (parms == null)
            {
                return;
            }
            if (parms is DynamicParameters)
            {
                var p = (DynamicParameters)parms;

                if (!p.ParameterNames.Any())
                {
                    return;
                }
                foreach (var name in p.ParameterNames)
                {
                    var val = p.Get<object>(name);
                    if (val == null)
                    {
                        continue;
                    }
                    dparam.Add(name, val);
                }
            }
            else if (parms is Condition)
            {
                var cond = (Condition)parms;
                if (!cond.ParamList.Any())
                {
                    return;
                }
                foreach (var item in cond.ParamList)
                {
                    if (item.Value != null)
                    {
                        dparam.Add(item.Key, item.Value);
                    }
                }
            }
            else
            {
                var pros = parms.GetType().GetProperties();

                if (!pros.Any()) return;

                foreach (var p in pros)
                {
                    var val = p.GetValue(parms);

                    if (val == null) continue;

                    dparam.Add(p.Name, val);
                }
            }
        }


        /// <summary>
        /// 根据条件获取条件语句和条件参数
        /// </summary>
        /// <param name="conditon"></param>
        /// <returns></returns>
        public static Tuple<string, DynamicParameters> GetWhereSqlAndParameters(this object conditon)
        {
            string where = "1=1";

            DynamicParameters dparam = new DynamicParameters();

            var sqlAndParamts = Tuple.Create(where, dparam);

            if (conditon == null)
            {
                return sqlAndParamts;
            }
            var items = new List<string>();

            if (conditon is DynamicParameters)
            {
                dparam = (DynamicParameters)conditon;

                if (!dparam.ParameterNames.Any())
                {
                    return sqlAndParamts;
                }
                foreach (var name in dparam.ParameterNames)
                {
                    var val = dparam.Get<object>(name);

                    if (val == null)
                    {
                        continue;
                    }
                    if (val.GetType().IsArray)
                    {
                        items.Add(string.Format("[{0}] IN @{0}", name));
                    }
                    else
                    {
                        items.Add(string.Format("[{0}]=@{0}", name));
                    }
                }
                where = string.Join(" AND ", items);
            }
            else if (conditon is Condition)
            {
                var cond = (Condition)conditon;
                if (!cond.ParamList.Any())
                {
                    return sqlAndParamts;
                }
                dparam = ToParameters(cond.ParamList);
                where = cond.WhereString;
            }
            else
            {
                var pros = conditon.GetType().GetProperties().Where(p => p.GetValue(conditon) != null);

                if (!pros.Any()) return sqlAndParamts;

                foreach (var p in pros)
                {
                    var val = p.GetValue(conditon);

                    if (p.PropertyType.IsArray)
                    {
                        items.Add(string.Format("[{0}] IN @{0}", p.Name));
                    }
                    else
                    {
                        items.Add(string.Format("[{0}]=@{0}", p.Name));
                    }
                    dparam.Add(p.Name, val);
                }
                where = string.Join(" AND ", items);
            }
            return Tuple.Create(where, dparam);
        }


        public static DynamicParameters ToParameters(this Dictionary<string, object> parms)
        {
            DynamicParameters param = new DynamicParameters();

            foreach (var item in parms)
            {
                if (item.Value != null)
                {
                    param.Add(item.Key, item.Value);
                }
            }
            return param;
        }

        /// <summary>
        /// 是否有属性或参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool HasParameters(this object param)
        {
            if (param == null)
                return false;
            if (param is DynamicParameters)
            {
                return ((DynamicParameters)param).ParameterNames.Any();
            }
            else if (param is Condition)
            {
                return ((Condition)param).ParamList.Any();
            }
            return param.GetType().GetProperties().Where(p => p.GetValue(param) != null).Any();
        }

        #endregion

        #region query List


        /// <summary>
        /// 根据条件查询单列数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回预定义原始类型 </typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="fieldNames">查询的字段</param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> QuerySingleList<TReturn, TEntity>(this IDbConnection conn, string fieldNames, object whereParam = null,
            string orderBy = null, IDbTransaction tran = null)
        {
            return QueryList<TReturn>(conn, typeof(TEntity).Name, fieldNames, whereParam, orderBy, tran);
        }


        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回的实体类型</typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> QuerySimpleList<TReturn, TEntity>(this IDbConnection conn, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        {
            return QueryList<TReturn>(conn, typeof(TEntity).Name, null, whereParam, orderBy, tran);
        }

        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TEntity">返回列表的数据类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> QueryList<TEntity>(this IDbConnection conn, object whereParam = null,
            string orderBy = null, IDbTransaction tran = null)
        {
            return QueryList<TEntity>(conn, typeof(TEntity).Name, "*", whereParam, orderBy, tran);
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
        private static IEnumerable<TReturn> QueryList<TReturn>(this IDbConnection cnn, string tableName, string fieldNames,
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

            return cnn.Query<TReturn>(sql, whereAndParam.Item2, tran);
        }


        #endregion

        #region query list 分页

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <typeparam name="T">返回列表类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="pqp">分页查询参数</param>
        /// <param name="tran">事务</param>
        /// <returns></returns>
        public static PageData<TEntity> QueryList<TEntity>(this IDbConnection conn, QueryParams queryParams, IDbTransaction tran = null)
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
                if(!queryParams.TableName.IsEquals(returnType.Name))
                {
                    if (queryParams.FieldNames.Trim().IsEquals("*"))
                    {
                        queryParams.FieldNames = returnType.GetPropertyNames();
                    }
                }
            }
            var Parameters = queryParams.ParamList.ToParameters();

            PageData<TEntity> pageData = new PageData<TEntity>();

            pageData.Total = conn.Query<int>(queryParams.CountSqlString, Parameters, tran).FirstOrDefault();

            pageData.Result = conn.Query<TEntity>(queryParams.ListSqlString, Parameters, tran);

            return pageData;

        }


        /// <summary>
        /// 根据条件查询数据列表
        /// </summary>
        /// <typeparam name="TReturn">返回的实体类型</typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="conn"></param>
        /// <param name="queryParams"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static PageData<TReturn> QueryList<TReturn, TEntity>(this IDbConnection conn, QueryParams queryParams, IDbTransaction tran = null)
        {
            queryParams.TableName = typeof(TEntity).Name;

            return QueryList<TReturn>(conn, queryParams, tran);

        }


        /// <summary>
        ///  查询分页数据,返回匿名实体列表  
        /// </summary>
        /// <typeparam name="T">查询的表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="queryParams"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static PageData<object> QueryAnonymousList<T>(this IDbConnection cnn, QueryParams queryParams, IDbTransaction tran = null)
        {
            if (string.IsNullOrWhiteSpace(queryParams.TableName))
            {
                queryParams.TableName = typeof(T).Name;
            }
            PageData<object> pageData = new PageData<object>();

            var Parameters = queryParams.ParamList.ToParameters();

            pageData.Total = cnn.Query<int>(queryParams.CountSqlString, Parameters, tran).Single();

            pageData.Result = cnn.Query(queryParams.ListSqlString, Parameters, tran);

            return pageData;
        }

        #endregion

        #region QueryCount

        /// <summary>
        /// 查询数据行数
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int QueryCount<TEntity>(this IDbConnection cnn, object whereParam = null, IDbTransaction tran = null)
        {
            var tableName = typeof(TEntity).Name;

            var whereAndParams = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("SELECT COUNT(1) FROM [{0}] WITH(NOLOCK) WHERE {1}", tableName, whereAndParams.Item1);

            return cnn.ExecuteScalar<int>(sql, whereAndParams.Item2, tran);
        }

        public static TReturn Sum<TReturn,TEntity>(this IDbConnection cnn, string fieldName, object whereParam = null, IDbTransaction tran = null)
        {
            var tableName = typeof(TEntity).Name;

            var whereAndParams = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("SELECT Sum({0}) FROM [{1}] WITH(NOLOCK) WHERE {2}", fieldName, tableName, whereAndParams.Item1);

            return cnn.ExecuteScalar<TReturn>(sql, whereAndParams.Item2, tran);
        }

        #endregion

        #region query Model


        /// <summary>
        /// 根据条件查询数据实体,或者某一列或几列的数据
        /// </summary>
        /// <typeparam name="TRetrun">返回的实体类型,类型 TRetrun 所有属性（字段）必须包含在类型 TEntity 中，如果返回的单列，那么该类型应为原始类型</typeparam>
        /// <typeparam name="TEntity">要查询的数据表对应的类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="whereParam"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static TRetrun QueryModel<TRetrun, TEntity>(this IDbConnection conn, string fieldNames, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
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

            var list = conn.Query<TRetrun>(sql, sqlAndParams.Item2, tran);

           

            return list.FirstOrDefault();
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
        public static TEntity QueryModel<TEntity>(this IDbConnection conn, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        {
            return QueryModel<TEntity, TEntity>(conn, "*", whereParam, orderBy, tran);
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
        //public static TRetrun QueryModel<TRetrun, TEntity>(this IDbConnection conn, object whereParam = null, string orderBy = null, IDbTransaction tran = null)
        //{
        //    return QueryModel<TRetrun, TEntity>(conn, null, whereParam, orderBy, tran);
        //}

        #endregion

        #region Insert

        public static int Insert<TEntity>(this IDbConnection cnn, TEntity model, string expelFields = null, IDbTransaction tran = null)
        {
            CommandDefinition cd = GetInsertCommand<TEntity>(model, expelFields, tran);

            return cnn.Execute(cd);
        }

        public static long InsertWithReturnIdentity<TEntity>(this IDbConnection cnn, TEntity model, string expelFields = null, IDbTransaction tran = null)
        {
            CommandDefinition cd = GetInsertCommand<TEntity>(model, expelFields, tran, true);

            return cnn.ExecuteScalar<long>(cd);
        }


        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="list"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int InsertList<TEntity>(this IDbConnection cnn, IEnumerable<TEntity> list, string expelFields = null, IDbTransaction tran = null)
        {
            CommandDefinition cmd = GetInsertListCommand<TEntity>(list, expelFields, tran);

            return cnn.Execute(cmd);
        }

        #endregion

        #region update

        /// <summary>
        /// 更新数据（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        public static int Update<T>(this IDbConnection cnn, object setParam, object whereParam, IDbTransaction tran = null)
        {
            if (whereParam == null || !whereParam.HasParameters())
            {
                throw new Exception("更新数据必须带有条件");
            }

            CommandDefinition cd = GetUpdateCommand<T>(setParam, whereParam, null, tran);

            return cnn.Execute(cd);
        }

        /// <summary>
        /// 更新数据（注意： model 和whereParam不能包含相同的属性名  ！！，whereParam 请使用主键）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="model"></param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int Update<T>(this IDbConnection cnn, T model, string expelFields = null, IDbTransaction tran = null)
        {
            CommandDefinition cd = GetUpdateCommand<T>(model, null, expelFields, tran);

            return cnn.Execute(cd);
        }

        /// <summary>
        /// 更新数据(原数据上加)（注意：setParam和whereParam不能包含相同的属性名！！）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="mparams"></param>
        /// <param name="otherParam">其他正常更新字段</param>
        /// <param name="whereParam"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int UpdateOnOld<T>(this IDbConnection cnn, object mparams, object whereParam = null, object otherParam = null, IDbTransaction tran = null)
        {
            var tableName = typeof(T).Name;

            var paramters = new DynamicParameters();
            var setSql = "";
            var pros = mparams.GetType().GetProperties();
            foreach (var p in pros)
            {
                var val = p.GetValue(mparams);

                setSql += $" {p.Name}={p.Name}+@{p.Name},";

                paramters.Add(p.Name, val);
            }

            ///model  匿名类的对象
            var prosO = otherParam.GetType().GetProperties();
            foreach (var p in prosO)
            {
                var val = p.GetValue(otherParam);

                setSql += $" {p.Name}=@{p.Name},";

                paramters.Add(p.Name, val);
            }

            //移除 ，
            if (setSql.Length > 1)
            {
                setSql = setSql.Remove(setSql.Length - 1, 1);
            }


            var whereAndParams = GetWhereSqlAndParameters(whereParam);

            paramters.AppendParameters(whereAndParams.Item2);


            string sql = string.Format("UPDATE [{0}] SET {1} WHERE {2}", tableName, setSql, whereAndParams.Item1);

            return cnn.Execute(sql, paramters, tran);
        }

        #endregion

        #region Command

        private static CommandDefinition GetInsertListCommand<T>(IEnumerable<T> list, string expelFields = null, IDbTransaction tran = null)
        {
            var type = typeof(T);

            var keyNames = type.GetKeyNames();

            var names = type.GetProperties().Select(p => p.Name);

            if (!string.IsNullOrEmpty(expelFields))
            {
                var exFieldList = expelFields.Split(',');
                names = names.Where(p => !exFieldList.IsContains(p));
            }
            var fields = new List<string>();

            foreach (var pname in names)
            {
                if (keyNames.Item2.IsContains(pname) || keyNames.Item3.IsContains(pname)) continue;

                fields.Add(pname);
            }
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})", type.Name,
                string.Format("[{0}]", string.Join("],[", fields)), string.Join(",@", fields));

            return new CommandDefinition(sql, list.ToArray(), tran);
        }

        private static CommandDefinition GetInsertCommand<T>(object model, string expelFields = null, IDbTransaction tran = null, bool returnIdentity = false)
        {
            var keyNames = typeof(T).GetKeyNames();

            var pros = model.GetType().GetProperties();

            if (!string.IsNullOrEmpty(expelFields))
            {
                var exFieldList = expelFields.Split(',');
                pros = pros.Where(p => !exFieldList.IsContains(p.Name)).ToArray();
            }
            var param = new DynamicParameters();

            foreach (var p in pros)
            {
                if (keyNames.Item2.IsContains(p.Name) || keyNames.Item3.IsContains(p.Name)) continue;
                var val = p.GetValue(model);
                if (val == null) continue;
                if (val is DateTime)
                {
                    if (Convert.ToDateTime(val) == DateTime.MinValue)
                    {
                        continue;
                    }
                }
                param.Add(p.Name, val);
            }
            var setNames = param.ParameterNames;

            string sql = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})", typeof(T).Name,
                string.Format("[{0}]", string.Join("],[", setNames)), string.Join(",@", setNames));

            if (returnIdentity)
            {
                sql += ";SELECT SCOPE_IDENTITY()";
            }

            return new CommandDefinition(sql, param, tran);
        }

        private static CommandDefinition GetUpdateCommand<T>(object model, object whereParam = null, string expelFields = null, IDbTransaction tran = null)
        {
            return GetUpdateCommand(typeof(T), model, whereParam, expelFields, tran);
        }

        private static CommandDefinition GetUpdateCommand(Type type, object model, object whereParam = null, string expelFields = null, IDbTransaction tran = null)
        {
            var keyNames = type.GetKeyNames();

            if (keyNames.Item1.Count == 0 && (whereParam == null || !whereParam.HasParameters()))
            {
                throw new Exception("数据没有设置主键或没有条件参数");
            }
            ///model  匿名类的对象
            var pros = model.GetType().GetProperties();

            if (!string.IsNullOrEmpty(expelFields))
            {
                var exFieldList = expelFields.Split(',');
                pros = pros.Where(p => !exFieldList.IsContains(p.Name)).ToArray();
            }
            var paramters = new DynamicParameters();

            ///主键参数
            DynamicParameters pkParamters = null;

            if (whereParam == null || !whereParam.HasParameters())
            {
                pkParamters = new DynamicParameters();
            }

            foreach (var p in pros)
            {
                var val = p.GetValue(model);
                if (keyNames.Item1.IsContains(p.Name) && pkParamters != null)
                {
                    pkParamters.Add(p.Name, val);
                    continue;
                }
                if (keyNames.Item2.IsContains(p.Name) || keyNames.Item3.IsContains(p.Name)) continue;

                if (val != null && val is DateTime)
                {
                    if (Convert.ToDateTime(val) == DateTime.MinValue)
                    {
                        continue;
                    }
                }
                paramters.Add(p.Name, val);
            }
            var sets = paramters.ParameterNames.Select(p => string.Format("[{0}]=@{0}", p)).ToList();

            string where = null;

            if (pkParamters != null)
            {
                where = string.Join(" AND ", pkParamters.ParameterNames.Select(p => string.Format("[{0}]=@{0}", p)));

                paramters.AppendParameters(pkParamters);
            }
            else
            {
                var whereAndParams = GetWhereSqlAndParameters(whereParam);

                where = whereAndParams.Item1;

                paramters.AppendParameters(whereAndParams.Item2);
            }
            string sql = string.Format("UPDATE [{0}] SET {1} WHERE {2}", type.Name, string.Join(",", sets), where);

            return new CommandDefinition(sql, paramters, tran);
        }

        #endregion

        #region delete
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        public static int Delete<T>(this IDbConnection cnn, object whereParam, IDbTransaction transacton = null)
        {
            if (whereParam == null || !whereParam.HasParameters())
            {
                throw new Exception("删除数据必须带有条件参数");
            }

            var whereAndParams = GetWhereSqlAndParameters(whereParam);

            string sql = string.Format("DELETE FROM [{0}] WHERE {1}", typeof(T).Name, whereAndParams.Item1);

            return cnn.Execute(sql, whereAndParams.Item2, transacton);
        }

        #endregion

    }

}
