
using SqlSugar;
using System.ComponentModel;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Entity
{
    
    [SugarTable("machine_setting", TableDescription = "机器设置")]
    public class MachineSetting:BaseAuditableEntity
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        [Description("设备型号")]
        [SugarColumn(ColumnName = "machine_model", ColumnDescription = "设备型号")]
        public int MachineModel { get; set; }

        [SugarColumn(IsIgnore = true)]
        public MachineModel MachineModelEnum
        {
            get => (MachineModel)MachineModel;
            set => MachineModel = (int)value;
        }

       
        [SugarColumn(IsIgnore = true)]
        public string PortKey
        {
            get => Code;
            set => Code = value;
        }

       
        [Description("端口名称")]
        [SugarColumn(ColumnName = "port_name", ColumnDescription = "端口名称", Length = 50)]
        public string PortName { get; set; } = null!;

      
    }
}
