

using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IService
{
    public interface ISysUserService: IBaseAuditableService<SysUser>
    {
        List<SysUser> ListByCondition(string? code = null, string? name = null);

        bool UpdatePasswordByUsername(string username, string password);

        List<SysUser> ListEnable();

        List<SysUser> ListExceptRoot();

        SysUser ListByUsername(string username);
    }
}
