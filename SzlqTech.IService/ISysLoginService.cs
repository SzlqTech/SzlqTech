using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IService
{
    public interface ISysLoginService : IBaseAuditableService<SysUser>
    {
        void UserLogin(string username, string password, bool remember);
    }
}