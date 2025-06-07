using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SqlSugar;

namespace SzlqTech.Repository
{
    public class SysMenuRepository : BaseAuditableRepository<SysMenu>, ISysMenuRepository
    {
        public List<SysMenu> SelectListOrderByCreateTime()
        {
            var db = GetDb();
            return db.Queryable<SysMenu>().OrderBy(o => o.CreateTime).ToList();
            // return base.Query("Select * from sys_menu order by create_time asc");
        }

        public async Task<List<SysMenu>> SelectListOrderByCreateTimeAsync()
        {
            var db = GetDb();
            return await db.Queryable<SysMenu>().OrderBy(o => o.CreateTime).ToListAsync();
            // return await base.QueryAsync("Select * from sys_menu order by create_time desc");
        }

        // /// <summary>
        // /// 删除当前和子菜单
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [Obsolete]
        // public new int DeleteById(long id)
        // {
        //     string sql = "delete from sys_menu where id=@Id or parent_id=@Id";
        //     return base.Execute(sql, new { Id = id });
        // }

        public List<SysMenu> SelectListByRoleId(long roleId)
        {
            var db = GetDb();
            return db.Queryable<SysMenu>()
                .Where(m => m.Id == SqlFunc.Subqueryable<SysRoleMenu>()
                    .Where(rm => rm.RoleId == roleId)
                    .GroupBy(rm => rm.MenuId)
                    .Select(rm => rm.MenuId))
                .OrderBy(m => new { m.ParentId, m.Ordinal })
                .ToList();
            // var db = GetDb();
            // return db.Queryable<SysMenu>()
            //     .Where(m => SqlFunc.Subqueryable<SysRoleMenu>().Where(rm => rm.RoleId == m.Id).Any())
            //     .OrderBy(m => new { m.ParentId, m.Ordinal })
            //     .ToList();
            // string sql = "select * from sys_menu where id in " +
            //              "(select menu_id from sys_role_menu where role_id=@RoleId) " +
            //              "Order by parent_id, ordinal";
            // return base.Query(sql, new { RoleId = roleId });
            // var db = GetDb();
        }

        public SysMenu SelectFirstByText(string text)
        {
            return base.SelectFirst(o => o.Text == text);
            // string sql = "select * from sys_menu Where text=@Text ";
            // return base.QueryFirstOrDefault(sql, new { Text = text });
        }
    }
}