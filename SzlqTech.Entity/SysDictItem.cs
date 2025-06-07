

using SqlSugar;
using SzlqTech.Entity;

namespace SzlqTech.Entity
{
    /// <summary>
    /// 字典项目表
    /// </summary>
    [SugarTable("sys_dict_item", TableDescription = "系统字典项目表")]
    public class SysDictItem : BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "dict_id", ColumnDescription = "字典id")]
        public long DictId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        
        [SugarColumn(ColumnName = "value", IsNullable = true, ColumnDescription = "项目值")]
        public string? Value { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        
        [SugarColumn(ColumnName = "item_name", IsNullable = true, ColumnDescription = "项目名称")]
        public string? Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        
        [SugarColumn(ColumnName = "full_name", IsNullable = true, ColumnDescription = "字典项目全程")]
        public string? FullName { get; set; }

        /// <summary>
        /// 默认
        /// </summary>
      
        [SugarColumn(ColumnName = "default", ColumnDescription = "默认项目")]
        public bool Default { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
       
        [SugarColumn(ColumnName = "ordinal", ColumnDescription = "项目序号")]
        public int Ordinal { get; set; }
    }
}