using Masuit.Tools;
using System.Linq.Expressions;
using SzlqTech.Common.Assert;
using SzlqTech.Entity;

namespace SzlqTech.DbHelper
{
    public class BaseServiceImpl<TM, T> : IBaseService<T> where TM : IBaseRepository<T> where T : BaseEntity, new()
    {
        protected TM BaseRepository;

        public BaseServiceImpl(TM baseRepository)
        {
            BaseRepository = baseRepository;
        }

        public virtual bool Save(T entity)
        {
            return SqlHelper.RetBool(BaseRepository.Insert(entity));
        }

        public virtual async Task<bool> SaveAsync(T entity)
        {
            return SqlHelper.RetBool(await BaseRepository.InsertAsync(entity));
        }

        public virtual bool Save(List<T> entities)
        {
            return SqlHelper.RetBool(BaseRepository.Insert(entities));
        }

        public virtual async Task<bool> SaveAsync(List<T> entities)
        {
            return SqlHelper.RetBool(await BaseRepository.InsertAsync(entities));
        }

        public virtual bool SaveBatch(List<T> entities, int batchSize = 100)
        {
            return BaseRepository.InsertBatch(entities, batchSize);
        }

        public virtual async Task<bool> SaveBatchAsync(List<T> entities, int batchSize = 100)
        {
            return await BaseRepository.InsertBatchAsync(entities, batchSize);
        }

        public virtual bool SaveOrUpdate(List<T> entities)
        {
            return SqlHelper.RetBool(BaseRepository.InsertOrUpdate(entities));
        }

        public virtual async Task<bool> SaveOrUpdateAsync(List<T> entities)
        {
            return SqlHelper.RetBool(await BaseRepository.InsertOrUpdateAsync(entities));
        }

        public virtual bool SaveOrUpdateBatch(List<T> entities, int batchSize = 100)
        {
            return BaseRepository.InsertOrUpdateBatch(entities, batchSize);
        }

        public virtual async Task<bool> SaveOrUpdateBatchAsync(List<T> entities, int batchSize = 100)
        {
            return await BaseRepository.InsertOrUpdateBatchAsync(entities, batchSize);
        }

        public virtual bool SaveBatchNotExist(List<T> entities, int batchSize = 100)
        {
            return BaseRepository.InsertBatchNotExist(entities, batchSize);
        }

        public async Task<bool> SaveBatchNotExistAsync(List<T> entities, int batchSize = 100)
        {
            return await BaseRepository.InsertBatchNotExistAsync(entities, batchSize);
        }

        public virtual bool RemoveById(long id, bool useFill = false)
        {
            if (useFill)
            {
                throw new ArgumentException("useFill");
            }

            return SqlHelper.RetBool(BaseRepository.DeleteById(id));
        }

        public virtual async Task<bool> RemoveByIdAsync(long id, bool useFill = false)
        {
            if (useFill)
            {
                throw new ArgumentException("useFill");
            }

            return SqlHelper.RetBool(await BaseRepository.DeleteByIdAsync(id));
        }

        public virtual bool RemoveById(string id, bool useFill = false)
        {
            if (useFill)
            {
                throw new ArgumentException("useFill");
            }

            return SqlHelper.RetBool(BaseRepository.DeleteById(id));
        }

        public async Task<bool> RemoveByIdAsync(string id, bool useFill = false)
        {
            if (useFill)
            {
                throw new ArgumentException("useFill");
            }

            return SqlHelper.RetBool(await BaseRepository.DeleteByIdAsync(id));
        }

        public bool RemoveById(T entity)
        {
            return SqlHelper.RetBool(BaseRepository.DeleteById(entity));
        }

        public async Task<bool> RemoveByIdAsync(T entity)
        {
            return SqlHelper.RetBool(await BaseRepository.DeleteByIdAsync(entity));
        }

        public bool RemoveByMap(List<Dictionary<string, object>> whereColumnMaps)
        {
            SqlAssert.NotEmpty(whereColumnMaps, "error: columnMap must not be empty");
            return SqlHelper.RetBool(BaseRepository.DeleteByMap(whereColumnMaps));
        }

        public async Task<bool> RemoveByMapAsync(List<Dictionary<string, object>> whereColumnMaps)
        {
            SqlAssert.NotEmpty(whereColumnMaps, "error: columnMap must not be empty");
            return SqlHelper.RetBool(await BaseRepository.DeleteByMapAsync(whereColumnMaps));
        }

