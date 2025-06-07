using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysUserDetailServiceImpl : BaseAuditableServiceImpl<ISysUserDetailRepository, SysUserDetail>, ISysUserDetailService
    {
        public SysUserDetailServiceImpl(ISysUserDetailRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
