

using HslCommunication;
using HslCommunication.Core;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Equipment.Machine
{
    public class PLCData
    {
        // public PLCData() { }

        public PLCData(string portKey, IReadWriteDevice plc, string address, DataType dataType, DecimalPointShiftType decimalPointShiftType)
        {
            PortKey = portKey;
            PLC = plc;
            Address = address;
            DataType = dataType;
            DecimalPointShiftType = decimalPointShiftType;
        }

        public PLCData(string portKey, IReadWriteDevice plc, string address, DataType dataType, DecimalPointShiftType decimalPointShiftType, OperateResult operateResult)
        {
            PortKey = portKey;
            PLC = plc;
            Address = address;
            DataType = dataType;
            DecimalPointShiftType = decimalPointShiftType;
            OperateResult = operateResult;
        }

        // public PLCScanItem(string portKey, string address, Type dataType, dynamic data)
        // {
        //     PortKey = portKey;
        //     Address = address;
        //     this.data = data;
        //     DataType = dataType;
        // }


        // [Name("标签")] 
        // [Index(0)] 
        public string PortKey { get; set; }

        public IReadWriteDevice PLC { get; set; }

        // [Name("地址")]
        // [Index(1)]
        public string Address { get; set; }

        // public string Length { get; set; }

        // [Name("类型")]
        // [Index(2)]
        public DataType DataType { get; set; }

        public DecimalPointShiftType DecimalPointShiftType { get; set; }

        public OperateResult OperateResult { get; set; } 
    }
}
