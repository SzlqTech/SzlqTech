
using SqlSugar;

namespace SzlqTech.DbHelper
{
    public class BaseEntity
    {
        [SugarColumn(ColumnName = "code", ColumnDescription = "编码", Length = 50)]
        public virtual string Code { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool IsSelected { get; set; }
    }
}
