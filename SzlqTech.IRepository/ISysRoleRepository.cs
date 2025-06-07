
using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IRepository
{
    public interface ISysRoleRepository : IBaseAuditableRepository<SysRole>
    {
        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<SysRole> SelectListByCondition(string? code = null, string? name = null);

        /// <summary>
        /// 获取所有并根据编码排序(排除特殊角色)
        /// </summary>
        /// <returns></returns>
        List<SysRole> SelectListOrderByCode();
    }
}