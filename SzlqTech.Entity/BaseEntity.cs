
using SqlSugar;
using System.ComponentModel;

namespace SzlqTech.Entity
{
    public class BaseEntity
    {
        [SugarColumn(ColumnName = "code", IsNullable =true, ColumnDescription = "编码", Length = 50)]
        public virtual string Code { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        [SugarColumn(ColumnName = "description", ColumnDescription = "描述", IsNullable = true)]
        public string? Description { get; set; }
    }
}
