using SqlSugar;
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;

namespace SzlqTech.Repository
{
    public class SysRoleMenuRepository : BaseAuditableRepository<SysRoleMenu>, ISysRoleMenuRepository
    {
        public int DeleteByRoleId(long roleId)
        {
            // string sql = "delete sys_role_menu where role_id=@RoleId";
            // return base.Execute(sql, new { RoleId = roleId });
            return base.Delete(o => o.RoleId == roleId);
        }

        public List<SysRoleMenu> SelectListByRoleId(long roleId)
        {
            // string sql = "select * from sys_role_menu where role_id=@RoleId;";
            // return base.Query(sql, new { RoleId = roleId });
            return base.SelectList(o => o.RoleId == roleId);
        }

        public int DeleteNotExistRoleId()
        {
            var db = GetDb();
            return db.Deleteable<SysRoleMenu>()
                .Where(rm => SqlFunc.Subqueryable<SysRole>().Where(r => r.Id == rm.RoleId).NotAny())
                .ExecuteCommand();
            //return base.Execute("Delete sys_role_menu Where role_id not in (select id from sys_role);");
        }

        /// <summary>
        /// 删除并新增sys_role_menu
        /// </summary>
        public bool UpdateByRoleId(long roleId, List<SysRoleMenu> sysRoleMenus)
        {
            var db = GetDb();
            // int val = 0;
            var tranResult = db.UseTran(() =>
            {
                db.Deleteable<SysRoleMenu>().Where(o => o.RoleId == roleId).ExecuteCommand();
                db.Insertable(sysRoleMenus).ExecuteCommand();
            });
            // SqlAssert.IsTrue(tranResult.IsSuccess, TransactionFail);
            if (tranResult.IsSuccess) return true;
            db.RollbackTran();
            return false;
        }
    }
}