        public bool Remove(Expression<Func<T, bool>> whereExpression)
        {
            return SqlHelper.RetBool(BaseRepository.Delete(whereExpression));
        }

        public async Task<bool> RemoveAsync(Expression<Func<T, bool>> whereExpression)
        {
            return SqlHelper.RetBool(await BaseRepository.DeleteAsync(whereExpression));
        }

        public bool RemoveByIds(List<long> list)
        {
            if (list.IsNullOrEmpty())
            {
                return false;
            }

            return BaseRepository.DeleteBatchIds(list);
        }

        public async Task<bool> RemoveByIdsAsync(List<long> list)
        {
            if (list.IsNullOrEmpty())
            {
                return false;
            }

            return await BaseRepository.DeleteBatchIdsAsync(list);
        }

        public bool RemoveByIds(List<string> list)
        {
            if (list.IsNullOrEmpty())
            {
                return false;
            }

            return BaseRepository.DeleteBatchIds(list);
        }

        public async Task<bool> RemoveByIdsAsync(List<string> list)
        {
            if (list.IsNullOrEmpty())
            {
                return false;
            }

            return await BaseRepository.DeleteBatchIdsAsync(list);
        }

        public bool RemoveByIds(List<T> entities)
        {
            if (entities.IsNullOrEmpty())
            {
                return false;
            }

            return BaseRepository.DeleteBatchIds(entities);
        }

        public async Task<bool> RemoveByIdsAsync(List<T> entities)
        {
            if (entities.IsNullOrEmpty())
            {
                return false;
            }

            return await BaseRepository.DeleteBatchIdsAsync(entities);
        }

        public bool RemoveAll()
        {
            return SqlHelper.RetBool(BaseRepository.DeleteAll());
        }

        public async Task<bool> RemoveAllAsync()
        {
            return SqlHelper.RetBool(await BaseRepository.DeleteAllAsync());
        }

