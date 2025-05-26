using SqlSugar;
using System.Linq.Expressions;
using SzlqTech.Common.Context;
using SzlqTech.Entity;

namespace SzlqTech.DbHelper
{
    public class BaseAuditableRepository<T> : BaseRepository<T>, IBaseAuditableRepository<T>, IBaseRepository<T> where T : BaseAuditableEntity, new()
    {
        private const string deletedColumn = "deleted";

        private const string updateUserColumn = "update_user";

        private const string updateTimeColumn = "update_time";

        public override int DeleteById(long id)
        {
            SqlSugarClient db = GetDb();
            if (SqlHelper.IsWithLogicDelete(typeof(T)))
            {
                return db.Deleteable<T>().In(id).IsLogic()
                    .ExecuteCommand("deleted", DateTime.Now, "update_time", "update_user", UserContext.Username);
            }

            return db.Deleteable<T>().In(id).ExecuteCommand();
        }

        public override async Task<int> DeleteByIdAsync(long id)
        {
            SqlSugarClient db = GetDb();
            if (SqlHelper.IsWithLogicDelete(typeof(T)))
            {
                return await db.Deleteable<T>().In(id).IsLogic()
                    .ExecuteCommandAsync("deleted", DateTime.Now, "update_time", "update_user", UserContext.UserId);
            }

            return await db.Deleteable<T>().In(id).ExecuteCommandAsync();
        }

        public override int Delete(Expression<Func<T, bool>> whereExpression)
        {
            SqlSugarClient db = GetDb();
            if (SqlHelper.IsWithLogicDelete(typeof(T)))
            {
                return db.Deleteable<T>().Where(whereExpression).IsLogic()
                    .ExecuteCommand("deleted", DateTime.Now, "update_time", "update_user", UserContext.UserId);
            }

            return db.Deleteable<T>().Where(whereExpression).ExecuteCommand();
        }

        public override async Task<int> DeleteAsync(Expression<Func<T, bool>> whereExpression)
        {
            SqlSugarClient db = GetDb();
            if (SqlHelper.IsWithLogicDelete(typeof(T)))
            {
                return await db.Deleteable<T>().Where(whereExpression).IsLogic()
                    .ExecuteCommandAsync("deleted", DateTime.Now, "update_time", "update_user", UserContext.UserId);
            }

            return await db.Deleteable<T>().Where(whereExpression).ExecuteCommandAsync();
        }

        public override bool DeleteBatchIds(List<long> ids)
        {
            List<long> ids2 = ids;
            SqlSugarClient db = GetDb();
            if (db.UseTran(delegate
            {
                if (SqlHelper.IsWithLogicDelete(typeof(T)))
                {
                    db.Deleteable<T>().In(ids2).IsLogic()
                        .ExecuteCommand("deleted", DateTime.Now, "update_time", "update_user", UserContext.UserId);
                }
                else
                {
                    db.Deleteable<T>().In(ids2).ExecuteCommand();
                }
            }).IsSuccess)
            {
                return true;
            }

            db.RollbackTran();
            return false;
        }

        public override async Task<bool> DeleteBatchIdsAsync(List<long> ids)
        {
            List<long> ids2 = ids;
            SqlSugarClient db = GetDb();
            if ((await db.UseTranAsync(async delegate
            {
                if (SqlHelper.IsWithLogicDelete(typeof(T)))
                {
                    await db.Deleteable<T>().In(ids2).IsLogic()
                        .ExecuteCommandAsync("deleted", DateTime.Now, "update_time", "update_user", UserContext.UserId);
                }
                else
                {
                    await db.Deleteable<T>().In(ids2).ExecuteCommandAsync();
                }
            })).IsSuccess)
            {
                return true;
            }

            await db.RollbackTranAsync();
            return false;
        }

        public override int UpdateColumns(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Updateable<T>().SetColumns(setColumnsExpression).SetColumns((T o) => o.UpdateUser == UserContext.UserId)
                .SetColumns((T o) => o.UpdateTime == DateTime.Now)
                .Where(whereExpression)
                .ExecuteCommand();
        }

        public override async Task<int> UpdateColumnsAsync(Expression<Func<T, bool>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Updateable<T>().SetColumns(setColumnsExpression).SetColumns((T o) => o.UpdateUser == UserContext.UserId)
                .SetColumns((T o) => o.UpdateTime == DateTime.Now)
                .Where(whereExpression)
                .ExecuteCommandAsync();
        }

        public override int UpdateColumns(Expression<Func<T, T>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return GetDb().Updateable<T>().SetColumns(setColumnsExpression).SetColumns((T o) => o.UpdateUser == UserContext.UserId)
                .SetColumns((T o) => o.UpdateTime == DateTime.Now)
                .Where(whereExpression)
                .ExecuteCommand();
        }

        public override async Task<int> UpdateColumnsAsync(Expression<Func<T, T>> setColumnsExpression, Expression<Func<T, bool>> whereExpression)
        {
            return await GetDb().Updateable<T>().SetColumns(setColumnsExpression).SetColumns((T o) => o.UpdateUser == UserContext.UserId)
                .SetColumns((T o) => o.UpdateTime == DateTime.Now)
                .Where(whereExpression)
                .ExecuteCommandAsync();
        }

        public virtual List<T> SelectListByStatus(int status, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return (from o in db.Queryable<T>()
                        where o.Status == status
                        select o).Where(whereExpression).ToList();
            }

            return (from o in db.Queryable<T>()
                    where o.Status == status
                    select o).ToList();
        }

        public virtual async Task<List<T>> SelectListByStatusAsync(int status, Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression == null)
            {
                return await (from o in db.Queryable<T>()
                              where o.Status == status
                              select o).ToListAsync();
            }

            return await (from o in db.Queryable<T>()
                          where o.Status == status
                          select o).Where(whereExpression).ToListAsync();
        }

        public virtual int UpdateStatusById(T entity, int status)
        {
            throw new InvalidOperationException();
        }

        public virtual int UpdateStatusById(long id, int status)
        {
            throw new InvalidOperationException();
        }

        public virtual int UpdateStatusById(string id, int status)
        {
            throw new InvalidOperationException();
        }

        public virtual int UpdateDeleted(int deleted = 0, Expression<Func<T, bool>>? whereExpression = null)
        {
            return GetDb().Updateable<T>().SetColumns((T o) => o.Deleted == deleted).Where(whereExpression)
                .ExecuteCommand();
        }

        public virtual int UpdateDeletedById(long id, int deleted = 0)
        {
            throw new InvalidOperationException();
        }

        public virtual int UpdateDeletedById(string id, int deleted = 0)
        {
            throw new InvalidOperationException();
        }

        public virtual List<T> SelectListDeleted(Expression<Func<T, bool>>? whereExpression = null)
        {
            SqlSugarClient db = GetDb();
            if (whereExpression != null)
            {
                return (from o in db.Queryable<T>()
                        where o.Deleted == 1
                        select o).Where(whereExpression).ToList();
            }

            return (from o in db.Queryable<T>()
                    where o.Deleted == 1
                    select o).ToList();
        }
    }
}
