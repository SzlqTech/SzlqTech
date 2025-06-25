using SqlSugar;
namespace SzlqTech.Entity
{
    [SugarTable("data_collect", TableDescription = "采集数据列表")]
    public class DataCollect:BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "machine_id", IsNullable = true, ColumnDescription = "关联的PLC表的ID")]
        public long MachineId { get; set; }

        [SugarColumn(ColumnName = "name", IsNullable = true, ColumnDescription = "名称")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "is_enable",
        IsNullable = true,
        ColumnDescription = "是否开启")]
        public bool IsEnable { get; set; }

        [SugarColumn(ColumnName = "key", ColumnDescription = "存储键")]
        public string Key { get; set; }

        [SugarColumn(ColumnName = "value", ColumnDescription = "存储值")]
        public string Value { get; set; }

        [SugarColumn(ColumnName = "station", IsNullable = true, ColumnDescription = "工站")]
        public int Station { get; set; }


        [SugarColumn(ColumnName = "station_name", IsNullable = true, ColumnDescription = "工站名称")]
        public string StationName { get; set; }

        [SugarColumn(ColumnName = "enter_date", IsNullable = true, ColumnDescription = "进入工站时间")]
        public string EnterDate { get; set; }

        [SugarColumn(ColumnName = "leave_date", IsNullable = true, ColumnDescription = "离开工站时间")]
        public string LeaveDate { get; set; }
    }
}
