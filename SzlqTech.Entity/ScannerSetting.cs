using SqlSugar;
using System.ComponentModel;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Entity
{
    [SugarTable("scanner_setting", TableDescription = "扫描设置")]
    public class ScannerSetting:BaseAuditableEntity
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        [SugarColumn(ColumnName = "scanner_type", ColumnDescription = "设备类型")]
        public int ScannerType { get; set; }

  
        [SugarColumn(ColumnName = "port_key", ColumnDescription = "端口键")]

        public string PortKey {  get; set; }
       

        /// <summary>
        /// 端口名称
        /// </summary>
        [Description("端口名称")]
        [SugarColumn(ColumnName = "port_name", ColumnDescription = "端口名称", Length = 50)]
        public string PortName { get; set; } = null!;

        /// <summary>
        /// 波特率
        /// </summary>
        [Description("波特率")]
        [SugarColumn(ColumnName = "baud_rate", ColumnDescription = "波特率", IsNullable = true)]
        public int? BaudRate { get; set; }

        /// <summary>
        /// 编码层级
        /// </summary>
        [Description("编码层级")]
        [SugarColumn(ColumnName = "code_level", ColumnDescription = "编码层级")]
        public int CodeLevel { get; set; }

        [SugarColumn(ColumnName = "encoding", ColumnDescription = "编码格式")]
        public int Encoding { get; set; }

        [SugarColumn(IsIgnore = true)]
        public ScannerType ScannerModel
        {
            get => (ScannerType)ScannerType;
            set => ScannerType = (int)value;
        }

        // public bool LogEnable { get; set; }

        // public bool Status
        // {
        // set => _enable = value;
        // get => _enable;
        // }

        [SugarColumn(ColumnName = "is_enable", ColumnDescription = "是否启用")]
        public bool IsEnable { get; set; }


        [SugarColumn(ColumnName = "attr0", IsNullable = true)]
        public string? Attr0 { get; set; }

        [SugarColumn(ColumnName = "attr1", IsNullable = true)]
        public string? Attr1 { get; set; }

        [SugarColumn(ColumnName = "attr2", IsNullable = true)]
        public string? Attr2 { get; set; }

        [SugarColumn(ColumnName = "attr3", IsNullable = true)]
        public string? Attr3 { get; set; }
    }
}
