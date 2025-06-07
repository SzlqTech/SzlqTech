

using SqlSugar;

namespace SzlqTech.Entity
{
    /// <summary>
    /// 部门表
    /// </summary>
    [SugarTable("sys_department", TableDescription = "系统部门表")]
    public sealed class SysDepartment : BaseAuditableEntity
    {
        public SysDepartment()
        {
        }

        public SysDepartment(long id)
        {
            Id = id;
        }

        /// <summary>
        /// 名称
        /// </summary>
       
        [SugarColumn(ColumnName = "department_name", IsNullable = true, ColumnDescription = "部门名称")]
        public string? Name { get; set; }

       
        [SugarColumn(ColumnName = "ordinal", ColumnDescription = "序号")]
        public int Ordinal { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        // [Description("部门类型")]
        [SugarColumn(ColumnName = "department_type", IsNullable = true, ColumnDescription = "部门类型")]
        public string? DepartmentType { get; set; }

        /// <summary>
        /// 领导
        /// </summary>
        
        [SugarColumn(ColumnName = "leader", IsNullable = true, ColumnDescription = "领导")]
        public string? Leader { get; set; }
    }
}