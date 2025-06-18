using SqlSugar;


namespace SzlqTech.Entity
{
    [SugarTable("qr_code", TableDescription = "二维码列表")]
    public class QrCode:BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "product_name", IsNullable = true, ColumnDescription = "产品名称")]
        public string ProductName { get; set; }

        [SugarColumn(ColumnName = "product_code", IsNullable = true, ColumnDescription = "产品代码")]
        public string ProductCode { get; set; }


        [SugarColumn(ColumnName = "station", IsNullable = true, ColumnDescription = "工站")]
        public int Station { get; set; }


        [SugarColumn(ColumnName = "station_name", IsNullable = true, ColumnDescription = "工站名称")]
        public string StationName { get; set; }

        [SugarColumn(ColumnName = "enter_date", IsNullable = true, ColumnDescription = "进入工站时间")]
        public string EnterDate { get; set; }

        [SugarColumn(ColumnName = "leave_date", IsNullable = true, ColumnDescription = "离开工站时间")]
        public string LeaveDate { get; set; }

        [SugarColumn(ColumnName = "parent_code", IsNullable = true, ColumnDescription = "父编码")]
        public string ParentCode { get; set; }

        [SugarColumn(ColumnName = "root_code", IsNullable = true, ColumnDescription = "根编码")]
        public string RootCode { get; set; }

        [SugarColumn(ColumnName = "sn", IsNullable = true, ColumnDescription = "sn")]
        public int SN { get; set; }
    }
}
