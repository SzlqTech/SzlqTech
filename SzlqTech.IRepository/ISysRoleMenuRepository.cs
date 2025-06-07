
using SzlqTech.DbHelper;
using SzlqTech.Entity;


namespace SzlqTech.IRepository
{
    public interface ISysRoleMenuRepository : IBaseAuditableRepository<SysRoleMenu>
    {
        /// <summary>
        /// 根据角色id删除分配的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int DeleteByRoleId(long roleId);

        /// <summary>
        /// 根据角色id查询列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<SysRoleMenu> SelectListByRoleId(long roleId);

        /// <summary>
        /// 删除角色不存在的实体
        /// </summary>
        /// <returns></returns>
        int DeleteNotExistRoleId();

        /// <summary>
        /// 删除并新增sys_role_menu
        /// </summary>
        bool UpdateByRoleId(long roleId, List<SysRoleMenu> sysRoleMenus);
    }
}