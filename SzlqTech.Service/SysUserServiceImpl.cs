using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysUserServiceImpl
        : BaseAuditableServiceImpl<ISysUserRepository, SysUser>, ISysUserService
    {
        public SysUserServiceImpl(ISysUserRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
