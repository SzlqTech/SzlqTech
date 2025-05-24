using System.Linq.Expressions;
using SzlqTech.Common.Context;

namespace SzlqTech.DbHelper
{
    public class BaseAuditableServiceImpl<TM, T> : BaseServiceImpl<TM, T>, IBaseAuditableService<T>, IBaseService<T> where TM : IBaseAuditableRepository<T> where T : BaseAuditableEntity, new()
    {
        public BaseAuditableServiceImpl(TM baseRepository)
            : base(baseRepository)
        {
        }

        protected virtual void ResolveEntity(T entity)
        {
            long? userId = UserContext.UserId;
            DateTime now = DateTime.Now;
            if (!entity.CreateUser.HasValue)
            {
                entity.CreateUser = userId;
                entity.CreateTime = now;
            }

            entity.UpdateUser = userId;
            entity.UpdateTime = now;
        }

        protected virtual void ResolveSaveEntity(T entity)
        {
            long? userId = UserContext.UserId;
            DateTime now = DateTime.Now;
            entity.CreateUser = userId;
            entity.CreateTime = now;
        }

        protected virtual void ResolveUpdateEntity(T entity)
        {
            long? userId = UserContext.UserId;
            DateTime now = DateTime.Now;
            entity.UpdateUser = userId;
            entity.UpdateTime = now;
        }

        protected virtual void ResolveSaveOrUpdateEntity(T entity)
        {
            long? userId = UserContext.UserId;
            DateTime now = DateTime.Now;
            if (!entity.CreateUser.HasValue)
            {
                entity.CreateUser = userId;
                entity.CreateTime = now;
            }
            else
            {
                entity.UpdateUser = userId;
                entity.UpdateTime = now;
            }
        }

        public override bool Save(T entity)
        {
            ResolveSaveEntity(entity);
            return base.Save(entity);
        }

        public override async Task<bool> SaveAsync(T entity)
        {
            ResolveSaveEntity(entity);
            return await base.SaveAsync(entity);
        }

        public override bool Save(List<T> entities)
        {
            entities.ForEach(ResolveSaveEntity);
            return base.Save(entities);
        }

        public override async Task<bool> SaveAsync(List<T> entities)
        {
            entities.ForEach(ResolveSaveEntity);
            return await base.SaveAsync(entities);
        }

        public override bool SaveBatch(List<T> entities, int batchSize = 100)
        {
            entities.ForEach(ResolveSaveEntity);
            return base.SaveBatch(entities, batchSize);
        }

        public override async Task<bool> SaveBatchAsync(List<T> entities, int batchSize = 100)
        {
            entities.ForEach(ResolveSaveEntity);
            return await base.SaveBatchAsync(entities, batchSize);
        }

        public override bool SaveOrUpdate(T entity)
        {
            ResolveSaveOrUpdateEntity(entity);
            return base.SaveOrUpdate(entity);
        }

        public override async Task<bool> SaveOrUpdateAsync(T entity)
        {
            ResolveSaveOrUpdateEntity(entity);
            return await base.SaveOrUpdateAsync(entity);
        }

        public override bool SaveOrUpdate(List<T> entities)
        {
            entities.ForEach(ResolveSaveOrUpdateEntity);
            return base.SaveOrUpdate(entities);
        }

        public override async Task<bool> SaveOrUpdateAsync(List<T> entities)
        {
            entities.ForEach(ResolveSaveOrUpdateEntity);
            return await base.SaveOrUpdateAsync(entities);
        }

        public override bool SaveOrUpdateBatch(List<T> entities, int batchSize = 100)
        {
            entities.ForEach(ResolveSaveOrUpdateEntity);
            return base.SaveOrUpdateBatch(entities, batchSize);
        }

        public override async Task<bool> SaveOrUpdateBatchAsync(List<T> entities, int batchSize = 100)
        {
            entities.ForEach(ResolveSaveOrUpdateEntity);
            return await base.SaveOrUpdateBatchAsync(entities, batchSize);
        }

        public override bool SaveBatchNotExist(List<T> entities, int batchSize = 100)
        {
            entities.ForEach(ResolveSaveOrUpdateEntity);
            return base.SaveBatchNotExist(entities, batchSize);
        }

        public override bool UpdateById(T entity)
        {
            ResolveUpdateEntity(entity);
            return base.UpdateById(entity);
        }

        public override async Task<bool> UpdateByIdAsync(T entity)
        {
            ResolveUpdateEntity(entity);
            return await base.UpdateByIdAsync(entity);
        }

        public override bool Update(T entity, Expression<Func<T, bool>> whereExpression)
        {
            ResolveUpdateEntity(entity);
            return base.Update(entity, whereExpression);
        }

        public override async Task<bool> UpdateAsync(T entity, Expression<Func<T, bool>> whereExpression)
        {
            ResolveUpdateEntity(entity);
            return await base.UpdateAsync(entity, whereExpression);
        }

        public override bool UpdateBatchById(List<T> entities, int batchSize = 100)
        {
            entities.ForEach(ResolveUpdateEntity);
            return base.UpdateBatchById(entities, batchSize);
        }

        public override async Task<bool> UpdateBatchByIdAsync(List<T> entities, int batchSize = 100)
        {
            entities.ForEach(ResolveUpdateEntity);
            return await base.UpdateBatchByIdAsync(entities, batchSize);
        }

        public override bool BulkSave(List<T> entities)
        {
            entities.ForEach(ResolveSaveEntity);
            return base.BulkSave(entities);
        }

        public override async Task<bool> BulkSaveAsync(List<T> entities)
        {
            entities.ForEach(ResolveSaveEntity);
            return await base.BulkSaveAsync(entities);
        }

        public override bool BulkUpdate(List<T> entities)
        {
            entities.ForEach(ResolveUpdateEntity);
            return base.BulkUpdate(entities);
        }

        public override async Task<bool> BulkUpdateAsync(List<T> entities)
        {
            entities.ForEach(ResolveUpdateEntity);
            return await base.BulkUpdateAsync(entities);
        }

        public override bool BulkSaveOrUpdate(List<T> entities)
        {
            entities.ForEach(ResolveSaveOrUpdateEntity);
            return base.BulkSaveOrUpdate(entities);
        }

        public override async Task<bool> BulkSaveOrUpdateAsync(List<T> entities)
        {
            entities.ForEach(ResolveSaveOrUpdateEntity);
            return await base.BulkSaveOrUpdateAsync(entities);
        }

        public List<T> ListByStatus(int status)
        {
            return BaseRepository.SelectListByStatus(status);
        }

        public bool UpdateStatusById(T entity, int status)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateStatusById(entity, status));
        }

        public bool UpdateStatusById(long id, int status)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateStatusById(id, status));
        }

        public bool Revert(Expression<Func<T, bool>>? whereExpression = null)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateDeleted(0, whereExpression));
        }

        public bool RevertById(long id)
        {
            return SqlHelper.RetBool(BaseRepository.UpdateDeletedById(id));
        }

        public List<T> ListDeleted(Expression<Func<T, bool>>? whereExpression = null)
        {
            return BaseRepository.SelectListDeleted(whereExpression);
        }
    }
}
