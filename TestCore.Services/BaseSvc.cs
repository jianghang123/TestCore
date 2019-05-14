using Autofac;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;
using TestCore.IRepository;
using TestCore.IService;

namespace TestCore.Services
{
    public class BaseSvc<TEntity> : IBaseSvc<TEntity>
    {
        private readonly IRepository<TEntity> _repository = IoCBootstrapper.AutoContainer.Resolve<IRepository<TEntity>>();

        public int Delete(object whereParam)
        {
            return _repository.Delete(whereParam);
        }

        public Task<int> DeleteAsync(object whereParam)
        {
            return _repository.DeleteAsync(whereParam);
        }

        public PageData<object> GetAnonymousList(QueryParams qparams)
        {
            return _repository.GetAnonymousList(qparams);
        }

        public Task<PageData<object>> GetAnonymousListAsync<TTable>(QueryParams qparams)
        {
            return _repository.GetAnonymousListAsync<TTable>(qparams);
        }

        public int GetCount<TTable>(object whereParam = null)
        {
            return _repository.GetCount<TTable>(whereParam);
        }

        public Task<int> GetCountAsync<TTable>(object whereParam = null, string distinctField = null)
        {
            return _repository.GetCountAsync<TTable>(whereParam, distinctField);
        }

        public IEnumerable<TEntity> GetList(object whereParam = null, string orderBy = null)
        {
            return _repository.GetList(whereParam, orderBy);
        }

        public IEnumerable<TTable> GetList<TTable>(object whereParam, string orderBy = null)
        {
            return _repository.GetList<TTable>(whereParam, orderBy);
        }

        public PageData<TEntity> GetList(QueryParams qparams)
        {
            return _repository.GetList(qparams);
        }

        public Task<IEnumerable<TEntity>> GetListAsync(object whereParam = null, string orderBy = null)
        {
            return _repository.GetListAsync(whereParam, orderBy);
        }

        public Task<IEnumerable<TTable>> GetListAsync<TTable>(object whereParam, string orderBy = null)
        {
            return _repository.GetListAsync<TTable>(whereParam, orderBy);
        }

        public Task<PageData<TTable>> GetListAsync<TTable>(QueryParams qparams)
        {
            return _repository.GetListAsync<TTable>(qparams);
        }

        public Task<PageData<TEntity>> GetListAsync(QueryParams qparams)
        {
            return _repository.GetListAsync(qparams);
        }

        public Task<TReturn> GetMaxAsync<TReturn, TTable>(string fieldName, object whereParam = null)
        {
            return _repository.GetMaxAsync<TReturn, TTable>(fieldName, whereParam);
        }

        public Task<TReturn> GetMinAsync<TReturn, TTable>(string fieldName, object whereParam = null)
        {
            return _repository.GetMinAsync<TReturn, TTable>(fieldName, whereParam);
        }

        public TEntity GetModel(object whereParam, string orderBy = null)
        {
            return _repository.GetModel(whereParam, orderBy);
        }

        public TTable GetModel<TTable>(object whereParam, string orderBy = null)
        {
            return _repository.GetModel<TTable>(whereParam, orderBy);
        }

        public Task<TEntity> GetModelAsync(object whereParam, string orderBy = null)
        {
            return _repository.GetModelAsync(whereParam, orderBy);
        }

        public Task<TTable> GetModelAsync<TTable>(object whereParam, string orderBy = null)
        {
            return _repository.GetModelAsync<TTable>(whereParam, orderBy);
        }

        public Task<TResult> GetModelAsync<TResult, TTable>(string fieldName, object whereParam, string orderBy = null)
        {
            return _repository.GetModelAsync<TResult, TTable>(fieldName, whereParam, orderBy);
        }

        public Task<IEnumerable<NameItem>> GetNameItemsAsync<TTable>(string keyField, string nameField, object whereParam = null, string orderBy = null)
        {
            return _repository.GetNameItemsAsync<TTable>(keyField, nameField, whereParam, orderBy);
        }

        public Task<IEnumerable<NameItem>> GetNameItemsAsync(string keyField, string nameField, string tableName, object whereParam = null, string orderBy = null, int count = 0)
        {
            return _repository.GetNameItemsAsync(keyField, nameField, tableName, whereParam, orderBy, count);
        }

        public Task<IEnumerable<SelectListItem>> GetSelectListAsync<TTable>(string valueField, string textField, object whereParam = null, string orderBy = null)
        {
            return _repository.GetSelectListAsync<TTable>(valueField, textField, whereParam, orderBy);
        }

