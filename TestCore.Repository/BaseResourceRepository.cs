using Autofac;
using Caiba.IRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Common.Helper;
using TestCore.Data.Dapper;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Enums;
using TestCore.Domain.SysEntity;
using TestCore.IRepository;
using TestCore.IRepository.SysAdmin;
using TestCore.Repositories;

namespace Caiba.Repositories
{

    public class BaseResourceRepository<TEntity> : BaseRepository<TEntity>, IBaseResourceRepository<TEntity>
    {
        public readonly IResourceRepository ResourceRepository;

        public string OrderBy = "sortIndex asc";

        public TableEnum Table;

        public int TableId;

        public BaseResourceRepository(IResourceRepository resourceRepository, TableEnum table)
        {
            ResourceRepository = resourceRepository;
            Table = table;
            TableId = (int)table;
        }

        public async Task<TEntity> GetModelAsync(int id, string fieldName = null, string lang = null)
        {
            var model = await base.GetModelAsync(new { id });

            if (string.IsNullOrEmpty(fieldName)) return model;

            var type = typeof(TEntity);

            var names = fieldName.Split(',');

            var pros = type.GetProperties().Where(p => names.IsContains(p.Name));

            if (!pros.Any()) return model;

            foreach (var p in pros)
            {
                var resValue = ResourceRepository.GetResourceFromCache(this.TableId, p.Name, id, lang);
                if (!string.IsNullOrEmpty(resValue))
                {
                    p.SetValue(model, resValue);
                }
            }
            return model;
        }


        /// <summary>
        /// 插入数据,，清除缓存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public async Task<int> InsertWithResourceAsync(TEntity entity, List<SysResource> list, int tableId)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                var pkid = await conn.InsertWithReturnIdentityAsync(entity, null, tran);

                foreach(var item in list)
                {
                    item.PkId = (int)pkid;
                }
   
                var row = await conn.InsertListAsync(list, null, tran);

                tran.Commit();

                row++;
 
                this.ResourceRepository.ClearResourceCache(tableId);

