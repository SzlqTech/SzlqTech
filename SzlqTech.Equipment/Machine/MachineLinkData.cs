using HslCommunication;
using HslCommunication.Core;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Equipment.Machine
{
    public class MachineLinkData : PLCData
    {
        public MachineLinkData(string portKey, IReadWriteDevice plc, string address, DataType dataType, DecimalPointShiftType decimalPointShiftType, OperateResult operateResult) : base(portKey, plc, address, dataType, decimalPointShiftType, operateResult)
        {
        }

        public string PLCPortKey { get; set; }
    }
}
