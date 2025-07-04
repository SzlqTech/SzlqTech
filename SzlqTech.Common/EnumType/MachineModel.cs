
using System.ComponentModel;

namespace SzlqTech.Common.EnumType
{
    public enum MachineModel : short
    {
        // [Description("未知PLC")]
        // Unknown = 0,

        #region 驱动

        [Description("西门子S7-200 Smart系列驱动")]
        SiemensS200Smart = 1,

        [Description("西门子S7-1200系列驱动")]
        SiemensS1200 = 2,

        [Description("西门子S7-1500系列驱动")]
        SiemensS1500 = 3,

        [Description("Modbus TCP协议驱动")]
        ModbusTcp = 10,


        //[Description("Modbus RTU协议驱动")]
        //ModbusRtu = 11,

        [Description("汇川H3U系列以太网驱动")]
        InovanceH3UNet = 20,

        [Description("汇川H5U系列以太网驱动")]
        InovanceH5UNet = 21,

        [Description("汇川Easy系列以太网驱动")]
        InovanceEasyNet = 22,

        [Description("汇川AM系列以太网驱动")]
        InovanceAMNet = 23,

        [Description("SEC/GEM标准协议")]
        SEC_GEM = 24,

        //[Description("倍福Twincat2系列以太网驱动")]
        //BeckoffAds2 =24,

        //[Description("倍福Twincat3系列以太网驱动")]
        //BeckoffAds3 = 25

        //[Description("汇川H3U系列串口驱动")]
        //InovanceH3USerial = 30,

        //[Description("汇川H5U系列串口驱动")]
        //InovanceH5USerial = 31,

        //[Description("汇川Easy系列串口驱动")]
        //InovanceEasySerial = 32,

        //[Description("汇川AM系列串口驱动")]
        //InovanceAMSerial = 33,

        //[Description("台达以太网驱动")]
        //DeltaTcpNet = 40,

        //[Description("台达DVP系列以太网驱动")]
        //DeltaDvpNet = 41,

        //[Description("台达AS系列以太网驱动")]
        //DeltaASNet = 42,

        //[Description("台达DVP系列串口驱动")]
        //DeltaDvpSerial = 43,

        //[Description("台达AS系列串口驱动")]
        //DeltaASSerial = 44,

        #endregion

    }
}
