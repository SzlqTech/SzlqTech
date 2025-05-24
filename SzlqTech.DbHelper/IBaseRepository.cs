using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace SzlqTech.DbHelper
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        SqlSugarClient GetDb();

        int Insert(T entity);

        int Insert(List<T> entities);

        bool InsertBatch(List<T> entities, int batchSize = 100);

        long InsertReturnSnowflakeId(T entity);

        Task<int> InsertAsync(T entity);

        Task<int> InsertAsync(List<T> entities);

        Task<long> InsertReturnSnowflakeIdAsync(T entity);

        Task<bool> InsertBatchAsync(List<T> entities, int batchSize = 100);

        int DeleteById(long id);

        int DeleteById(string id);

        int DeleteById(T entity);

        bool DeleteBatchIds(List<long> ids);

        bool DeleteBatchIds(List<string> ids);

        bool DeleteBatchIds(List<T> entities);

        int DeleteByCode(string code);

        int DeleteByMap(List<Dictionary<string, object>> whereColumnMaps);

        int Delete(Expression<Func<T, bool>> whereExpression);

        int DeleteAll();

        Task<int> DeleteByIdAsync(long id);

        Task<int> DeleteByIdAsync(string id);

        Task<int> DeleteByIdAsync(T entity);

        Task<bool> DeleteBatchIdsAsync(List<long> ids);

        Task<bool> DeleteBatchIdsAsync(List<string> ids);

        Task<bool> DeleteBatchIdsAsync(List<T> entities);

        Task<int> DeleteByCodeAsync(string code);

        Task<int> DeleteByMapAsync(List<Dictionary<string, object>> whereColumnMaps);

        Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> DeleteAllAsync();

        int UpdateById(T entity);

        int Update(T entity, Expression<Func<T, bool>> whereExpression);

        int UpdateColumnsById(T entity, Expression<Func<T, object>> columnExpression);

        int UpdateColumnsById(T entity, params string[] columns);

        int UpdateColumns(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression);

        int UpdateColumns(Expression<Func<T, T>> setColumnsExpression, Expression<Func<T, bool>> whereExpression);

        bool UpdateBatchById(List<T> entities, int batchSize = 100);

        Task<int> UpdateByIdAsync(T entity);

        Task<int> UpdateAsync(T entity, Expression<Func<T, bool>> whereExpression);

        Task<int> UpdateColumnsByIdAsync(T entity, Expression<Func<T, object>> columnExpression);

        Task<int> UpdateColumnsByIdAsync(T entity, params string[] columns);

        Task<int> UpdateColumnsAsync(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression);

        Task<int> UpdateColumnsAsync(Expression<Func<T, T>> setColumnsExpression, Expression<Func<T, bool>> whereExpression);

        Task<bool> UpdateBatchByIdAsync(List<T> entities, int batchSize = 100);

        T? SelectById(long id);

        T? SelectById(string id);

        T? SelectByCode(string code, Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectBatchIds(List<long> ids);

        List<T> SelectBatchIds(List<string> ids);

        List<T> SelectByMap(List<Dictionary<string, object>> whereColumnMaps);

        T? SelectOne(Expression<Func<T, bool>> whereExpression);

        T? SelectFirst(Expression<Func<T, bool>> whereExpression);

        bool ExistById(long id);

        bool ExistById(string id);

        bool ExistById(T entity);

        bool ExistCode(string code, Expression<Func<T, bool>>? whereExpression = null);

        bool Exist(Expression<Func<T, bool>> whereExpression);

        int SelectCount(Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectList(Expression<Func<T, bool>>? whereExpression = null);

        List<TResult> SelectObjList<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<T?> SelectByIdAsync(long id);

        Task<T?> SelectByIdAsync(string id);

        Task<T?> SelectByCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectBatchIdsAsync(List<long> ids);

        Task<List<T>> SelectBatchIdsAsync(List<string> ids);

        Task<List<T>> SelectByMapAsync(List<Dictionary<string, object>> whereColumnMaps);

        Task<T?> SelectOneAsync(Expression<Func<T, bool>> whereExpression);

        Task<T?> SelectFirstAsync(Expression<Func<T, bool>> whereExpression);

        Task<bool> ExistByIdAsync(long id);

        Task<bool> ExistByIdAsync(string id);

        Task<bool> ExistByIdAsync(T entity);

        Task<bool> ExistCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null);

        Task<bool> ExistAsync(Expression<Func<T, bool>> whereExpression);

        Task<int> SelectCountAsync(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectListAsync(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<TResult>> SelectObjListAsync<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectPageList(int pageNumber, int pageSize, ref int totalNumber, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectPageListAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? whereExpression = null);

        int InsertOrUpdate(T entity);

        Task<int> InsertOrUpdateAsync(T entity);

        int InsertOrUpdate(List<T> entities);

        Task<int> InsertOrUpdateAsync(List<T> entities);

        bool InsertOrUpdateBatch(List<T> entities, int batchSize = 100);

        Task<bool> InsertOrUpdateBatchAsync(List<T> entities, int batchSize = 100);

        int InsertNotExist(List<T> entities);

        Task<int> InsertNotExistAsync(List<T> entities);

        bool InsertBatchNotExist(List<T> entities, int batchSize = 100);

        Task<bool> InsertBatchNotExistAsync(List<T> entities, int batchSize = 100);

        int BulkInsert(List<T> entities);

        Task<int> BulkInsertAsync(List<T> entities);

        int BulkUpdate(List<T> entities);

        Task<int> BulkUpdateAsync(List<T> entities);

        int BulkInsertOrUpdate(List<T> entities);

        Task<int> BulkInsertOrUpdateAsync(List<T> entities);

        List<Dictionary<string, object>> SelectMapList(Expression<Func<T, bool>>? whereExpression = null);

        Dictionary<string, object> SelectMap(Expression<Func<T, object>> keyExpression, Expression<Func<T, object>> valueExpression, Expression<Func<T, bool>>? whereExpression = null);

        DataTable SelectDataTable(Expression<Func<T, bool>>? whereExpression = null);

        string SelectJson(Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectListOrderByCode(Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectListOrderByCodeDescending(Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectListOrderBy(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectListOrderByDescending(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult? SelectObjFirst<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>> whereExpression);

        T? SelectFirst(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        T? SelectLast(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        List<T> SelectTopList(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        List<T> SelectLastList(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult SelectMax<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult SelectMax<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        TResult SelectMin<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult SelectMin<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        TResult SelectSum<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null);

        TResult SelectSum<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<Dictionary<string, object>>> SelectMapListAsync(Expression<Func<T, bool>>? whereExpression = null);

        Task<Dictionary<string, object>> SelectMapAsync(Expression<Func<T, object>> keyExpression, Expression<Func<T, object>> valueExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<DataTable> SelectDataTableAsync(Expression<Func<T, bool>>? whereExpression = null);

        Task<string> SelectJsonAsync(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectListOrderByCodeAsync(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectListOrderByCodeDescendingAsync(Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectListOrderByAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectListOrderByDescendingAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult?> SelectObjFirstAsync<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>> whereExpression);

        Task<T?> SelectFirstAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        Task<T?> SelectLastAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression);

        Task<List<T>> SelectTopListAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectLastListAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SelectMaxAsync<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SelectMaxAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SelectMinAsync<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SelectMinAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SelectSumAsync<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null);

        Task<TResult> SelectSumAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null);

        T SelectSingle(Expression<Func<T, bool>> whereExpression);

        T? SelectSingleOrDefault(Expression<Func<T, bool>> whereExpression);

        T? SelectFirstOrDefault(Expression<Func<T, bool>> whereExpression);

        int Execute(string sql, object? parameters = null);

        int Execute(string sql, params SugarParameter[] parameters);

        int Execute(string sql, List<SugarParameter> parameters);

        List<T> Query(string sql, object? parameters = null);

        List<T> Query(string sql, params SugarParameter[] parameters);

        List<T> Query(string sql, List<SugarParameter> parameters);

        List<TResult> Query<TResult>(string sql, object? parameters = null);

        List<TResult> Query<TResult>(string sql, params SugarParameter[] parameters);

        List<TResult> Query<TResult>(string sql, List<SugarParameter> parameters);

        T? QueryFirstOrDefault(string sql, object? parameters = null);

        T? QueryFirstOrDefault(string sql, params SugarParameter[] parameters);

        T? QueryFirstOrDefault(string sql, List<SugarParameter> parameters);

        TResult? QueryFirstOrDefault<TResult>(string sql, object? parameters = null);

        TResult? QueryFirstOrDefault<TResult>(string sql, params SugarParameter[] parameters);

        TResult? QueryFirstOrDefault<TResult>(string sql, List<SugarParameter> parameters);

        TResult QuerySingle<TResult>(string sql, object? parameters = null);

        T QuerySingle(string sql, object? parameters = null);

        TResult? QuerySingleOrDefault<TResult>(string sql, object? parameters = null);

        T? QuerySingleOrDefault(string sql, object? parameters = null);

        TResult QueryFirst<TResult>(string sql, object? parameters = null);

        T QueryFirst(string sql, object? parameters = null);

        Task<int> ExecuteAsync(string sql, object? parameters = null);

        Task<int> ExecuteAsync(string sql, params SugarParameter[] parameters);

        Task<int> ExecuteAsync(string sql, List<SugarParameter> parameters);

        Task<List<T>> QueryAsync(string sql, object? parameters = null);

        Task<List<T>> QueryAsync(string sql, params SugarParameter[] parameters);

        Task<List<T>> QueryAsync(string sql, List<SugarParameter> parameters);

        Task<List<TResult>> QueryAsync<TResult>(string sql, object? parameters = null);

        Task<List<TResult>> QueryAsync<TResult>(string sql, params SugarParameter[] parameters);

        Task<List<TResult>> QueryAsync<TResult>(string sql, List<SugarParameter> parameters);

        Task<T?> QueryFirstOrDefaultAsync(string sql, object? parameters = null);

        Task<T?> QueryFirstOrDefaultAsync(string sql, params SugarParameter[] parameters);

        Task<T?> QueryFirstOrDefaultAsync(string sql, List<SugarParameter> parameters);

        Task<TResult?> QueryFirstOrDefaultAsync<TResult>(string sql, object? parameters = null);

        Task<TResult?> QueryFirstOrDefaultAsync<TResult>(string sql, params SugarParameter[] parameters);

        Task<TResult?> QueryFirstOrDefaultAsync<TResult>(string sql, List<SugarParameter> parameters);

        Task<TResult> QuerySingleAsync<TResult>(string sql, object? parameters = null);

        Task<T> QuerySingleAsync(string sql, object? parameters = null);

        Task<TResult?> QuerySingleOrDefaultAsync<TResult>(string sql, object? parameters = null);

        Task<T?> QuerySingleOrDefaultAsync(string sql, object? parameters = null);

        Task<TResult> QueryFirstAsync<TResult>(string sql, object? parameters = null);

        Task<T> QueryFirstAsync(string sql, object? parameters = null);
    }
}