        public Task<IEnumerable<SelectListItem>> GetSelectListAsync(string valueField, string textField, string tableName, object whereParam = null, string orderBy = null)
        {
            return _repository.GetSelectListAsync(valueField, textField, tableName, whereParam, orderBy);
        }

        public IEnumerable<TSimple> GetSimpleList<TSimple>(object whereParam = null, string orderBy = null)
        {
            return _repository.GetSimpleList<TSimple>(whereParam, orderBy);
        }

        public IEnumerable<TSimple> GetSimpleList<TSimple, TTable>(object whereParam = null, string orderBy = null)
        {
            return _repository.GetSimpleList<TSimple, TTable>(whereParam, orderBy);
        }

        public PageData<TSimple> GetSimpleList<TSimple>(QueryParams qparams)
        {
            return _repository.GetSimpleList<TSimple>(qparams);
        }

        public Task<IEnumerable<TSimple>> GetSimpleListAsync<TSimple, TTable>(object whereParam = null, string orderBy = null)
        {
            return _repository.GetSimpleListAsync<TSimple, TTable>(whereParam, orderBy);
        }

        public Task<IEnumerable<TSimple>> GetSimpleListAsync<TSimple>(object whereParam = null, string orderBy = null)
        {
            return _repository.GetSimpleListAsync<TSimple>(whereParam, orderBy);
        }

        public Task<PageData<TSimple>> GetSimpleListAsync<TSimple, TTable>(QueryParams qparams)
        {
            return _repository.GetSimpleListAsync<TSimple, TTable>(qparams);
        }

        public TSimple GetSimpleModel<TSimple>(object whereParam, string orderBy = null)
        {
            return _repository.GetSimpleModel<TSimple>(whereParam, orderBy);
        }

        public IEnumerable<TReturn> GetSingleList<TReturn>(string fieldName, object whereParam = null, string orderBy = null)
        {
            return _repository.GetSingleList<TReturn>(fieldName, whereParam, orderBy);
        }

        public IEnumerable<TReturn> GetSingleList<TReturn, TTable>(string fieldName, object whereParam = null, string orderBy = null)
        {
            return _repository.GetSingleList<TReturn, TTable>(fieldName, whereParam, orderBy);
        }

        public Task<IEnumerable<TReturn>> GetSingleListAsync<TReturn>(string fieldName, object whereParam = null, string orderBy = null)
        {
            return _repository.GetSingleListAsync<TReturn>(fieldName, whereParam, orderBy);
        }

        public Task<IEnumerable<TReturn>> GetSingleListAsync<TReturn, TTable>(string fieldName, object whereParam = null, string orderBy = null)
        {
            return _repository.GetSingleListAsync<TReturn, TTable>(fieldName, whereParam, orderBy);
        }

        public TReturn GetSum<TReturn, TTable>(string fieldName, object whereParam = null, IDbTransaction tran = null)
        {
            return _repository.GetSum<TReturn, TTable>(fieldName, whereParam, tran);
        }

        public Task<TReturn> GetSumAsync<TReturn, TTable>(string fieldName, object whereParam = null, IDbTransaction tran = null)
        {
            return _repository.GetSumAsync<TReturn, TTable>(fieldName, whereParam, tran);
        }

        public int Insert(TEntity entity, string expelFields = null)
        {
            return _repository.Insert(entity, expelFields);
        }

        public Task<int> InsertAsync(TEntity entity, string expelFields = null)
        {
            return _repository.InsertAsync(entity, expelFields);
        }

        public long InsertWithReturnIdentity(TEntity entity, string expelFields = null)
        {
            return _repository.InsertWithReturnIdentity(entity, expelFields);
        }

        public Task<long> InsertWithReturnIdentityAsync(TEntity entity, string expelFields = null)
        {
            return _repository.InsertWithReturnIdentityAsync(entity, expelFields);
        }

        public int Update(object setParam, object whereParam)
        {
            return _repository.Update(setParam, whereParam);
        }

        public int Update(TEntity model, string expelFields = null)
        {
            return _repository.Update(model, expelFields);
        }

        public Task<int> UpdateAsync(object setParam, object whereParam)
        {
            return _repository.UpdateAsync(setParam, whereParam);
        }

        public Task<int> UpdateAsync(TEntity model, string expelFields = null)
        {
            return _repository.UpdateAsync(model, expelFields);
        }

        public int UpdateOnOld<TTable>(object mparams, object whereParam, object otherParam = null)
        {
            return _repository.UpdateOnOld<TTable>(mparams, whereParam, otherParam = null);
        }
    }
}
