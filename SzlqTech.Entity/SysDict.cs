

using SqlSugar;
using SzlqTech.Entity;

namespace SzlqTech.Entity
{
    /// <summary>
    /// 字典表
    /// </summary>
    [SugarTable("sys_dict", TableDescription = "系统字典表")]
    public class SysDict : BaseAuditableEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
       
        [SugarColumn(ColumnName = "dict_name", IsNullable = true, ColumnDescription = "字典名称")]
        public string? Name { get; set; }
    }
}