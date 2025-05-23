
using HslCommunication.Core;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Equipment.Machine
{
    public class PLCDictionary
    {
        public Dictionary<string, PLCData> PLCDict { get; set; } = new Dictionary<string, PLCData>();

        public PLCData Get(string portKey)
        {
            return PLCDict[portKey];
        }

        public IReadWriteDevice GetPLC(string portKey)
        {
            return PLCDict[portKey].PLC;
        }

        public PLCData Add(string portKey, IReadWriteDevice plc, string address, DataType dataType, DecimalPointShiftType decimalPointShiftType)
        {
            PLCData plcItem;
            if (!PLCDict.ContainsKey(portKey))
            {
                plcItem = new PLCData(portKey, plc, address, dataType, decimalPointShiftType);
                PLCDict.Add(portKey, plcItem);
            }
            else
            {
                plcItem = PLCDict[portKey];
            }

            return plcItem;
        }



        public PLCData Add(PLCData plcItem)
        {
            if (!PLCDict.ContainsKey(plcItem.PortKey))
            {
                PLCDict.Add(plcItem.PortKey, plcItem);
            }
            return plcItem;
        }



        public virtual void Remove(PLCData plcItem)
        {
            if (PLCDict.ContainsKey(plcItem.PortKey))
            {
                PLCDict.Remove(plcItem.PortKey);
            }
        }

        public virtual void Remove(string code)
        {
            if (PLCDict.ContainsKey(code))
            {
                PLCDict.Remove(code);
            }
        }

        public virtual void Clear()
        {
            PLCDict.Clear();
        }

        // public virtual bool Wait(string portKey, dynamic data, int readInterval = 100, int waitTimeout = 5000,
        //     bool failThrowEx = false)
        // {
        //     PLCData plcData = Get(portKey);
        //     return plcData.Wait(data, readInterval, waitTimeout, failThrowEx);
        // }

    }
}
