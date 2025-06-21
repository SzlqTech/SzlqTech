using SqlSugar;
using System.ComponentModel;

namespace SzlqTech.Entity
{
    [SugarTable("machine_data_collect", TableDescription = "设备数据采集详情列表")]
    public class MachineDataCollect:BaseAuditableEntity
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


        [Description("标题名称")]
        [SugarColumn(ColumnName = "header_title",
            IsNullable = false,
            ColumnDescription = "标题")]
        public string HeaderTitle { get; set; }

        [Description("绑定名称")]
        [SugarColumn(ColumnName = "binding_name",
          IsNullable = false,
          ColumnDescription = "绑定名称")]
        public string BindingName { get; set; }



        [SugarColumn(ColumnName = "is_enable",
           IsNullable = false,
           ColumnDescription = "是否启用")]
        public bool IsEnableScan { get; set; }

        [SugarColumn(ColumnName = "is_sys_date",
          IsNullable = false,
          ColumnDescription = "是否系统采集时间")]
        public bool IsEnableHeartbeat { get; set; }

        [SugarColumn(ColumnName = "string_format",
         IsNullable = true,
         ColumnDescription = "字段格式")]
        public string StringFormat { get; set; }
    }
}
