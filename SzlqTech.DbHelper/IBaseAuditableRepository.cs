using System.Linq.Expressions;

namespace SzlqTech.DbHelper
{
    public interface IBaseAuditableRepository<T>:IBaseRepository<T> where T : BaseAuditableEntity
    {
        List<T> SelectListByStatus(int status, Expression<Func<T, bool>>? whereExpression = null);

        Task<List<T>> SelectListByStatusAsync(int status, Expression<Func<T, bool>>? whereExpression = null);

        int UpdateStatusById(T entity, int status);

        int UpdateStatusById(long id, int status);

        int UpdateStatusById(string id, int status);

        int UpdateDeleted(int deleted = 0, Expression<Func<T, bool>>? whereExpression = null);

        int UpdateDeletedById(long id, int deleted = 0);

        int UpdateDeletedById(string id, int deleted = 0);

        List<T> SelectListDeleted(Expression<Func<T, bool>>? whereExpression = null);
    }
}
