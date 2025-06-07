
using SqlSugar;


namespace SzlqTech.Entity
{
    [SugarTable("sys_sequence", TableDescription = "顺序断点表")]
    public class SysSequence : BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "current_value")]
        public long CurrentValue { get; set; }
    }
}