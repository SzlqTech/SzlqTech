using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysRoleMenuServiceImpl : BaseAuditableServiceImpl<ISysRoleMenuRepository, SysRoleMenu>, ISysRoleMenuService
    {
        public SysRoleMenuServiceImpl(ISysRoleMenuRepository baseRepository) : base(baseRepository)
        {
        }

        public bool RemoveByRoleId(long roleId)
        {
            return SqlHelper.RetBool(BaseRepository.DeleteByRoleId(roleId));
        }

        public List<SysRoleMenu> GetByRoleId(long roleId)
        {
            return BaseRepository.SelectListByRoleId(roleId);
        }

        public bool RemoveNotExistRoleId()
        {
            return SqlHelper.RetBool(BaseRepository.DeleteNotExistRoleId());
        }

        public bool UpdateByRoleId(long roleId, List<SysRoleMenu> sysRoleMenus)
        {
            return BaseRepository.UpdateByRoleId(roleId, sysRoleMenus);
        }
    }
}