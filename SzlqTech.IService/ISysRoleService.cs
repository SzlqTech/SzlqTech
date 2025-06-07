using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IService
{
    public interface ISysRoleService : IBaseAuditableService<SysRole>
    {
        List<SysRole> ListByCondition(string? code = null, string? name = null);

        List<SysRole> ListOrderByCode();
    }
}