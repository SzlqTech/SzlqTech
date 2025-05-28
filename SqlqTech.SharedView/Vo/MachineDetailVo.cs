
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using SzlqTech.Common.EnumType;
using SzlqTech.Entity;

namespace SqlqTech.SharedView.Vo
{
    public partial class MachineDetailVo:BaseVo
    {
        /// <summary>
        /// 关联的PLC表的ID
        /// </summary>
        [ObservableProperty]
        public long machineId;

        /// <summary>
        /// 端口键
        /// </summary
        [ObservableProperty]
        public string portKey;


        /// <summary>
        /// 变量地址
        /// </summary>
        [ObservableProperty]
        public string address;


        /// <summary>
        /// 描述
        /// </summary>
        [ObservableProperty]
        public string? description;

        /// <summary>
        /// 数据类型
        /// </summary>
        [ObservableProperty]
        public int dataType;

        [ObservableProperty]
        public string dataTypeName;

        [ObservableProperty]
        public bool isEnableScan;

        /// <summary>
        /// 数据类型枚举
        /// </summary>
        public DataType DataTypeEnum
        {
            get => (DataType)DataType;
            set => DataType = (int)value;
        }

        /// <summary>
        /// 是否扫描 1:使能扫描 0:不扫描
        /// </summary>
        [ObservableProperty]
        public int scanStatus  = 0;

        /// <summary>
        /// 扫描周期 扫描周期10/50/100/200/500/1000/5000
        /// </summary>
        [ObservableProperty]
        public int scanCycle =0;

        [ObservableProperty]
        public int scanCycleValue ;


        [Description("小数点移位")]
        [ObservableProperty]
        public int decimalPointShift  = 0;

        
        public DecimalPointShiftType DecimalPointShiftType
        {
            get => (DecimalPointShiftType)DecimalPointShift;
            set => DecimalPointShift = (int)value;
        }

        [ObservableProperty]
        public string? machineCode;
    }
}
