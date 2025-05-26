using System.Linq.Expressions;
using SzlqTech.Entity;

namespace SzlqTech.DbHelper
{
    public interface IBaseService<T> where T : BaseEntity
    {
        bool Save(T entity);

        Task<bool> SaveAsync(T entity);

        bool Save(List<T> entities);

        Task<bool> SaveAsync(List<T> entities);

        bool SaveBatch(List<T> entities, int batchSize = 100);

        Task<bool> SaveBatchAsync(List<T> entities, int batchSize = 100);

        bool SaveOrUpdate(List<T> entities);

        Task<bool> SaveOrUpdateAsync(List<T> entities);

        bool SaveOrUpdateBatch(List<T> entities, int batchSize = 100);

        Task<bool> SaveOrUpdateBatchAsync(List<T> entities, int batchSize = 100);

        bool SaveBatchNotExist(List<T> entities, int batchSize = 100);

        Task<bool> SaveBatchNotExistAsync(List<T> entities, int batchSize = 100);

        bool RemoveById(long id, bool useFill = false);

        Task<bool> RemoveByIdAsync(long id, bool useFill = false);

        bool RemoveById(string id, bool useFill = false);

        Task<bool> RemoveByIdAsync(string id, bool useFill = false);

        bool RemoveById(T entity);

        Task<bool> RemoveByIdAsync(T entity);

        bool RemoveByMap(List<Dictionary<string, object>> whereColumnMaps);

        Task<bool> RemoveByMapAsync(List<Dictionary<string, object>> whereColumnMaps);

        bool Remove(Expression<Func<T, bool>> whereExpression);

        Task<bool> RemoveAsync(Expression<Func<T, bool>> whereExpression);

        bool RemoveByIds(List<long> list);

        Task<bool> RemoveByIdsAsync(List<long> list);

        bool RemoveByIds(List<string> list);

        Task<bool> RemoveByIdsAsync(List<string> list);

        bool RemoveByIds(List<T> entities);

        Task<bool> RemoveByIdsAsync(List<T> entities);

        bool RemoveAll();

        Task<bool> RemoveAllAsync();

        bool UpdateById(T entity);

        Task<bool> UpdateByIdAsync(T entity);

        bool Update(T entity, Expression<Func<T, bool>> whereExpression);

        Task<bool> UpdateAsync(T entity, Expression<Func<T, bool>> whereExpression);

        bool UpdateBatchById(List<T> entities, int batchSize = 100);

        Task<bool> UpdateBatchByIdAsync(List<T> entities, int batchSize = 100);

        bool SaveOrUpdate(T entity);

        Task<bool> SaveOrUpdateAsync(T entity);

        bool UpdateColumnsById(T entity, Expression<Func<T, object>> columnExpression);

        Task<bool> UpdateColumnsByIdAsync(T entity, Expression<Func<T, object>> columnExpression);

        bool UpdateColumnsById(T entity, params string[] columns);

        Task<bool> UpdateColumnsByIdAsync(T entity, params string[] columns);

        bool UpdateColumns(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression);

        Task<bool> UpdateColumnsAsync(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression);

        T? GetById(long id);

        Task<T?> GetByIdAsync(long id);

        T? GetById(string id);

        Task<T?> GetByIdAsync(string id);

        T? GetByCode(string code, Expression<Func<T, bool>>? whereExpression = null);

        Task<T?> GetByCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null);

        List<T> ListByIds(List<long> ids);

        Task<List<T>> ListByIdAsync(List<long> ids);

        List<T> ListByIds(List<string> ids);

        Task<List<T>> ListByIdAsync(List<string> ids);

        List<T> ListByMap(List<Dictionary<string, object>> whereColumnMaps);

        Task<List<T>> ListByMapAsync(List<Dictionary<string, object>> whereColumnMaps);

        T? GetOne(Expression<Func<T, bool>> whereExpression, bool throwEx = true);

        Task<T?> GetOneAsync(Expression<Func<T, bool>> whereExpression, bool throwEx = true);

        long Count(Expression<Func<T, bool>>? whereExpression = null);

        Task<long> CountAsync(Expression<Func<T, bool>>? whereExpression = null);

        List<T> List(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListAsync(Expression<Func<T, bool>>? whereExpression = null);

        List<TResult> ListObj<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<TResult>> ListObjAsync<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null);

        List<Dictionary<string, object>> ListMaps(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<Dictionary<string, object>>> ListMapsAsync(Expression<Func<T, bool>>? whereExpression = null);

        bool ExistById(long id);

        Task<bool> ExistByIdAsync(long id);

        bool ExistById(string id);

        Task<bool> ExistByIdAsync(string id);

        bool ExistById(T entity);

        Task<bool> ExistByIdAsync(T entity);

        bool ExistCode(string code, Expression<Func<T, bool>>? whereExpression = null);

        Task<bool> ExistCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null);

        bool Exist(Expression<Func<T, bool>> whereExpression);

        Task<bool> ExistAsync(Expression<Func<T, bool>> whereExpression);

        List<T> ListPage(int pageNumber, int pageSize, ref int totalNumber, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListPageAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? whereExpression = null);

        T? GetSingleOrDefault(Expression<Func<T, bool>> whereExpression);

        T? GetFirstOrDefault(Expression<Func<T, bool>> whereExpression);

        List<T> ListOrderByCode(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListOrderByCodeAsync(Expression<Func<T, bool>>? whereExpression = null);

        List<T> ListOrderByCodeDescending(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListOrderByCodeDescendingAsync(Expression<Func<T, bool>>? whereExpression = null);

        List<T> ListOrderBy(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListOrderByAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        List<T> ListOrderByDescending(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListOrderByDescendingAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult? GetObjFirst<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> whereExpression);

        Task<TResult?> GetObjFirstAsync<TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> whereExpression);

        T? GetFirst(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        Task<T?> GetFirstAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        T? GetLast(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        Task<T?> GetLastAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        List<T> ListTop(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListTopAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        List<T> ListLast(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> ListLastAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult Max<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult Max<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> MaxAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        TResult Min<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult Min<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> MinAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        TResult Sum<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult Sum<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SumAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        bool BulkSave(List<T> entities);

        Task<bool> BulkSaveAsync(List<T> entities);

        bool BulkUpdate(List<T> entities);

        Task<bool> BulkUpdateAsync(List<T> entities);

        bool BulkSaveOrUpdate(List<T> entities);

        Task<bool> BulkSaveOrUpdateAsync(List<T> entities);

        Type GetEntityType();
    }
}
