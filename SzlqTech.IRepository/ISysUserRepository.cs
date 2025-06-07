using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IRepository
{
    public interface ISysUserRepository:IBaseAuditableRepository<SysUser>
    {
        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        List<SysUser> SelectListByCondition(string? code = null, string? username = null);

        /// <summary>
        /// 根据用户名更新密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        int UpdatePasswordByUsername(string username, string password);

        /// <summary>
        /// 查询全部使能的用户
        /// </summary>
        List<SysUser> SelectListEnable();

        /// <summary>
        /// 查询全部使能的用户(不含root)
        /// </summary>
        List<SysUser> SelectListExceptRoot();

        /// <summary>
        /// 根据用户账号查询
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        SysUser SelectFirstByUsername(string username);
    }
}
