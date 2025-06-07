using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IService
{
    public interface ISysRoleMenuService : IBaseAuditableService<SysRoleMenu>
    {
        bool RemoveByRoleId(long roleId);

        List<SysRoleMenu> GetByRoleId(long roleId);

        bool RemoveNotExistRoleId();

        bool UpdateByRoleId(long roleId, List<SysRoleMenu> sysRoleMenus);
    }
}