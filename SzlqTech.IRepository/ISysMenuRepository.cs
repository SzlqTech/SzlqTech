
using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IRepository
{
    public interface ISysMenuRepository : IBaseAuditableRepository<SysMenu>
    {
        /// <summary>
        /// 查询所有菜单并根据创建时间排序
        /// </summary>
        /// <returns></returns>
        List<SysMenu> SelectListOrderByCreateTime();

        /// <summary>
        /// 查询所有菜单并根据创建时间排序
        /// </summary>
        /// <returns></returns>
        Task<List<SysMenu>> SelectListOrderByCreateTimeAsync();

        /// <summary>
        /// 根据角色id查询所有关联的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<SysMenu> SelectListByRoleId(long roleId);

        /// <summary>
        /// 根据角色名称查询第一个菜单
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        SysMenu SelectFirstByText(string text);
    }
}