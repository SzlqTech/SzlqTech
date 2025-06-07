using SzlqTech.Common.Context;
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;

namespace SzlqTech.Repository
{
    public class SysUserRepository:BaseAuditableRepository<SysUser>,ISysUserRepository
    {
        public List<SysUser> SelectListByCondition(string? code = null, string? username = null)
        {
            var db = GetDb();
            return db.Queryable<SysUser>()
                .Where(s => s.Id != 0 && s.RoleId != 0)
                .WhereIF(!string.IsNullOrEmpty(code), r => r.Code.Contains(code!))
                .WhereIF(!string.IsNullOrEmpty(username), r => r.Username.Contains(username!))
                .OrderBy(s => s.Code)
                .ToList();
            // StringBuilder sb = new StringBuilder();
            // sb.Append("select * from sys_user Where id !=0 ");
            // if (!string.IsNullOrEmpty(code))z
            // {
            //     sb.Append(" and code like '%");
            //     sb.Append(code);
            //     sb.Append("%' ");
            // }
            // if (!string.IsNullOrEmpty(name))
            // {
            //     sb.Append(" and username like '%");
            //     sb.Append(name.Replace("%", "[%]"));
            //     sb.Append("%' ");
            // }
            // sb.Append(" order by code");
            // return base.Query(sb.ToString());
        }

        public int UpdatePasswordByUsername(string username, string password)
        {
            return base.UpdateColumns(u =>
                u.Password == password
                && u.UpdateUser == UserContext.UserId
                && u.UpdateTime == DateTime.Now
                && u.LastUpdatePasswordTime == DateTime.Now
                , u => u.Username == username);
            // string sql = "update sys_user set password=@Password where username=@Username;";
            // return base.Execute(sql, new { Username = username, Password = password });
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        public List<SysUser> SelectListEnable()
        {
            // return base.Query("select * from sys_user Where status=1 order by code ");
            return base.SelectListOrderBy(o => o.Code, o => o.Status == 1 && o.Id != 0 && o.RoleId != 0);
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        public List<SysUser> SelectListExceptRoot()
        {
            // return base.Query("select * from sys_user where id<>0 and role_id<>0 order by code ");
            return base.SelectList(o => o.Id != 0 && o.RoleId != 0);
        }

        /// <summary>
        /// 根据用户账号查询
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public SysUser SelectFirstByUsername(string username)
        {
            // return base.QueryFirstOrDefault("select * from sys_user Where username=@Username;",  new { Username = username });
            return base.SelectFirst(o => o.Username == username);
        }
    }
}
