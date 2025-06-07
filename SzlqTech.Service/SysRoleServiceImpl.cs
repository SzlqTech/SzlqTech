using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;
namespace SzlqTech.Service
{
    public class SysRoleServiceImpl : BaseAuditableServiceImpl<ISysRoleRepository, SysRole>, ISysRoleService
    {
        public SysRoleServiceImpl(ISysRoleRepository baseRepository) : base(baseRepository)
        {
        }

        public List<SysRole> ListByCondition(string? code = null, string? name = null)
        {
            return BaseRepository.SelectListByCondition(code, name);
        }

        public List<SysRole> ListOrderByCode()
        {
            return BaseRepository.SelectListOrderByCode();
        }
    }
}