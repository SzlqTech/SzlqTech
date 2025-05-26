using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysUserService
        : BaseAuditableServiceImpl<ISysUserRepository, SysUser>, ISysUserService
    {
        public SysUserService(ISysUserRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