                return row;
            }
        }


        public async Task<int> InsertWithResourceAsync(TEntity entity, string fieldNames)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                var pkid = await conn.InsertWithReturnIdentityAsync(entity, null, tran);

                var names = fieldNames.Split(',');

                int row = 1;

                var list = this.BuildResourceList(entity, (int)pkid, names);

                if(list.Any())
                {
                    row = await conn.InsertListAsync(list, null, tran);
                }
                tran.Commit();

                this.ResourceRepository.ClearResourceCache(this.TableId);

                return row;
            }
        }


        private List<SysResource> BuildResourceList(TEntity model, int pkId, string[] fieldNames )
        {
            
            var list = new List<SysResource>();
            if (fieldNames == null || !fieldNames.Any()) return list;
            var langList = ResourceHelper.GetLangSelectListFromCache();
            var type = typeof(TEntity);

            var pros = type.GetProperties().Where(p => fieldNames.IsContains(p.Name)).ToArray();

            var status = Convert.ToInt32(type.GetPropertyValue(model, "Status") ?? 0);
            var sortIndex =  Convert.ToInt32( type.GetPropertyValue(model, "SortIndex") ?? 0);

            var userName = CoreHttpContext.Current.User.Identity.Name;

            foreach (var p in pros)
            {
                var resValue = p.GetValue(model) + "";

                foreach (var lang in langList)
                {
                    list.Add(new SysResource
                    {
                        FieldName = p.Name,
                        Lang = lang.Value,
                        PkId = pkId,
                        ResValue = resValue,
                        SortIndex = sortIndex,
                        Status = status,
                        TableId = TableId,
                        TableName = Table.ToString(),
                        CreateTime = DateTime.Now,
                        Creator = userName,
                        DbId = 1,
                        UpdateTime = DateTime.Now
                    });
                }
            }
            return list;
        }

        public async Task<int> DeleteWithResourceAsync(string[] ids, int tableId = 0)
        {
            using (var conn = ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                var row = await conn.DeleteAsync<TEntity>(new { id = ids }, tran);

                if (tableId > 0)
                {
                    row += await conn.DeleteAsync<SysResource>(new
                    {
                        TableId = tableId,
                        PkId = ids
                    }, tran);

                    this.ResourceRepository.ClearResourceCache(tableId);
                }
                tran.Commit();

                return row;
            }
        }

        /// <summary>
        /// 更新数据，同时更新 数据字典  SysResource
        /// </summary>
        /// <param name="model"></param>
        /// <param name="expelFields"></param>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkId"></param>
        /// <param name="setParam"></param>
        /// <returns></returns>
        public async Task<int> UpdateWithResourceAsync(TEntity model, string expelFields = null, SysResource resource = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                var row = await conn.UpdateAsync(model, expelFields, tran);

                if (resource != null)
                {
                    //修改资源
                    row += await conn.UpdateAsync<SysResource>(new {  resource.ResValue },  
                        new { resource.TableId, resource.FieldName, resource.PkId, Lang = CoreHttpContext.CurrentCulture.Name }, tran);

                    if (resource.SortIndex > 0)  // 修改排序
                    {
                        row += await conn.UpdateAsync<SysResource>(new { resource.SortIndex },
                        new { resource.TableId, resource.PkId }, tran);
                    }
                    this.ResourceRepository.ClearResourceCache(resource.TableId);
                }
                tran.Commit();

                return row;
            }
        }

        /// <summary>
        /// 更新数据状态状态，同时更新 数据字典  SysResource
        /// </summary>
        /// <param name="status"></param>
        /// <param name="table"></param>
        /// <param name="fieldName"></param>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusWithResourceAsync(int status, int tableId, string[] pkId)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                var tran = conn.BeginTransaction();

                var setParam = new { status };

                var row = await conn.UpdateAsync<TEntity>(setParam, new { id = pkId }, tran);

                row += await conn.UpdateAsync<SysResource>(setParam, new { tableId, pkId }, tran);

                tran.Commit();

                this.ResourceRepository.ClearResourceCache(tableId);

                return row;
            }
        }

        /// <summary>
        /// 上升或者下降排序
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="sortIndex"></param>
        /// <param name="sortType"></param>
        /// <param name="parentId"></param>
        /// <param name="roleGroup"></param>
        /// <param name="memId"></param>
        /// <param name="operId"></param>
        /// <returns></returns>
        public async Task<int> ChangeSortIndexAsync(Type type, int id, int sortIndex, int sortType, Condition cond = null, bool updateResource = true )
        {
            //默认下降
            var compareType = CompareType.GreaterThanEquals;
            var orderType = "asc";

            if (sortType == 1) //上升
            {
                compareType = CompareType.LessThanEquals;
                orderType = "desc";
            }
            var pkName = type.GetPkName();

            string orderBy = string.Format("SortIndex {0},{1} {0}", orderType, pkName);

            if(cond == null)
            {
                cond = new Condition();
            }

            // id <> 10 and SortIndex >= 22 
            cond.Where(pkName, id, CompareType.NotEquals).Where("SortIndex", sortIndex, compareType);

            //if (type.HasProperty("ParentId"))
            //{
            //    cond.Where("ParentId", parentId);
            //}
            //if (type == typeof(SysAccount))
            //{
            //    if (roleGroup==RoleGroupEnum.Operator)
            //    {
            //        cond.Where(nameof(SysAccount.OperId), operId);
            //    }
            //    else 
            //    {
            //        cond.Where(nameof(SysAccount.MemId), memId);
            //    }
            //}

            var qparams = new QueryParams
            {
                FieldNames = string.Format("{0} as Id,SortIndex", pkName),
                OrderBy = orderBy,
                PageSize = 1,
                TableName = type.Name,
                Condition = cond,
            };
            int row = 0;

            var tableId = 0; //TableHelper.GetTableId(type);

            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                var redata = await conn.QueryListAsync<SimpleIndex>(qparams);

                if (!redata.Result.Any()) return 0;

                ///DateTime.UtcNow

                var next = redata.Result.FirstOrDefault();

                var oldWhereParam = new DynamicParameters();
                oldWhereParam.Add(pkName, id);

                var oldSetParam = new { SortIndex = next.SortIndex };

                var nextWhereParam = new DynamicParameters();
                nextWhereParam.Add(pkName, next.Id);
                var nextSetParam = new { SortIndex = sortIndex };

                var tran = conn.BeginTransaction();

                ///更新 sortIndex  交换 sortIndex
                row = await conn.UpdateAsync(type, oldSetParam, oldWhereParam, tran);
                row += await conn.UpdateAsync(type, nextSetParam, nextWhereParam, tran);

                if (updateResource)
                {
                    tableId = TableHelper.GetTableId(type);
                    //更新资源表的排序
                    row += await conn.UpdateAsync<SysResource>(oldSetParam, new { tableId = tableId, pkId = id }, tran);
                    row += await conn.UpdateAsync<SysResource>(nextSetParam, new { tableId = tableId, pkId = next.Id }, tran);
                }
                tran.Commit();
            }

            if (row > 0)
            {
                ///清除缓存
                if (type == typeof(SysLang))
                {
                    this.ResourceRepository.ClearLangCache();
                }
                else
                {
                    this.ResourceRepository.ClearResourceCache(tableId);
                }
            }
            return row;
        }


    }
}
