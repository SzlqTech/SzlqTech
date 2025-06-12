using SqlSugar;
using System.ComponentModel;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Entity
{
    [SugarTable("machine_detail", TableDescription = "设备控制器详情表")]
    public class MachineDetail:BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "machine_id", ColumnDescription = "关联的PLC表的ID")]
        public long MachineId { get; set; }

        /// <summary>
        /// 端口键
        /// </summary>
        [Description("端口键")]
        [SugarColumn(ColumnName = "portKey",
            IsNullable = false,
            ColumnDescription = "地址键")]
        public string PortKey { get; set; }
       

        [Description("变量地址")]
        [SugarColumn(ColumnName = "address",
            IsNullable = false,
            ColumnDescription = "变量地址")]
        public string Address { get; set; } = null!;


        /// <summary>
      

        /// <summary>
        /// 数据类型
        /// </summary>
        [Description("数据类型")]
        [SugarColumn(ColumnName = "data_type",
            IsNullable = false,
            ColumnDescription = "数据类型")]
        public int DataType { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Description("数据类型")]
        [SugarColumn(ColumnName = "data_type_name",
            IsNullable = false,
            ColumnDescription = "数据类型名称")]
        public string DataTypeName { get; set; }

        /// <summary>
        /// 数据类型枚举
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public DataType DataTypeEnum
        {
            get => (DataType)DataType;
            set => DataType = (int)value;
        }

        /// <summary>
        /// 是否扫描
        /// </summary>
        [Description("是否扫描")]
        [SugarColumn(ColumnName = "scan_status",
            IsNullable = false,
            ColumnDescription = "是否扫描:1:使能扫描 0:不扫描")]
        public int ScanStatus { get; set; } = 0;

        /// <summary>
        /// 扫描周期
        /// </summary>
        [Description("扫描周期")]
        [SugarColumn(ColumnName = "scan_cycle",
            IsNullable = false,
            ColumnDescription = "扫描周期10/50/100/200/500/1000/5000")]
        public int ScanCycle { get; set; } = 1000;


        [Description("小数点移位")]
        [SugarColumn(ColumnName = "decimal_point_shift",
            IsNullable = false,
            ColumnDescription = "小数点移位")]
        public int DecimalPointShift { get; set; } = 0;

        [SugarColumn(IsIgnore = true)]
        public DecimalPointShiftType DecimalPointShiftType
        {
            get => (DecimalPointShiftType)DecimalPointShift;
            set => DecimalPointShift = (int)value;
        }

        [SugarColumn(IsIgnore = true)]
        public string? MachineCode { get; set; }

        [SugarColumn(ColumnName = "is_enable_scan",
           IsNullable = false,
           ColumnDescription = "是否启用扫描")]
        public bool IsEnableScan { get; set; }

        [SugarColumn(ColumnName = "is_enable_heartbeat",
          IsNullable = false,
          ColumnDescription = "是否启用心跳键")]
        public bool IsEnableHeartbeat { get; set; }

    }
}
