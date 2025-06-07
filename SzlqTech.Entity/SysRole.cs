
using SqlSugar;
using SzlqTech.Entity;


namespace SzlqTech.Entity
{
    /// <summary>
    /// 角色
    /// </summary>
    [Serializable]
    [SugarTable("sys_role", TableDescription = "系统角色表")]
    public partial class SysRole : BaseAuditableEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
      
        [SugarColumn(ColumnName = "role_name", IsNullable = true, ColumnDescription = "名称")]
        public string? Name { get; set; }
    }
}