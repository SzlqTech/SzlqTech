
using SqlSugar;


namespace SzlqTech.Entity
{
    [SugarTable("sys_user_detail", TableDescription = "用户明细表")]
    public class SysUserDetail: BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "name", IsNullable = true, ColumnDescription = "名称")]
        public string? Name { get; set; }

        [SugarColumn(ColumnName = "role_id", IsNullable = true, ColumnDescription = "角色Id")]
        public long? RoleId { get; set; }

        [SugarColumn(ColumnName = "role_name", IsNullable = true, ColumnDescription = "角色名称")]
        public string? RoleName { get; set; }
    }
}
