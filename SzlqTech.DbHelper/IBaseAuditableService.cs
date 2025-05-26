using System.Linq.Expressions;
using SzlqTech.Entity;

namespace SzlqTech.DbHelper
{
    public interface IBaseAuditableService<T> :IBaseService<T> where T : BaseAuditableEntity
    {
        List<T> ListByStatus(int status);

        bool UpdateStatusById(T entity, int status);

        bool UpdateStatusById(long id, int status);

        bool Revert(Expression<Func<T, bool>>? whereExpression = null);

        bool RevertById(long id);

        List<T> ListDeleted(Expression<Func<T, bool>>? whereExpression = null);
    }
}