        public virtual bool UpdateById(T entity)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateById(entity));
        }

        public virtual async Task<bool> UpdateByIdAsync(T entity)
        {
            return SqlHelper.RetBool(await BaseRepository.UpdateByIdAsync(entity));
        }

        public virtual bool Update(T entity, Expression<Func<T, bool>> whereExpression)
        {
            return SqlHelper.RetBool(BaseRepository.Update(entity, whereExpression));
        }

        public virtual async Task<bool> UpdateAsync(T entity, Expression<Func<T, bool>> whereExpression)
        {
            return SqlHelper.RetBool(await BaseRepository.UpdateAsync(entity, whereExpression));
        }

        public virtual bool UpdateBatchById(List<T> entities, int batchSize = 100)
        {
            return BaseRepository.UpdateBatchById(entities, batchSize);
        }

        public virtual async Task<bool> UpdateBatchByIdAsync(List<T> entities, int batchSize = 100)
        {
            return await BaseRepository.UpdateBatchByIdAsync(entities, batchSize);
        }

        public virtual bool SaveOrUpdate(T entity)
        {
            return SqlHelper.RetBool(BaseRepository.InsertOrUpdate(entity));
        }

        public virtual async Task<bool> SaveOrUpdateAsync(T entity)
        {
            return SqlHelper.RetBool(await BaseRepository.InsertOrUpdateAsync(entity));
        }

        public virtual bool UpdateColumnsById(T entity, Expression<Func<T, object>> columnExpression)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateColumnsById(entity, columnExpression));
        }

        public virtual async Task<bool> UpdateColumnsByIdAsync(T entity, Expression<Func<T, object>> columnExpression)
        {
            return SqlHelper.RetBool(await BaseRepository.UpdateColumnsByIdAsync(entity, columnExpression));
        }

        public virtual bool UpdateColumnsById(T entity, params string[] columns)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateColumnsById(entity, columns));
        }

        public virtual async Task<bool> UpdateColumnsByIdAsync(T entity, params string[] columns)
        {
            return SqlHelper.RetBool(await BaseRepository.UpdateColumnsByIdAsync(entity, columns));
        }

        public virtual bool UpdateColumns(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateColumns(setColumnsExpression, whereExpression));
        }

        public virtual async Task<bool> UpdateColumnsAsync(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return SqlHelper.RetBool(await BaseRepository.UpdateColumnsAsync(setColumnsExpression, whereExpression));
        }

        public T GetById(long id)
        {
            return BaseRepository.SelectById(id);
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await BaseRepository.SelectByIdAsync(id);
        }

        public T? GetById(string id)
        {
            return BaseRepository.SelectById(id);
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await BaseRepository.SelectByIdAsync(id);
        }

        public T? GetByCode(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectByCode(code, whereExpression);
        }

        public async Task<T?> GetByCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectByCodeAsync(code, whereExpression);
        }

        public List<T> ListByIds(List<long> ids)
        {
            return BaseRepository.SelectBatchIds(ids);
        }

        public async Task<List<T>> ListByIdAsync(List<long> ids)
        {
            return await BaseRepository.SelectBatchIdsAsync(ids);
        }

        public List<T> ListByIds(List<string> ids)
        {
            return BaseRepository.SelectBatchIds(ids);
        }

        public async Task<List<T>> ListByIdAsync(List<string> ids)
        {
            return await BaseRepository.SelectBatchIdsAsync(ids);
        }

        public List<T> ListByMap(List<Dictionary<string, object>> whereColumnMaps)
        {
            return BaseRepository.SelectByMap(whereColumnMaps);
        }

        public async Task<List<T>> ListByMapAsync(List<Dictionary<string, object>> whereColumnMaps)
        {
            return await BaseRepository.SelectByMapAsync(whereColumnMaps);
        }

        public virtual T? GetOne(Expression<Func<T, bool>> whereExpression, bool throwEx = true)
        {
            if (!throwEx)
            {
                return BaseRepository.SelectFirst(whereExpression);
            }

            return BaseRepository.SelectOne(whereExpression);
        }

        public virtual async Task<T?> GetOneAsync(Expression<Func<T, bool>> whereExpression, bool throwEx = true)
        {
            return (!throwEx) ? (await BaseRepository.SelectFirstAsync(whereExpression)) : (await BaseRepository.SelectOneAsync(whereExpression));
        }

        public virtual long Count(Expression<Func<T, bool>>? whereExpression = null)
        {
            return SqlHelper.RetCount(BaseRepository.SelectCount(whereExpression));
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            return SqlHelper.RetCount(await BaseRepository.SelectCountAsync(whereExpression));
        }

        public virtual List<T> List(Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectList(whereExpression);
        }

        public virtual async Task<List<T>> ListAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectListAsync(whereExpression);
        }

        public virtual List<TResult> ListObj<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectObjList(selectExpression, whereExpression);
        }

        public virtual async Task<List<TResult>> ListObjAsync<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectObjListAsync(selectExpression, whereExpression);
        }

        public virtual List<Dictionary<string, object>> ListMaps(Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectMapList(whereExpression);
        }

        public virtual async Task<List<Dictionary<string, object>>> ListMapsAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectMapListAsync(whereExpression);
        }

        public virtual bool ExistById(long id)
        {
            return BaseRepository.ExistById(id);
        }

        public virtual async Task<bool> ExistByIdAsync(long id)
        {
            return await BaseRepository.ExistByIdAsync(id);
        }

        public virtual bool ExistById(string id)
        {
            return BaseRepository.ExistById(id);
        }

        public virtual async Task<bool> ExistByIdAsync(string id)
        {
            return await BaseRepository.ExistByIdAsync(id);
        }

        public virtual bool ExistById(T entity)
        {
            return BaseRepository.ExistById(entity);
        }

        public virtual async Task<bool> ExistByIdAsync(T entity)
        {
            return await BaseRepository.ExistByIdAsync(entity);
        }

        public virtual bool ExistCode(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.ExistCode(code, whereExpression);
        }

        public async Task<bool> ExistCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.ExistCodeAsync(code, whereExpression);
        }

        public bool Exist(Expression<Func<T, bool>> whereExpression)
        {
            return BaseRepository.Exist(whereExpression);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await BaseRepository.ExistAsync(whereExpression);
        }

        public async Task<List<T>> ListPageAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectPageListAsync(pageNumber, pageSize, whereExpression);
        }

        public T? GetSingleOrDefault(Expression<Func<T, bool>> whereExpression)
        {
            return BaseRepository.SelectSingleOrDefault(whereExpression);
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> whereExpression)
        {
            return BaseRepository.SelectFirstOrDefault(whereExpression);
        }

        public List<T> ListPage(int pageNumber, int pageSize, ref int totalNumber, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectPageList(pageNumber, pageSize, ref totalNumber, whereExpression);
        }

        public List<T> ListOrderByCode(Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectListOrderByCode(whereExpression);
        }

        public async Task<List<T>> ListOrderByCodeAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectListOrderByCodeAsync(whereExpression);
        }

        public List<T> ListOrderByCodeDescending(Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectListOrderByCodeDescending(whereExpression);
        }

        public async Task<List<T>> ListOrderByCodeDescendingAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectListOrderByCodeDescendingAsync(whereExpression);
        }

        public List<T> ListOrderBy(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectListOrderBy(orderExpression, whereExpression);
        }

        public async Task<List<T>> ListOrderByAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectListOrderByAsync(orderExpression, whereExpression);
        }

        public List<T> ListOrderByDescending(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectListOrderByDescending(orderExpression, whereExpression);
        }

        public async Task<List<T>> ListOrderByDescendingAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectListOrderByDescendingAsync(orderExpression, whereExpression);
        }

        public TResult? GetObjFirst<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> whereExpression)
        {
            return BaseRepository.SelectObjFirst(expression, whereExpression);
        }

        public async Task<TResult?> GetObjFirstAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> whereExpression)
        {
            return await BaseRepository.SelectObjFirstAsync(expression, whereExpression);
        }

        public T? GetFirst(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return BaseRepository.SelectFirst(orderExpression, whereExpression);
        }

        public async Task<T?> GetFirstAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await BaseRepository.SelectFirstAsync(orderExpression, whereExpression);
        }

        public T? GetLast(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return BaseRepository.SelectLast(orderExpression, whereExpression);
        }

        public async Task<T?> GetLastAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await BaseRepository.SelectLastAsync(orderExpression, whereExpression);
        }

        public List<T> ListTop(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectTopList(num, orderExpression, whereExpression);
        }

        public async Task<List<T>> ListTopAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectTopListAsync(num, orderExpression, whereExpression);
        }

        public List<T> ListLast(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectLastList(num, orderExpression, whereExpression);
        }

        public async Task<List<T>> ListLastAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectLastListAsync(num, orderExpression, whereExpression);
        }

        public TResult Max<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectMax(maxExpression, whereExpression);
        }

        public async Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectMaxAsync(maxExpression, whereExpression);
        }

        public TResult Max<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectMax<TResult>(column, whereExpression);
        }

        public async Task<TResult> MaxAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectMaxAsync<TResult>(column, whereExpression);
        }

        public TResult Min<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectMin(minExpression, whereExpression);
        }

        public async Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectMinAsync(minExpression, whereExpression);
        }

        public TResult Min<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectMin<TResult>(column, whereExpression);
        }

        public async Task<TResult> MinAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectMinAsync<TResult>(column, whereExpression);
        }

        public TResult Sum<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectMin(sumExpression, whereExpression);
        }

        public async Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectMinAsync(sumExpression, whereExpression);
        }

        public TResult Sum<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectSum<TResult>(column, whereExpression);
        }

        public async Task<TResult> SumAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            return await BaseRepository.SelectSumAsync<TResult>(column, whereExpression);
        }

        public virtual bool BulkSave(List<T> entities)
        {
            return SqlHelper.RetBool(BaseRepository.BulkInsert(entities));
        }

        public virtual async Task<bool> BulkSaveAsync(List<T> entities)
        {
            return SqlHelper.RetBool(await BaseRepository.BulkInsertAsync(entities));
        }

        public virtual bool BulkUpdate(List<T> entities)
        {
            return SqlHelper.RetBool(BaseRepository.BulkUpdate(entities));
        }

        public virtual async Task<bool> BulkUpdateAsync(List<T> entities)
        {
            return SqlHelper.RetBool(await BaseRepository.BulkUpdateAsync(entities));
        }

        public virtual bool BulkSaveOrUpdate(List<T> entities)
        {
            return SqlHelper.RetBool(BaseRepository.BulkInsertOrUpdate(entities));
        }

        public virtual async Task<bool> BulkSaveOrUpdateAsync(List<T> entities)
        {
            return SqlHelper.RetBool(await BaseRepository.BulkInsertOrUpdateAsync(entities));
        }

        public Type GetEntityType()
        {
            return typeof(T);
        }

        public async Task<List<T>> PageList(int pageNumber, int pageSize, SqlSugar.RefAsync<int> total)
        {
            return await BaseRepository.PageList(pageNumber,pageSize,total);
        }
    }
}
