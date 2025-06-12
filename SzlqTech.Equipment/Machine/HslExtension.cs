
using HslCommunication;
using NLog;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;

namespace SzlqTech.Equipment.Machine
{
    public static class HslExtension
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static OperateResult LogErrorHandler(this OperateResult operateResult)
        {
            if (!operateResult.IsSuccess)
            {
                //Logger.ErrorHandler($"PLC操作失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult;
        }

        public static OperateResult LogError(this OperateResult operateResult)
        {
            if (!operateResult.IsSuccess)
            {
                Logger.Error($"PLC操作失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult;
        }

        public static OperateResult<T> LogError<T>(this OperateResult<T> operateResult)
        {
            if (!operateResult.IsSuccess)
            {
                //Logger.ErrorHandler($"PLC操作失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult;
        }

        public static bool LogErrorHandlerAndIsSuccess(this OperateResult operateResult)
        {
            if (!operateResult.IsSuccess)
            {
                //Logger.ErrorHandler($"PLC操作失败, 错误编码: [{operateResult.ErrorCode}], 信息: {operateResult.Message}");
            }
            return operateResult.IsSuccess;
        }

        public static T LogErrorHandlerAndContent<T>(this OperateResult<T> operateResult)
        {
            if (!operateResult.IsSuccess)
            {
                //Logger.ErrorHandler($"PLC操作失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult.Content;
        }

        public static bool LogErrorAndIsSuccess(this OperateResult operateResult)
        {
            if (!operateResult.IsSuccess)
            {
                Logger.Error($"PLC操作失败, 错误编码: [{operateResult.ErrorCode}], 信息: {operateResult.Message}");
            }
            return operateResult.IsSuccess;
        }

        public static T LogErrorAndContent<T>(this OperateResult<T> operateResult)
        {
            if (!operateResult.IsSuccess)
            {
                Logger.Error($"PLC操作失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult.Content;
        }

        public static OperateResult EnsureSuccess(this OperateResult operateResult, string operate = "")
        {
            if (!operateResult.IsSuccess)
            {
                //throw new EquipmentException($"机器PLC控制器{operate}操作失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult;
        }

        public static OperateResult<T> EnsureSuccess<T>(this OperateResult<T> operateResult, string operate = "")
        {
            if (!operateResult.IsSuccess)
            {
                //throw new EquipmentException($"机器PLC控制器{operate}操作失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult;
        }

        private static int _failCount = 0;

        public static OperateResult<T> EnsureScanSuccess<T>(this OperateResult<T> operateResult)
        {
            if (operateResult.IsSuccess)
            {
                _failCount = 0;
            }
            else
            {
                _failCount++;
                Logger.Error($"扫描发生一次错误, {operateResult.ToMessageShowString()}");
            }

            if (_failCount > 2)
            {
                //throw new EquipmentException($"PLC超过连续2次失败, {operateResult.ToMessageShowString()}");
            }
            return operateResult;
        }

        public static bool Write(this PLCData item, dynamic data, bool failThrowEx = false)
        {
            OperateResult operateResult;
            string dataTypeMessage = "当前写入的数据类型与配置的不一致";
            switch (item.DataType)
            {
                case DataType.Bool:
                    if (data is not bool boolValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Write(item.Address, boolValue);
                    break;

                case DataType.Byte:
                    if (data is not byte byteValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Write(item.Address, byteValue);
                    break;

                case DataType.Int16:
                    operateResult = data switch
                    {
                        short val => item.PLC.Write(item.Address, val),
                        decimal val => item.PLC.Write(item.Address,
                            Convert.ToInt16((int)(val * (10 ^ (int)item.DecimalPointShiftType)))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.UInt16:
                    operateResult = data switch
                    {
                        ushort val => item.PLC.Write(item.Address, val),
                        decimal val => item.PLC.Write(item.Address,
                            Convert.ToUInt16(val * (10 ^ (int)item.DecimalPointShiftType))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.Int32:
                    operateResult = data switch
                    {
                        int val => item.PLC.Write(item.Address, val),
                        decimal val => item.PLC.Write(item.Address,
                            Convert.ToInt32(val * (10 ^ (int)item.DecimalPointShiftType))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.UInt32:
                    operateResult = data switch
                    {
                        uint val => item.PLC.Write(item.Address, val),
                        decimal val => item.PLC.Write(item.Address,
                            Convert.ToUInt32(val * (10 ^ (int)item.DecimalPointShiftType))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.Int64:
                    if (data is not long longValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Write(item.Address, longValue);
                    break;

                case DataType.UInt64:
                    if (data is not ulong ulongValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Write(item.Address, ulongValue);
                    break;

                case DataType.Float:
                    if (data is not float floatValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Write(item.Address, floatValue);
                    break;

                case DataType.Double:
                    if (data is not double doubleValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Write(item.Address, doubleValue);
                    break;

                default:
                    throw new ArgumentException("数据类型配置错误");
            }

            if (!operateResult.IsSuccess)
            {
                string writeFailMessage = $"PLC写入失败, 地址为[{item.Address}]";
                if (failThrowEx)
                {
                   // throw new EquipmentException(writeFailMessage);
                }
                else
                {
                    Logger.Error(writeFailMessage);
                }
            }
            else
            {
                Logger.Info($"地址为[{item.Address}]写入值$[{data}]");
            }
            return operateResult.IsSuccess;
        }

        public static async Task<bool> WriteAsync(this PLCData item, dynamic data, bool failThrowEx = true)
        {
            OperateResult operateResult;
            string dataTypeMessage = "当前数据的类型与配置的不一致";
            switch (item.DataType)
            {
                case DataType.Bool:
                    if (data is not bool boolValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WriteAsync(item.Address, boolValue);
                    break;

                case DataType.Byte:
                    if (data is not byte byteValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WriteAsync(item.Address, byteValue);
                    break;

                case DataType.Int16:
                    operateResult = data switch
                    {
                        short val => await item.PLC.WriteAsync(item.Address, val),
                        decimal val => await item.PLC.WriteAsync(item.Address,
                            Convert.ToInt16(val * (10 ^ (int)item.DecimalPointShiftType))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.UInt16:
                    operateResult = data switch
                    {
                        ushort val => await item.PLC.WriteAsync(item.Address, val),
                        decimal val => await item.PLC.WriteAsync(item.Address,
                            Convert.ToUInt16(val * (10 ^ (int)item.DecimalPointShiftType))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.Int32:
                    operateResult = data switch
                    {
                        int val => await item.PLC.WriteAsync(item.Address, val),
                        decimal val => await item.PLC.WriteAsync(item.Address,
                            Convert.ToInt32(val * (10 ^ (int)item.DecimalPointShiftType))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.UInt32:
                    operateResult = data switch
                    {
                        uint val => await item.PLC.WriteAsync(item.Address, val),
                        decimal val => await item.PLC.WriteAsync(item.Address,
                            Convert.ToUInt32(val * (10 ^ (int)item.DecimalPointShiftType))),
                        _ => throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]")
                    };
                    break;

                case DataType.Int64:
                    if (data is not long longValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WriteAsync(item.Address, longValue);
                    break;

                case DataType.UInt64:
                    if (data is not ulong ulongValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WriteAsync(item.Address, ulongValue);
                    break;

                case DataType.Float:
                    if (data is not float floatValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WriteAsync(item.Address, floatValue);
                    break;

                case DataType.Double:
                    if (data is not double doubleValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WriteAsync(item.Address, doubleValue);
                    break;

                default:
                    throw new ArgumentException("数据类型配置错误");
            }

            if (!operateResult.IsSuccess)
            {
                string writeFailMessage = $"PLC写入失败, 地址为[{item.Address}]";
                if (failThrowEx)
                {
                    //throw new EquipmentException(writeFailMessage);
                }
                else
                {
                    Logger.Error(writeFailMessage);
                }
            }
            Logger.Info($"地址为[{item.Address}]写入值$[{data}]");
            return operateResult.IsSuccess;
        }

        public static bool SendPositiveSignal(this PLCData item, int signalTime = 10, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Bool)
            {
                throw new ArgumentException("当前写入的数据类型与配置的不一致");
            }

            var operateResult = item.PLC.Write(item.Address, true).Then(() =>
            {
                Thread.Sleep(signalTime);
                var or = item.PLC.Write(item.Address, false);
                Thread.Sleep(signalTime);
                return or;
            });
            if (!operateResult.IsSuccess)
            {
                string writeFailMessage = $"PLC发送信号失败, 地址为[{item.Address}]";
                if (failThrowEx)
                {
                    //throw new EquipmentException(writeFailMessage);
                }
                else
                {
                    Logger.Error(writeFailMessage);
                }
            }
            Logger.Info($"地址为[{item.Address}]$发送时间为[{signalTime}]的正脉冲");
            return operateResult.IsSuccess;
        }

        public static bool SendHeartbeatPositiveSignal(this PLCData item, int signalTime = 10, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Bool)
            {
                throw new ArgumentException("当前写入的数据类型与配置的不一致");
            }

            var operateResult = item.PLC.Write(item.Address, true).Then(() =>
            {
                Thread.Sleep(signalTime);
                var or = item.PLC.Write(item.Address, false);
                Thread.Sleep(signalTime);
                return or;
            });
            if (!operateResult.IsSuccess)
            {
                string writeFailMessage = $"PLC发送信号失败, 地址为[{item.Address}]";
                if (failThrowEx)
                {
                    //throw new EquipmentException(writeFailMessage);
                }
                else
                {
                    Logger.Error(writeFailMessage);
                }
            }
            //Logger.Info($"地址为[{item.Address}]$发送时间为[{signalTime}]的正脉冲");
            return operateResult.IsSuccess;
        }

        public static async Task SendPositiveSignalAsync(this PLCData item, int signalTime = 10)
        {
            if (item.DataType != DataType.Bool)
            {
                throw new ArgumentException("当前写入的数据类型与配置的不一致");
            }

            await Task.Factory.StartNew(() =>
            {
                item.Write(true);
                Thread.Sleep(signalTime);
                item.Write(false);
                Thread.Sleep(signalTime);
            });
            Logger.Debug($"地址为[{item.Address}]$发送时间为[{signalTime}]的正脉冲成功");
        }

        public static async Task DelaySendPositiveSignalAsync(this PLCData item, int delayTime = 100, int signalTime = 100)
        {
            if (item.DataType != DataType.Bool)
            {
                throw new ArgumentException("当前写入的数据类型与配置的不一致");
            }

            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delayTime);
                item.Write(true);
                Thread.Sleep(signalTime);
                item.Write(false);
            });
            Logger.Debug($"地址为[{item.Address}]$延时[{delayTime}]发送时间为[{signalTime}]的正脉冲");
        }

        public static void DelaySendPositiveSignalThread(this PLCData item, int delayTime = 100, int signalTime = 100)
        {
            if (item.DataType != DataType.Bool)
            {
                throw new ArgumentException("当前写入的数据类型与配置的不一致");
            }

            ThreadStart threadStart = new ThreadStart(() =>
            {
                Thread.Sleep(delayTime);
                item.Write(true);
                Thread.Sleep(signalTime);
                item.Write(false);
            });
            Thread thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
            Logger.Debug($"地址为[{item.Address}]$延时[{delayTime}]发送时间为[{signalTime}]的正脉冲");
        }

        // public static async Task<bool> SendPositiveSignalAsync(this PLCItem item, int signalTime = 100, bool failThrowEx = true)
        // {
        //     if (item.DataType != DataType.Bool)
        //     {
        //         throw new ArgumentException("当前写入的数据类型与配置的不一致");
        //     }
        //     var operateResult = (await item.PLC.WriteAsync(item.Address, true)).Then(() =>
        //     {
        //         Thread.Sleep(signalTime);
        //         return item.PLC.Write(item.Address, false);
        //     });
        //     if (!operateResult.IsSuccess)
        //     {
        //         string writeFailMessage = $"PLC发送信号失败, 地址为[{item.Address}]";
        //         if (failThrowEx)
        //         {
        //             throw new EquipmentException(writeFailMessage);
        //         }
        //         else
        //         {
        //             Logger.Error(writeFailMessage);
        //         }
        //     }
        //
        //     return operateResult.IsSuccess;
        // }

        public static bool Wait(this PLCData item, dynamic data, int readInterval = 100, int waitTimeout = 5000, bool failThrowEx = false)
        {
            OperateResult operateResult;
            string dataTypeMessage = "当前等待的数据类型与配置的不一致";
            switch (item.DataType)
            {
                case DataType.Bool:
                    if (data is not bool boolValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Wait(item.Address, boolValue, readInterval, waitTimeout);
                    break;

                case DataType.Int16:
                    if (data is not short shortValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Wait(item.Address, shortValue, readInterval, waitTimeout);
                    break;

                case DataType.UInt16:
                    if (data is not ushort ushortValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Wait(item.Address, ushortValue, readInterval, waitTimeout);
                    break;

                case DataType.Int32:
                    if (data is not int intValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Wait(item.Address, intValue, readInterval, waitTimeout);
                    break;

                case DataType.UInt32:
                    if (data is not uint uintValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Wait(item.Address, uintValue, readInterval, waitTimeout);
                    break;

                case DataType.Int64:
                    if (data is not long longValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Wait(item.Address, longValue, readInterval, waitTimeout);
                    break;

                case DataType.UInt64:
                    if (data is not ulong ulongValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = item.PLC.Wait(item.Address, ulongValue, readInterval, waitTimeout);
                    break;

                default:
                    throw new ArgumentException("数据类型配置错误");
            }

            if (!operateResult.IsSuccess)
            {
                string writeFailMessage = $"PLC等待失败, 地址为[{item.Address}], 消息:  {operateResult.ToMessageShowString()}";
                if (failThrowEx)
                {
                    //throw new EquipmentException(writeFailMessage);
                }
                else
                {
                    Logger.Error(writeFailMessage);
                }
            }
            Logger.Debug($"地址为[{item.Address}]写入值$[{data}]");
            return operateResult.IsSuccess;
        }

        public static async Task<bool> WaitAsync(this PLCData item, dynamic data, int readInterval = 100, int waitTimeout = 5000, bool failThrowEx = false)
        {
            OperateResult operateResult;
            string dataTypeMessage = "当前等待的数据类型与配置的不一致";
            switch (item.DataType)
            {
                case DataType.Bool:
                    if (data is not bool boolValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WaitAsync(item.Address, boolValue, readInterval, waitTimeout);
                    break;

                case DataType.Int16:
                    if (data is not short shortValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WaitAsync(item.Address, shortValue, readInterval, waitTimeout);
                    break;

                case DataType.UInt16:
                    if (data is not ushort ushortValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WaitAsync(item.Address, ushortValue, readInterval, waitTimeout);
                    break;

                case DataType.Int32:
                    if (data is not int intValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WaitAsync(item.Address, intValue, readInterval, waitTimeout);
                    break;

                case DataType.UInt32:
                    if (data is not uint uintValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WaitAsync(item.Address, uintValue, readInterval, waitTimeout);
                    break;

                case DataType.Int64:
                    if (data is not long longValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WaitAsync(item.Address, longValue, readInterval, waitTimeout);
                    break;

                case DataType.UInt64:
                    if (data is not ulong ulongValue) throw new ArgumentException($"{dataTypeMessage}, 地址是[{item.Address}]");
                    operateResult = await item.PLC.WaitAsync(item.Address, ulongValue, readInterval, waitTimeout);
                    break;

                default:
                    throw new ArgumentException("数据类型配置错误");
            }

            if (!operateResult.IsSuccess)
            {
                string writeFailMessage = $"PLC写入失败, 地址为[{item.Address}], 消息: {operateResult.ToMessageShowString()}";
                if (failThrowEx)
                {
                    throw new EquipmentException(writeFailMessage);
                }              else
                {
                    Logger.Error(writeFailMessage);
                }
            }
            Logger.Info($"地址为[{item.Address}]写入值$[{data}]");
            return operateResult.IsSuccess;
        }

        private static T EnsureReadOperateResult<T>(this OperateResult<T> operateResult, string address, bool failThrowEx = true)
        {
            if (!operateResult.IsSuccess)
            {
                string readFailMessage = $"PLC读取失败, 地址为[{address}], 消息: {operateResult.ToMessageShowString()}";
                if (failThrowEx)
                {
                    throw new EquipmentException(readFailMessage);
                }
                else
                {
                    Logger.Error(readFailMessage);
                }
            }

            return operateResult.Content;
        }

        #region Read Sync

        private const string ReadArgumentExceptionMessage = "当前读取的数据类型与配置的不一致";

        public static bool ReadBool(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Bool)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadBool(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static short ReadInt16(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Int16)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadInt16(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static ushort ReadUInt16(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.UInt16)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadUInt16(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static int ReadInt32(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Int32)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadInt32(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static uint ReadUInt32(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.UInt32)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadUInt32(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static long ReadInt64(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Int64)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadInt64(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static ulong ReadUInt64(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.UInt64)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadUInt64(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static float ReadFloat(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Float)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadFloat(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static double ReadDouble(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Double)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return item.PLC.ReadDouble(item.Address).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        #endregion Read Sync

        #region Read Async

        public static async Task<bool> ReadBoolAsync(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Bool)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadBoolAsync(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<short> ReadInt16Async(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Int16)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadInt16Async(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<ushort> ReadUInt16Async(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.UInt16)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadUInt16Async(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<int> ReadInt32Async(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Int32)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadInt32Async(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<uint> ReadUInt32Async(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.UInt32)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadUInt32Async(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<long> ReadInt64Async(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Int64)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadInt64Async(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<ulong> ReadUInt64Async(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.UInt64)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadUInt64Async(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<float> ReadFloatAsync(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Float)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadFloatAsync(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        public static async Task<double> ReadDoubleAsync(this PLCData item, bool failThrowEx = true)
        {
            if (item.DataType != DataType.Double)
            {
                throw new ArgumentException(ReadArgumentExceptionMessage);
            }
            return (await item.PLC.ReadDoubleAsync(item.Address)).EnsureReadOperateResult(item.Address, failThrowEx);
        }

        #endregion Read Async
    }
}
