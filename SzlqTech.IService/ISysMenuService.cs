using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IService
{
    public interface ISysMenuService : IBaseAuditableService<SysMenu>
    {
        List<SysMenu> GetLoginMenuList();

        List<SysMenu> ReadFromFile(string fileName);

        bool RemoveSysMenuAndChild(long id);
    }
}