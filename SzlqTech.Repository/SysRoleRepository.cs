using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;

namespace SzlqTech.Repository
{
    public class SysRoleRepository : BaseAuditableRepository<SysRole>, ISysRoleRepository
    {
        public List<SysRole> SelectListByCondition(string? code = null, string? name = null)
        {
            var db = GetDb();
            return db.Queryable<SysRole>()
                .Where(s => s.Id != 0)
                .WhereIF(!string.IsNullOrEmpty(code), r => r.Code.Contains(code!))
                .WhereIF(!string.IsNullOrEmpty(name), r => r.Name!.Contains(name!))
                .OrderBy(s => s.Code)
                .ToList();
            // StringBuilder sb = new StringBuilder();
            // sb.Append("select * from sys_role where 1=1 ");
            // if (!string.IsNullOrEmpty(code))
            // {
            //     sb.Append(" and code like '%");
            //     sb.Append(code);
            //     sb.Append("%' ");
            // }
            // if (!string.IsNullOrEmpty(name))
            // {
            //     sb.Append(" and name like '%");
            //     sb.Append(name.Replace("%", "[%]"));
            //     sb.Append("%' ");
            // }
            // sb.Append(" order by code");
            // return base.Query(sb.ToString());
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public List<SysRole> SelectListOrderByCode()
        {
            var db = GetDb();
            return db.Queryable<SysRole>()
                .Where(s => s.Id != 0)
                .OrderBy(s => s.Code)
                .ToList();
            // string sql = "select * from sys_role where id<>0 order by code";
            // return base.Query(sql);
        }
    }
}