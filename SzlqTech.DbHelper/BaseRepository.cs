using NLog;
using SqlSugar;
using System.Configuration;
using System.Data;
using System.Linq.Expressions;

namespace SzlqTech.DbHelper
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        private string? _dbConnectionStr;

        protected readonly string TransactionFail = "事务执行失败已回退";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected virtual string DbConnectionStr
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_dbConnectionStr))
                {
                    return _dbConnectionStr;
                }

                if (DbAndApiAuthConfig.Config.New)
                {
                    _dbConnectionStr = DbAndApiAuthConfig.Config.DbConnectionString;
                    return _dbConnectionStr;
                }

                _dbConnectionStr = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
                return _dbConnectionStr;
            }
        }

        public virtual SqlSugar.DbType DbType
        {
            get
            {
                if (!DbAndApiAuthConfig.Config.New)
                {
                    return SqlSugar.DbType.SqlServer;
                }

                return DbAndApiAuthConfig.Config.DbType;
            }
        }

        public virtual SqlSugarClient GetDb()
        {
            return new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = DbConnectionStr,
                DbType = DbType,
                IsAutoCloseConnection = true
            }, delegate (SqlSugarClient db)
            {
                db.Aop.OnLogExecuting = delegate (string sql, SugarParameter[] pars)
                {
                    logger.Trace("db.Aop.OnLogExecuting");
                    logger.Trace(UtilMethods.GetNativeSql(sql, pars));
                };
                db.Aop.OnLogExecuted = delegate
                {
                    logger.Trace("db.Aop.OnLogExecuted");
                    logger.Trace("time:" + db.Ado.SqlExecutionTime.ToString());
                };
                db.Aop.OnError = delegate (SqlSugarException exp)
                {
                    logger.Debug("db.Aop.OnError");
                    logger.Debug(UtilMethods.GetNativeSql(exp.Sql, (SugarParameter[])exp.Parametres));
                };
            });
        }

        public virtual int DeleteAll()
        {
            return GetDb().Deleteable<T>().ExecuteCommand();
        }

        public virtual async Task<int> DeleteAllAsync()
        {
            return await GetDb().Deleteable<T>().ExecuteCommandAsync();
        }

        public virtual int Delete(Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Deleteable<T>().Where(whereExpression).ExecuteCommand();
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Deleteable<T>().Where(whereExpression).ExecuteCommandAsync();
        }

        public virtual bool DeleteBatchIds(List<long> ids)
        {
            List<long> ids2 = ids;
            SqlSugarClient db = GetDb();
            if (db.UseTran(delegate
            {
                db.Deleteable<T>().In(ids2).ExecuteCommand();
            }).IsSuccess)
            {
                return true;
            }

            db.RollbackTran();
            return false;
        }

        public virtual bool DeleteBatchIds(List<string> ids)
        {
            List<string> ids2 = ids;
            SqlSugarClient db = GetDb();
            if (db.UseTran(delegate
            {
                db.Deleteable<T>().In(ids2).ExecuteCommand();
            }).IsSuccess)
            {
                return true;
            }

            db.RollbackTran();
            return false;
        }

        public virtual bool DeleteBatchIds(List<T> entities)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            if (db.UseTran(delegate
            {
                db.Deleteable(entities2).ExecuteCommand();
            }).IsSuccess)
            {
                return true;
            }

            db.RollbackTran();
            return false;
        }

        public int DeleteByCode(string code)
        {
            string code2 = code;
            return (from o in GetDb().Deleteable<T>()
                    where o.Code == code2
                    select o).ExecuteCommand();
        }

        public virtual async Task<bool> DeleteBatchIdsAsync(List<long> ids)
        {
            List<long> ids2 = ids;
            SqlSugarClient db = GetDb();
            if ((await db.UseTranAsync(async delegate
            {
                await db.Deleteable<T>().In(ids2).ExecuteCommandAsync();
            })).IsSuccess)
            {
                return true;
            }

            await db.RollbackTranAsync();
            return false;
        }

        public virtual async Task<bool> DeleteBatchIdsAsync(List<string> ids)
        {
            List<string> ids2 = ids;
            SqlSugarClient db = GetDb();
            if ((await db.UseTranAsync(async delegate
            {
                await db.Deleteable<T>().In(ids2).ExecuteCommandAsync();
            })).IsSuccess)
            {
                return true;
            }

            await db.RollbackTranAsync();
            return false;
        }

        public virtual async Task<bool> DeleteBatchIdsAsync(List<T> entities)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            if ((await db.UseTranAsync(async delegate
            {
                await db.Deleteable(entities2).ExecuteCommandAsync();
            })).IsSuccess)
            {
                return true;
            }

            await db.RollbackTranAsync();
            return false;
        }

        public async Task<int> DeleteByCodeAsync(string code)
        {
            string code2 = code;
            return await (from o in GetDb().Deleteable<T>()
                          where o.Code == code2
                          select o).ExecuteCommandAsync();
        }

        public virtual async Task<long> InsertReturnSnowflakeIdAsync(T entity)
        {
            return await GetDb().Insertable(entity).ExecuteReturnSnowflakeIdAsync();
        }

        public virtual async Task<bool> InsertBatchAsync(List<T> entities, int batchSize)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            try
            {
                if ((await db.UseTranAsync(async delegate
                {
                    await db.Utilities.PageEachAsync(entities2, batchSize, async delegate (List<T> pageList)
                    {
                        await db.Insertable(pageList).ExecuteCommandAsync();
                    });
                })).IsSuccess)
                {
                    return true;
                }

                await db.RollbackTranAsync();
                return false;
            }
            finally
            {
                if (db != null)
                {
                    ((IDisposable)db).Dispose();
                }
            }
        }

        public virtual int DeleteById(long id)
        {
            return GetDb().Deleteable<T>().In(id).ExecuteCommand();
        }

        public virtual int DeleteById(string id)
        {
            return GetDb().Deleteable<T>().In(id).ExecuteCommand();
        }

        public virtual int DeleteById(T entity)
        {
            return GetDb().Deleteable(entity).ExecuteCommand();
        }

        public virtual async Task<int> DeleteByIdAsync(long id)
        {
            return await GetDb().Deleteable<T>().In(id).ExecuteCommandAsync();
        }

        public virtual async Task<int> DeleteByIdAsync(string id)
        {
            return await GetDb().Deleteable<T>().In(id).ExecuteCommandAsync();
        }

        public virtual async Task<int> DeleteByIdAsync(T entity)
        {
            return await GetDb().Deleteable(entity).ExecuteCommandAsync();
        }

        public virtual int DeleteByMap(List<Dictionary<string, object>> whereColumnMaps)
        {
            return GetDb().Deleteable<T>().WhereColumns(whereColumnMaps).ExecuteCommand();
        }

        public virtual async Task<int> DeleteByMapAsync(List<Dictionary<string, object>> whereColumnMaps)
        {
            return await GetDb().Deleteable<T>().WhereColumns(whereColumnMaps).ExecuteCommandAsync();
        }

        public bool ExistCode(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            string code2 = code;
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Any((T e) => e.Code == code2);
            }

            return db.Queryable<T>().Any((T e) => e.Code == code2);
        }

        public bool Exist(Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().Any(whereExpression);
        }

        public virtual int Insert(T entity)
        {
            return GetDb().Insertable(entity).ExecuteCommand();
        }

        public virtual int Insert(List<T> entities)
        {
            return GetDb().Insertable(entities).ExecuteCommand();
        }

        public bool InsertBatch(List<T> entities, int batchSize = 1000)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            if (db.UseTran(delegate
            {
                db.Utilities.PageEach(entities2, batchSize, delegate (List<T> pageList)
                {
                    db.Insertable(pageList).ExecuteCommand();
                });
            }).IsSuccess)
            {
                return true;
            }

            db.RollbackTran();
            return false;
        }

        public long InsertReturnSnowflakeId(T entity)
        {
            return GetDb().Insertable(entity).ExecuteReturnSnowflakeId();
        }

        public virtual async Task<int> InsertAsync(T entity)
        {
            return await GetDb().Insertable(entity).ExecuteCommandAsync();
        }

        public virtual async Task<int> InsertAsync(List<T> entities)
        {
            return await GetDb().Insertable(entities).ExecuteCommandAsync();
        }

        public virtual T? SelectById(string id)
        {
            return GetDb().Queryable<T>().InSingle(id);
        }

        public virtual T? SelectByCode(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            string code2 = code;
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return (from o in db.Queryable<T>().Where(whereExpression)
                        where o.Code == code2
                        select o).First();
            }

            return db.Queryable<T>().First((T o) => o.Code == code2);
        }

        public virtual List<T> SelectBatchIds(List<long> ids)
        {
            return GetDb().Queryable<T>().In(ids).ToList();
        }

        public virtual List<T> SelectBatchIds(List<string> ids)
        {
            return GetDb().Queryable<T>().In(ids).ToList();
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Updateable<T>().SetColumns(setColumnsExpression).Where(whereExpression)
                .ExecuteCommandAsync();
        }

        public virtual async Task<int> UpdateColumnsAsync(Expression<Func<T, T>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Updateable<T>().SetColumns(setColumnsExpression).Where(whereExpression)
                .ExecuteCommandAsync();
        }

        public async Task<bool> UpdateBatchByIdAsync(List<T> entities, int batchSize = 100)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            try
            {
                if ((await db.UseTranAsync(async delegate
                {
                    await db.Utilities.PageEachAsync(entities2, batchSize, async delegate (List<T> pageList)
                    {
                        await db.Updateable(pageList).ExecuteCommandAsync();
                    });
                })).IsSuccess)
                {
                    return true;
                }

                await db.RollbackTranAsync();
                return false;
            }
            finally
            {
                if (db != null)
                {
                    ((IDisposable)db).Dispose();
                }
            }
        }

        public virtual T SelectById(long id)
        {
            return GetDb().Queryable<T>().InSingle(id);
        }

        public virtual List<T> SelectByMap(List<Dictionary<string, object>> whereColumnMaps)
        {
            return GetDb().Queryable<T>().WhereColumns(whereColumnMaps).ToList();
        }

        public virtual int SelectCount(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Count();
            }

            return db.Queryable<T>().Count();
        }

        public virtual List<T> SelectList(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).ToList();
            }

            return db.Queryable<T>().ToList();
        }

        public virtual List<TResult> SelectObjList<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Select(selectExpression)
                    .ToList();
            }

            return db.Queryable<T>().Select(selectExpression).ToList();
        }

        public virtual async Task<T?> SelectByIdAsync(long id)
        {
            return await GetDb().Queryable<T>().InSingleAsync(id);
        }

        public virtual async Task<T?> SelectByIdAsync(string id)
        {
            return await GetDb().Queryable<T>().InSingleAsync(id);
        }

        public async Task<T?> SelectByCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            string code2 = code;
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await (from o in db.Queryable<T>().Where(whereExpression)
                                                       where o.Code == code2
                                                       select o).FirstAsync()) : (await db.Queryable<T>().FirstAsync((T o) => o.Code == code2));
        }

        public virtual async Task<List<T>> SelectBatchIdsAsync(List<long> ids)
        {
            return await GetDb().Queryable<T>().In(ids).ToListAsync();
        }

        public virtual async Task<List<T>> SelectBatchIdsAsync(List<string> ids)
        {
            return await GetDb().Queryable<T>().In(ids).ToListAsync();
        }

        public virtual async Task<List<T>> SelectByMapAsync(List<Dictionary<string, object>> whereColumnMaps)
        {
            return await GetDb().Queryable<T>().WhereColumns(whereColumnMaps).ToListAsync();
        }

        public virtual async Task<T?> SelectOneAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Queryable<T>().SingleAsync(whereExpression);
        }

        public virtual async Task<T?> SelectFirstAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Queryable<T>().FirstAsync(whereExpression);
        }

        public virtual async Task<bool> ExistByIdAsync(long id)
        {
            await Task.Delay(0);
            throw new InvalidOperationException();
        }

        public virtual async Task<bool> ExistByIdAsync(string id)
        {
            await Task.Delay(0);
            throw new InvalidOperationException();
        }

        public virtual async Task<bool> ExistByIdAsync(T entity)
        {
            await Task.Delay(0);
            throw new InvalidOperationException();
        }

        public async Task<bool> ExistCodeAsync(string code, Expression<Func<T, bool>>? whereExpression = null)
        {
            string code2 = code;
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).AnyAsync((T e) => e.Code == code2)) : (await db.Queryable<T>().AnyAsync((T e) => e.Code == code2));
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Queryable<T>().AnyAsync(whereExpression);
        }

        public virtual async Task<int> SelectCountAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).CountAsync()) : (await db.Queryable<T>().CountAsync());
        }

        public virtual async Task<List<T>> SelectListAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).ToListAsync()) : (await db.Queryable<T>().ToListAsync());
        }

        public virtual async Task<List<TResult>> SelectObjListAsync<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).Select(selectExpression)
                .ToListAsync()) : (await db.Queryable<T>().Select(selectExpression).ToListAsync());
        }

        public string SelectJson(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).ToJson();
            }

            return db.Queryable<T>().ToJson();
        }

        public List<T> SelectListOrderByCode(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return (from t in db.Queryable<T>().Where(whereExpression)
                        orderby t.Code
                        select t).ToList();
            }

            return (from t in db.Queryable<T>()
                    orderby t.Code
                    select t).ToList();
        }

        public List<T> SelectListOrderByCodeDescending(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return (from t in db.Queryable<T>().Where(whereExpression)
                        orderby t.Code descending
                        select t).ToList();
            }

            return (from t in db.Queryable<T>()
                    orderby t.Code descending
                    select t).ToList();
        }

        public List<T> SelectListOrderBy(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).OrderBy(orderExpression)
                    .ToList();
            }

            return db.Queryable<T>().OrderBy(orderExpression).ToList();
        }

        public List<T> SelectListOrderByDescending(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).OrderByDescending(orderExpression)
                    .ToList();
            }

            return db.Queryable<T>().OrderByDescending(orderExpression).ToList();
        }

        public TResult? SelectObjFirst<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().Where(whereExpression).Select(selectExpression)
                .First();
        }

        public T? SelectFirst(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().OrderBy(orderExpression).First(whereExpression);
        }

        public T? SelectLast(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().OrderByDescending(orderExpression).First(whereExpression);
        }

        public List<T> SelectTopList(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Take(num).Where(whereExpression)
                    .OrderBy(orderExpression)
                    .ToList();
            }

            return db.Queryable<T>().Take(num).OrderBy(orderExpression)
                .ToList();
        }

        public List<T> SelectLastList(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Take(num).Where(whereExpression)
                    .OrderByDescending(orderExpression)
                    .ToList();
            }

            return db.Queryable<T>().Take(num).OrderByDescending(orderExpression)
                .ToList();
        }

        public TResult SelectMax<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Max(maxExpression);
            }

            return db.Queryable<T>().Max(maxExpression);
        }

        public TResult SelectMax<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Max<TResult>(column);
            }

            return db.Queryable<T>().Max<TResult>(column);
        }

        public TResult SelectMin<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Min(minExpression);
            }

            return db.Queryable<T>().Min(minExpression);
        }

        public TResult SelectMin<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Min<TResult>(column);
            }

            return db.Queryable<T>().Min<TResult>(column);
        }

        public TResult SelectSum<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Sum(sumExpression);
            }

            return db.Queryable<T>().Sum(sumExpression);
        }

        public TResult SelectSum<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).Sum<TResult>(column);
            }

            return db.Queryable<T>().Sum<TResult>(column);
        }

        public virtual async Task<List<Dictionary<string, object>>> SelectMapListAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).ToDictionaryListAsync()) : (await db.Queryable<T>().ToDictionaryListAsync());
        }

        public async Task<Dictionary<string, object>> SelectMapAsync(Expression<Func<T, object>> keyExpression, Expression<Func<T, object>> valueExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).ToDictionaryAsync(keyExpression, valueExpression)) : (await db.Queryable<T>().ToDictionaryAsync(keyExpression, valueExpression));
        }

        public async Task<DataTable> SelectDataTableAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).ToDataTableAsync()) : (await db.Queryable<T>().ToDataTableAsync());
        }

        public async Task<string> SelectJsonAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).ToJsonAsync()) : (await db.Queryable<T>().ToJsonAsync());
        }

        public async Task<List<T>> SelectListOrderByCodeAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await (from t in db.Queryable<T>().Where(whereExpression)
                                                       orderby t.Code
                                                       select t).ToListAsync()) : (await (from t in db.Queryable<T>()
                                                                                          orderby t.Code
                                                                                          select t).ToListAsync());
        }

        public async Task<List<T>> SelectListOrderByCodeDescendingAsync(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await (from t in db.Queryable<T>().Where(whereExpression)
                                                       orderby t.Code descending
                                                       select t).ToListAsync()) : (await (from t in db.Queryable<T>()
                                                                                          orderby t.Code descending
                                                                                          select t).ToListAsync());
        }

        public async Task<List<T>> SelectListOrderByAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).OrderBy(orderExpression)
                .ToListAsync()) : (await db.Queryable<T>().OrderBy(orderExpression).ToListAsync());
        }

        public async Task<List<T>> SelectListOrderByDescendingAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).OrderByDescending(orderExpression)
                .ToListAsync()) : (await db.Queryable<T>().OrderByDescending(orderExpression).ToListAsync());
        }

        public async Task<TResult?> SelectObjFirstAsync<TResult>(Expression<Func<T, TResult>> selectExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Queryable<T>().Where(whereExpression).Select(selectExpression)
                .FirstAsync();
        }

        public async Task<T?> SelectFirstAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Queryable<T>().Where(whereExpression).OrderBy(orderExpression)
                .FirstAsync();
        }

        public async Task<T?> SelectLastAsync(Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Queryable<T>().Where(whereExpression).OrderByDescending(orderExpression)
                .FirstAsync();
        }

        public async Task<List<T>> SelectTopListAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Take(num).Where(whereExpression)
                .OrderBy(orderExpression)
                .ToListAsync()) : (await db.Queryable<T>().Take(num).OrderBy(orderExpression)
                .ToListAsync());
        }

        public async Task<List<T>> SelectLastListAsync(int num, Expression<Func<T, object>> orderExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Take(num).Where(whereExpression)
                .OrderByDescending(orderExpression)
                .ToListAsync()) : (await db.Queryable<T>().Take(num).OrderByDescending(orderExpression)
                .ToListAsync());
        }

        public async Task<TResult> SelectMaxAsync<TResult>(Expression<Func<T, TResult>> maxExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).MaxAsync(maxExpression)) : (await db.Queryable<T>().MaxAsync(maxExpression));
        }

        public async Task<TResult> SelectMaxAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).MaxAsync<TResult>(column)) : (await db.Queryable<T>().MaxAsync<TResult>(column));
        }

        public async Task<TResult> SelectMinAsync<TResult>(Expression<Func<T, TResult>> minExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).MinAsync(minExpression)) : (await db.Queryable<T>().MinAsync(minExpression));
        }

        public async Task<TResult> SelectMinAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).MinAsync<TResult>(column)) : (await db.Queryable<T>().MinAsync<TResult>(column));
        }

        public async Task<TResult> SelectSumAsync<TResult>(Expression<Func<T, TResult>> sumExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).SumAsync(sumExpression)) : (await db.Queryable<T>().SumAsync(sumExpression));
        }

        public async Task<TResult> SelectSumAsync<TResult>(string column, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).SumAsync<TResult>(column)) : (await db.Queryable<T>().SumAsync<TResult>(column));
        }

        public virtual T SelectSingle(Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().Single(whereExpression) ?? throw new InvalidOperationException("返回结果为空");
        }

        public virtual T? SelectSingleOrDefault(Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().Single(whereExpression);
        }

        public virtual T? SelectFirstOrDefault(Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().First(whereExpression);
        }

        public virtual List<T> SelectPageList(int pageNumber, int pageSize, ref int totalNumber, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).ToPageList(pageNumber, pageSize, ref totalNumber);
            }

            return db.Queryable<T>().ToPageList(pageNumber, pageSize, ref totalNumber);
        }

        public async Task<List<T>> SelectPageListAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            return (whereExpression != null) ? (await db.Queryable<T>().Where(whereExpression).ToPageListAsync(pageNumber, pageSize)) : (await db.Queryable<T>().ToPageListAsync(pageNumber, pageSize));
        }

        public virtual int InsertOrUpdate(T entity)
        {
            return GetDb().Storageable(entity).ExecuteCommand();
        }

        public virtual async Task<int> InsertOrUpdateAsync(T entity)
        {
            return await GetDb().Storageable(entity).ExecuteCommandAsync();
        }

        public virtual int InsertOrUpdate(List<T> entities)
        {
            return GetDb().Storageable(entities).ExecuteCommand();
        }

        public virtual async Task<int> InsertOrUpdateAsync(List<T> entities)
        {
            return await GetDb().Storageable(entities).ExecuteCommandAsync();
        }

        public bool InsertOrUpdateBatch(List<T> entities, int batchSize = 1000)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            if (db.UseTran(delegate
            {
                db.Utilities.PageEach(entities2, batchSize, delegate (List<T> pageList)
                {
                    db.Storageable(pageList).ExecuteCommand();
                });
            }).IsSuccess)
            {
                return true;
            }

            db.RollbackTran();
            return false;
        }

        public virtual async Task<bool> InsertOrUpdateBatchAsync(List<T> entities, int batchSize = 1000)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            if ((await db.UseTranAsync(async delegate
            {
                await db.Utilities.PageEachAsync(entities2, batchSize, async delegate (List<T> pageList)
                {
                    await db.Storageable(pageList).ExecuteCommandAsync();
                });
            })).IsSuccess)
            {
                return true;
            }

            await db.RollbackTranAsync();
            return false;
        }

        public virtual int InsertNotExist(List<T> entities)
        {
            return GetDb().Storageable(entities).ToStorage().AsInsertable.ExecuteCommand();
        }

        public virtual async Task<int> InsertNotExistAsync(List<T> entities)
        {
            return await (await GetDb().Storageable(entities).ToStorageAsync()).AsInsertable.ExecuteCommandAsync();
        }

        public bool InsertBatchNotExist(List<T> entities, int batchSize = 100)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            if (db.UseTran(delegate
            {
                db.Utilities.PageEach(entities2, batchSize, delegate (List<T> pageList)
                {
                    db.Storageable(pageList).ToStorage().AsInsertable.ExecuteCommand();
                });
            }).IsSuccess)
            {
                return true;
            }

            db.RollbackTran();
            return false;
        }

        public async Task<bool> InsertBatchNotExistAsync(List<T> entities, int batchSize = 100)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            if ((await db.UseTranAsync(async delegate
            {
                await db.Utilities.PageEachAsync(entities2, batchSize, async delegate (List<T> pageList)
                {
                    await (await db.Storageable(pageList).ToStorageAsync()).AsInsertable.ExecuteCommandAsync();
                });
            })).IsSuccess)
            {
                return true;
            }

            await db.RollbackTranAsync();
            return false;
        }

        public int BulkInsert(List<T> entities)
        {
            return GetDb().Fastest<T>().BulkCopy(entities);
        }

        public async Task<int> BulkInsertAsync(List<T> entities)
        {
            return await GetDb().Fastest<T>().BulkCopyAsync(entities);
        }

        public int BulkUpdate(List<T> entities)
        {
            return GetDb().Fastest<T>().BulkUpdate(entities);
        }

        public async Task<int> BulkUpdateAsync(List<T> entities)
        {
            return await GetDb().Fastest<T>().BulkUpdateAsync(entities);
        }

        public virtual int BulkInsertOrUpdate(List<T> entities)
        {
            return GetDb().Storageable(entities).ExecuteSqlBulkCopy();
        }

        public virtual async Task<int> BulkInsertOrUpdateAsync(List<T> entities)
        {
            return await GetDb().Storageable(entities).ExecuteSqlBulkCopyAsync();
        }

        public virtual List<Dictionary<string, object>> SelectMapList(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).ToDictionaryList();
            }

            return db.Queryable<T>().ToDictionaryList();
        }

        public Dictionary<string, object> SelectMap(Expression<Func<T, object>> keyExpression, Expression<Func<T, object>> valueExpression, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).ToDictionary(keyExpression, valueExpression);
            }

            return db.Queryable<T>().ToDictionary(keyExpression, valueExpression);
        }

        public DataTable SelectDataTable(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return db.Queryable<T>().Where(whereExpression).ToDataTable();
            }

            return db.Queryable<T>().ToDataTable();
        }

        public virtual T? SelectOne(Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().Single(whereExpression);
        }

        public virtual T? SelectFirst(Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Queryable<T>().First(whereExpression);
        }

        public virtual bool ExistById(long id)
        {
            throw new InvalidOperationException();
        }

        public virtual bool ExistById(string id)
        {
            return GetDb().Queryable<T>().Where("id=@Id", new
            {
                Id = id
            }).Any();
        }

        public virtual bool ExistById(T entity)
        {
            throw new InvalidOperationException();
        }

        public virtual int Update(T entity, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Updateable(entity).Where(whereExpression).ExecuteCommand();
        }

        public virtual int UpdateColumnsById(T entity, Expression<Func<T, object>> columnExpression)
        {
            return GetDb().Updateable(entity).UpdateColumns(columnExpression).ExecuteCommand();
        }

        public virtual int UpdateColumnsById(T entity, params string[] columns)
        {
            return GetDb().Updateable(entity).UpdateColumns(columns).ExecuteCommand();
        }

        public virtual int UpdateColumns(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Updateable<T>().SetColumns(setColumnsExpression).Where(whereExpression)
                .ExecuteCommand();
        }

        public virtual int UpdateColumns(Expression<Func<T, T>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Updateable<T>().SetColumns(setColumnsExpression).Where(whereExpression)
                .ExecuteCommand();
        }

        public bool UpdateBatchById(List<T> entities, int batchSize = 100)
        {
            List<T> entities2 = entities;
            SqlSugarClient db = GetDb();
            try
            {
                if (db.UseTran(delegate
                {
                    db.Utilities.PageEach(entities2, batchSize, delegate (List<T> pageList)
                    {
                        db.Updateable(pageList).ExecuteCommand();
                    });
                }).IsSuccess)
                {
                    return true;
                }

                db.RollbackTran();
                return false;
            }
            finally
            {
                if (db != null)
                {
                    ((IDisposable)db).Dispose();
                }
            }
        }

        public virtual async Task<int> UpdateAsync(T entity, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Updateable(entity).Where(whereExpression).ExecuteCommandAsync();
        }

        public virtual async Task<int> UpdateColumnsByIdAsync(T entity, Expression<Func<T, object>> columnExpression)
        {
            return await GetDb().Updateable(entity).UpdateColumns(columnExpression).ExecuteCommandAsync();
        }

        public virtual async Task<int> UpdateColumnsByIdAsync(T entity, params string[] columns)
        {
            return await GetDb().Updateable(entity).UpdateColumns(columns).ExecuteCommandAsync();
        }

        public virtual int UpdateById(T entity)
        {
            return GetDb().Updateable(entity).ExecuteCommand();
        }

        public virtual async Task<int> UpdateByIdAsync(T entity)
        {
            return await GetDb().Updateable(entity).ExecuteCommandAsync();
        }

        public int Execute(string sql, List<SugarParameter> parameters)
        {
            return GetDb().Ado.ExecuteCommand(sql, parameters);
        }

        public int Execute(string sql, object? parameters = null)
        {
            return GetDb().Ado.ExecuteCommand(sql, parameters);
        }

        public int Execute(string sql, params SugarParameter[] parameters)
        {
            return GetDb().Ado.ExecuteCommand(sql, parameters);
        }

        public List<T> Query(string sql, List<SugarParameter> parameters)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters);
        }

        public List<T> Query(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters);
        }

        public List<T> Query(string sql, params SugarParameter[] parameters)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters);
        }

        public T? QueryFirstOrDefault(string sql, List<SugarParameter> parameters)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        public T? QueryFirstOrDefault(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        public T? QueryFirstOrDefault(string sql, params SugarParameter[] parameters)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        public List<TResult> Query<TResult>(string sql, List<SugarParameter> parameters)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters);
        }

        public List<TResult> Query<TResult>(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters);
        }

        public List<TResult> Query<TResult>(string sql, params SugarParameter[] parameters)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters);
        }

        public TResult QueryFirstOrDefault<TResult>(string sql, List<SugarParameter> parameters)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters).FirstOrDefault();
        }

        public TResult QuerySingle<TResult>(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters).Single();
        }

        public T QuerySingle(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters).Single();
        }

        public TResult? QuerySingleOrDefault<TResult>(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters).SingleOrDefault();
        }

        public T? QuerySingleOrDefault(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters).SingleOrDefault();
        }

        public TResult QueryFirst<TResult>(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters).First();
        }

        public T QueryFirst(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<T>(sql, parameters).First();
        }

        public TResult QueryFirstOrDefault<TResult>(string sql, object? parameters = null)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters).FirstOrDefault();
        }

        public TResult QueryFirstOrDefault<TResult>(string sql, params SugarParameter[] parameters)
        {
            return GetDb().Ado.SqlQuery<TResult>(sql, parameters).FirstOrDefault();
        }

        public async Task<int> ExecuteAsync(string sql, List<SugarParameter> parameters)
        {
            return await GetDb().Ado.ExecuteCommandAsync(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            return await GetDb().Ado.ExecuteCommandAsync(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, params SugarParameter[] parameters)
        {
            return await GetDb().Ado.ExecuteCommandAsync(sql, parameters);
        }

        public async Task<List<T>> QueryAsync(string sql, List<SugarParameter> parameters)
        {
            return await GetDb().Ado.SqlQueryAsync<T>(sql, parameters);
        }

        public async Task<List<T>> QueryAsync(string sql, object? parameters = null)
        {
            return await GetDb().Ado.SqlQueryAsync<T>(sql, parameters);
        }

        public async Task<List<T>> QueryAsync(string sql, params SugarParameter[] parameters)
        {
            return await GetDb().Ado.SqlQueryAsync<T>(sql, parameters);
        }

        public async Task<T?> QueryFirstOrDefaultAsync(string sql, List<SugarParameter> parameters)
        {
            return (await GetDb().Ado.SqlQueryAsync<T>(sql, parameters)).FirstOrDefault();
        }

        public async Task<T?> QueryFirstOrDefaultAsync(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<T>(sql, parameters)).FirstOrDefault();
        }

        public async Task<T?> QueryFirstOrDefaultAsync(string sql, params SugarParameter[] parameters)
        {
            return (await GetDb().Ado.SqlQueryAsync<T>(sql, parameters)).FirstOrDefault();
        }

        public async Task<List<TResult>> QueryAsync<TResult>(string sql, List<SugarParameter> parameters)
        {
            return await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters);
        }

        public async Task<List<TResult>> QueryAsync<TResult>(string sql, object? parameters = null)
        {
            return await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters);
        }

        public async Task<List<TResult>> QueryAsync<TResult>(string sql, params SugarParameter[] parameters)
        {
            return await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters);
        }

        public async Task<TResult?> QueryFirstOrDefaultAsync<TResult>(string sql, List<SugarParameter> parameters)
        {
            return (await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters)).FirstOrDefault();
        }

        public async Task<TResult> QuerySingleAsync<TResult>(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters)).Single();
        }

        public async Task<T> QuerySingleAsync(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<T>(sql, parameters)).Single();
        }

        public async Task<TResult?> QuerySingleOrDefaultAsync<TResult>(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters)).SingleOrDefault();
        }

        public async Task<T?> QuerySingleOrDefaultAsync(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<T>(sql, parameters)).SingleOrDefault();
        }

        public async Task<TResult> QueryFirstAsync<TResult>(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters)).First();
        }

        public async Task<T> QueryFirstAsync(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<T>(sql, parameters)).First();
        }

        public async Task<TResult?> QueryFirstOrDefaultAsync<TResult>(string sql, object? parameters = null)
        {
            return (await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters)).FirstOrDefault();
        }

        public async Task<TResult?> QueryFirstOrDefaultAsync<TResult>(string sql, params SugarParameter[] parameters)
        {
            return (await GetDb().Ado.SqlQueryAsync<TResult>(sql, parameters)).FirstOrDefault();
        }
    }
}
