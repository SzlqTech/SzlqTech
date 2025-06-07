
using SqlSugar;
using SzlqTech.Entity;


namespace SzlqTech.Entity
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Serializable]
    [SugarTable("sys_role_menu", TableDescription = "系统角色菜单表")]
    public sealed class SysRoleMenu : BaseAuditableEntity
    {
        public SysRoleMenu()
        { }

        public SysRoleMenu(long id, long roleId, long menuId) : this()
        {
            Id = id;
            RoleId = roleId;
            MenuId = menuId;
        }

        /// <summary>
        /// RoleId
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色id")]
        public long RoleId { get; set; }

        /// <summary>
        /// MenuId
        /// </summary>
        [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单id")]
        public long MenuId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public override string Code { get; set; } = string.Empty;
    }
}