using HslCommunication.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Equipment.Machine
{
    public class PLCScanDictionary
    {


        public Dictionary<string, PLCScanData> PLCScanDict { get; set; } = new Dictionary<string, PLCScanData>();

        public PLCScanData Get(string portKey)
        {
            return PLCScanDict[portKey];
        }

        public IReadWriteDevice GetPLC(string portKey)
        {
            return PLCScanDict[portKey].PLC;
        }

        public PLCScanData Add(string portKey, IReadWriteDevice plc, string address, DataType dataType, int scanCycle = 1000)
        {
            PLCScanData scanData;
            if (!PLCScanDict.ContainsKey(portKey))
            {
                scanData = new PLCScanData
                {
                    PortKey = portKey,
                    PLC = plc,
                    Address = address,
                    DataType = dataType,
                    ScanCycle = scanCycle
                };
                PLCScanDict.Add(portKey, scanData);

            }
            else
            {
                scanData = PLCScanDict[portKey];
            }

            return scanData;
        }



        public PLCScanData Add(PLCScanData scanData)
        {
            if (!PLCScanDict.ContainsKey(scanData.PortKey))
            {
                PLCScanDict.Add(scanData.PortKey, scanData);
            }
            return scanData;
        }

        public virtual void AddToStartScan(string portKey, IReadWriteDevice plc, string address, DataType dataType, int scanCycle = 1000)
        {
            if (!PLCScanDict.ContainsKey(portKey))
            {
                PLCScanData scanData = Add(portKey, plc, address, dataType, scanCycle);
                Scan(scanData);
            }
        }

        public virtual void AddToStartScan(PLCScanData scanData)
        {
            if (!PLCScanDict.ContainsKey(scanData.PortKey))
            {
                Add(scanData);
                Scan(scanData);
            }
        }

        public virtual void RemoveToStopScan(PLCScanData scanData)
        {
            if (PLCScanDict.ContainsKey(scanData.PortKey))
            {
                scanData.Enable = false;
                scanData.Data = default;
                PLCScanDict.Remove(scanData.PortKey);
            }
        }

        public virtual void RemoveToStopScan(string code)
        {
            if (PLCScanDict.ContainsKey(code))
            {
                PLCScanData scanData = PLCScanDict[code];
                scanData.Enable = false;
                scanData.Data = default;
                PLCScanDict.Remove(scanData.PortKey);
            }
        }

        public virtual void ClearToStopScan()
        {
            foreach (var pair in PLCScanDict)
            {
                // string code = pair.Key;
                PLCScanData scanData = pair.Value;
                scanData.Enable = false;
                scanData.Data = default;
            }
            PLCScanDict.Clear();
        }



        protected virtual void Scan(PLCScanData scanData)
        {
            ThreadStart threadStart = () =>
            {
                while (scanData.Enable)
                {
                    switch (scanData.DataType)
                    {
                        case DataType.Bool:
                            var boolResult = scanData.PLC.ReadBool(scanData.Address).EnsureScanSuccess();
                            scanData.Data = boolResult.Content;
                            break;
                        case DataType.Byte:
                            var byteResult = scanData.PLC.Read(scanData.Address, 1).EnsureScanSuccess();
                            scanData.Data = byteResult.Content[0];
                            break;
                        case DataType.Int16:
                            var int16Result = scanData.PLC.ReadInt16(scanData.Address).EnsureScanSuccess();
                            scanData.Data = int16Result.Content;
                            break;
                        case DataType.UInt16:
                            var uint16Result = scanData.PLC.ReadUInt16(scanData.Address).EnsureScanSuccess();
                            scanData.Data = uint16Result.Content;
                            break;
                        case DataType.Int32:
                            var int32Result = scanData.PLC.ReadInt32(scanData.Address).EnsureScanSuccess();
                            scanData.Data = int32Result.Content;
                            break;
                        case DataType.UInt32:
                            var uint32Result = scanData.PLC.ReadUInt32(scanData.Address).EnsureScanSuccess();
                            scanData.Data = uint32Result.Content;
                            break;
                        case DataType.Int64:
                            var int64Result = scanData.PLC.ReadInt64(scanData.Address).EnsureScanSuccess();
                            scanData.Data = int64Result.Content;
                            break;
                        case DataType.UInt64:
                            var uint64Result = scanData.PLC.ReadUInt64(scanData.Address).EnsureScanSuccess();
                            scanData.Data = uint64Result.Content;
                            break;
                        case DataType.Float:
                            var floatResult = scanData.PLC.ReadFloat(scanData.Address).EnsureScanSuccess();
                            scanData.Data = floatResult.Content;
                            break;
                        case DataType.Double:
                            var doubleResult = scanData.PLC.ReadDouble(scanData.Address).EnsureScanSuccess();
                            scanData.Data = doubleResult.Content;
                            break;
                        default:
                            throw new ArgumentException(nameof(scanData.DataType));
                    }
                    Thread.Sleep(scanData.ScanCycle);
                }
            };
            Thread thread = new Thread(threadStart);
            thread.Start();
        }


        public virtual void StartScan()
        {
            foreach (var scanTagPair in PLCScanDict)
            {
                var scanTag = scanTagPair.Value;
                Scan(scanTag);
            }
        }
    }
}
