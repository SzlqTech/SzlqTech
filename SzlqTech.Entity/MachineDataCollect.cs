using SqlSugar;
using System.ComponentModel;

namespace SzlqTech.Entity
{
    [SugarTable("machine_data_collect", TableDescription = "设备数据采集详情列表")]
    public class MachineDataCollect:BaseAuditableEntity
    {
        [SugarColumn(ColumnName = "machine_id", IsNullable = true, ColumnDescription = "关联的PLC表的ID")]
        public long MachineId { get; set; }

        /// <summary>
        /// 端口键
        /// </summary>
        [Description("端口键")]
        [SugarColumn(ColumnName = "portKey",
            IsNullable = true,
            ColumnDescription = "地址键")]
        public string PortKey { get; set; }


       

        [Description("绑定名称")]
        [SugarColumn(ColumnName = "binding_name",
           IsNullable = true,
          ColumnDescription = "绑定名称")]
        public string BindingName { get; set; }


        [SugarColumn(ColumnName = "is_sys_date",
          IsNullable = true,
          ColumnDescription = "是否系统采集时间")]
        public bool IsEnableHeartbeat { get; set; }

        [SugarColumn(ColumnName = "string_format",
         IsNullable = true,
         ColumnDescription = "字段格式")]
        public string StringFormat { get; set; }


        [SugarColumn(ColumnName = "zh_header_title",
       IsNullable = true,
       ColumnDescription = "中文标题")]
        public string ZhHeaderTitle { get; set; }

        [SugarColumn(ColumnName = "en_header_title",
         IsNullable = true,
         ColumnDescription = "英文标题")]
        public string EnHeaderTitle { get; set; }

        [SugarColumn(ColumnName = "tai_header_title",
         IsNullable = true,
         ColumnDescription = "泰文标题")]
        public string TaiHeaderTitle { get; set; }

        [SugarColumn(ColumnName = "is_enable",
        IsNullable = true,
        ColumnDescription = "是否开启")]
        public bool IsEnable { get; set; }

       


    }
}
