using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Inovance;
using HslCommunication.Profinet.Siemens;
using NLog;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Extensions;
using SzlqTech.Common.MultiThreads;
using SzlqTech.Common.Nlogs;
using SzlqTech.Equipment.Machine;
using SzlqTech.IService;

namespace SzlqTech.Equipment
{
    public class ExecutingMachine : IExecutingMachine
    {
        #region 属性定义
        /// <summary>
        /// 日志
        /// </summary>
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 机器列表
        /// </summary>
        private readonly IMachineSettingService machineSettingService;

        /// <summary>
        /// 机器明细详情
        /// </summary>
        private readonly IMachineDetailService machineDetailService;

        /// <summary>
        /// 心跳扫描数据列表
        /// </summary>
        private List<MachineLinkData> HeartbeatScanDatas=new List<MachineLinkData>();

        /// <summary>
        /// 心跳键
        /// </summary>
        private const string HeartbeatKey = "Heartbeat";

        /// <summary>
        /// 网口TCP设备字典
        /// </summary>
        public Dictionary<string, NetworkDeviceBase> TCPDeviceDictionary { get; } = new Dictionary<string, NetworkDeviceBase>();


        /// <summary>
        /// PLC扫描项目字典
        /// </summary>
        protected PLCScanDictionary PLCScanDictionary { get; } = new PLCScanDictionary();

        /// <summary>
        /// PLC写入项目字典
        /// </summary>
        protected PLCDictionary PLCDictionary { get; } = new PLCDictionary();

        /// <summary>
        /// 信号锁
        /// </summary>
        protected virtual SemaphoreQueue MachineLock { get; set; } = new SemaphoreQueue(1, 1);

        /// <summary>
        /// 是否开启设备
        /// </summary>
        private bool IsOpen = false;

        private Task? HeartbeatTask = null;
        #endregion

        public ExecutingMachine(IMachineSettingService machineSettingService,IMachineDetailService machineDetailService)
        {
            this.machineSettingService = machineSettingService;
            this.machineDetailService = machineDetailService;
        }


        #region 数据接受事件
        public event EventHandler<TEventArgs<MachineData>>? DataReceived;


        public event EventHandler<TEventArgs<List<MachineLinkData>>>? PLCDataReceived;
        /// <summary>
        /// 引发设备数据接收事件
        /// </summary>
        /// <param name="data"></param>
        protected void RaiseDataReceived(MachineData data)
        {
            DataReceived?.Invoke(this, new TEventArgs<MachineData>(data));
        } 

        public void RaisePLCDataReceived(List<MachineLinkData> data)
        {
            PLCDataReceived?.Invoke(this, new TEventArgs<List<MachineLinkData>>(data));
        }
        #endregion

        #region 设备启动顺序
        /// <summary>
        /// 获取PLC设备状态
        /// </summary>
        /// <returns></returns>
        public bool MainMachineExecute()
        {
            return true;
        }


        /// <summary>
        /// 1.查询数据库需要启动的PLC
        /// </summary>
        /// <exception cref="EquipmentException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public void GetMachines()
        {
            TCPDeviceDictionary.Clear();

            var machineSettings = machineSettingService.List(o => o.IsEnable);
            if (machineSettings.Count == 0)
            {
                throw new EquipmentException("沒有使能的PLC");
            }
            foreach (var machineSetting in machineSettings)
            {
                switch (machineSetting.MachineModelEnum)
                {
                    case MachineModel.SiemensS200Smart:
                        var smart = new SiemensS7Net(SiemensPLCS.S200Smart, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, smart);
                        break;

                    case MachineModel.SiemensS1200:
                        var s1200 = new SiemensS7Net(SiemensPLCS.S1200, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, s1200);
                        break;
                    case MachineModel.SiemensS1500:
                        var s1500 = new SiemensS7Net(SiemensPLCS.S1500, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, s1500);
                        break;

                    case MachineModel.InovanceAMNet:
                        var amNet = new InovanceTcpNet(InovanceSeries.AM, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, amNet);
                        break;

                    case MachineModel.InovanceEasyNet:
                        var easy = new InovanceTcpNet(InovanceSeries.H5U, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, easy);
                        break;

                    case MachineModel.InovanceH3UNet:
                        var h3u = new InovanceTcpNet(InovanceSeries.H3U, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, h3u);
                        break;

                    case MachineModel.InovanceH5UNet:
                        var h5u = new InovanceTcpNet(InovanceSeries.H5U, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, h5u);
                        break;

                    case MachineModel.ModbusTcp:
                        var modbusTcp = new ModbusTcpNet(machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, modbusTcp);
                        break;
                    //case MachineModel.BeckoffAds2:
                    //    var beckoff2Tcp = new BeckhoffAdsNet(machineSetting.PortKey,801);
                    //    TCPDeviceDictionary.Add(machineSetting.PortKey, beckoff2Tcp);
                    //    break;
                    //case MachineModel.BeckoffAds3:
                    //    var beckoff3Tcp = new BeckhoffAdsNet(machineSetting.PortKey, 851);
                    //    TCPDeviceDictionary.Add(machineSetting.PortKey, beckoff3Tcp);
                    //    break;
                    default:
                        throw new NotImplementedException("暂未支持");
                }
            }
        }

        /// <summary>
        ///2. 打开并连接机器
        /// </summary>
        public void OpenMachine()
        {
            HeartbeatScanDatas.Clear();
            try
            {
                foreach (var pair in TCPDeviceDictionary)
                {
                    var tcpDevice = pair.Value;
                    tcpDevice.ConnectTimeOut = 1000;
                    var operateResult = tcpDevice.ConnectServer().EnsureSuccess($"[{pair.Key}]连接网口");
                    if (!operateResult.IsSuccess)
                    {
                        logger.ErrorHandler($"[{pair.Key}]连接网口失败: {operateResult.Message}");
                        continue;
                    }
                    string code = pair.Key;
                    var machineSetting = machineSettingService.GetByCode(code)!;
                    var machineDetails = machineDetailService.List(m =>
                        m.MachineId == machineSetting.Id);
                    foreach (var machineDetail in machineDetails)
                    {
                        if (machineDetail.IsEnableHeartbeat)
                        {
                            var plcData = new MachineLinkData(machineDetail.PortKey, tcpDevice, machineDetail.Address,
                                machineDetail.DataTypeEnum, machineDetail.DecimalPointShiftType, operateResult);
                            plcData.PLCPortKey = code;
                            HeartbeatScanDatas.Add(plcData);
                        }
                        else
                        {
                            if (machineDetail.ScanStatus == SettingStatus.Enable)
                            {
                                PLCScanDictionary.AddToStartScan(machineDetail.PortKey, tcpDevice, machineDetail.Address,
                                    machineDetail.DataTypeEnum, machineDetail.ScanCycle);
                            }

                            PLCDictionary.Add(machineDetail.PortKey, tcpDevice, machineDetail.Address,
                                machineDetail.DataTypeEnum, machineDetail.DecimalPointShiftType);
                        }

                    }
                }         
            }
            catch (Exception ex)
            {
                logger.ErrorHandler($"打开机器失败:[{ex.Message}]");          
            }
            finally
            {           
                RaisePLCDataReceived(HeartbeatScanDatas);
                //只有一个plc链接成功就可以算开启成功
                if (HeartbeatScanDatas.Count > 0) IsOpen = true;
                else
                {
                    IsOpen = false;
                    throw new EquipmentException("没有可用的PLC设备，请检查网络连接或PLC配置。");
                }
            }
        }


        /// <summary>
        /// 3.注册PLC扫描接受事件
        /// </summary>
        public void RegisterMachineEvent()
        {
            foreach (var plcScanData in PLCScanDictionary.PLCScanDict.Select(pair => pair.Value))
            {
                plcScanData.DataReceived -= PLCScanData_DataReceived;
                plcScanData.DataReceived += PLCScanData_DataReceived;
            }
        }

        /// <summary>
        /// 4.发送启动命令
        /// </summary>
        public void StartMachine()
        {
            //发送心跳
             HeartbeatTask=Task.Factory.StartNew(ExcuteHeartbeat,TaskCreationOptions.LongRunning);
        }

        public void ExcuteHeartbeat()
        {
            logger.InfoHandler("开始执行心跳扫描任务");
            while (IsOpen)
            {       
                Parallel.ForEach(HeartbeatScanDatas, item =>
                {
                    try
                    {
                        if(item.OperateResult.IsSuccess)
                            item.SendHeartbeatPositiveSignal(50);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorHandler(ex.Message);
                        logger.InfoHandler("执行心跳扫描任务错误");
                    }
                });
                Thread.Sleep(500);
            }
           
        }

    
        #endregion

        #region 停止设备顺序
        /// <summary>
        /// 1.发送停止命令
        /// </summary>
        public void StopMachine()
        {
            if (IsOpen)
            {
                IsOpen = false;
                HeartbeatTask?.Wait();
                //更新PLC链接状态
                HeartbeatScanDatas.ForEach(item => item.OperateResult.IsSuccess = false);
                RaisePLCDataReceived(HeartbeatScanDatas);
                logger.InfoHandler("停止执行心跳扫描任务");
            }
            
        }

        /// <summary>
        /// 2.注销设备接受事件
        /// </summary>
        public void UnRegisterMachineEvent()
        {
            foreach (var plcScanData in PLCScanDictionary.PLCScanDict.Select(pair => pair.Value))
            {
                plcScanData.DataReceived -= PLCScanData_DataReceived;
            }
        }

        /// <summary>
        /// 3.关闭并断开机器
        /// </summary>
        public void CloseMachine()
        {
            PLCScanDictionary.ClearToStopScan();
            PLCDictionary.Clear();
            HeartbeatScanDatas.Clear();
            foreach (var pair in TCPDeviceDictionary)
            {
                var tcpDevice = pair.Value;
                tcpDevice.ConnectClose().EnsureSuccess($"[{pair.Key}]断开网口");
            }
            TCPDeviceDictionary.Clear();
        }
        #endregion


        /// <summary>
        ///  PLC扫描接收事件, 一般不继承和重写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PLCScanData_DataReceived(object? sender, EventArgs e)
        {
            if (sender is PLCScanData plcScanData)
            {
                MachineData data = new MachineData();
                data.PortKey = plcScanData.PortKey;
                data.Data = plcScanData.Data;
                RaiseDataReceived(data);
            }
        }


        #region 同步读写值

        public bool ReadBoolValue(string key)
        {
            return PLCDictionary.Get(key).ReadBool();
        }

        public short ReadInt16Value(string key)
        {
            return PLCDictionary.Get(key).ReadInt16();
        }

        public int ReadInt32Value(string key)
        {
            return PLCDictionary.Get(key).ReadInt32();
        }

        public float ReadFloatValue(string key)
        {
            return PLCDictionary.Get(key).ReadFloat();
        }

        public double ReadDoubleValue(string key)
        {
            return PLCDictionary.Get(key).ReadDouble();
        }

        public string ReadStringValue(string key)
        {
            return PLCDictionary.Get(key).ReadString();
        }


        public bool WriteValue(string key, object value)
        {
           return  PLCDictionary.Get(key).Write(value);
        }


        #endregion

        #region 异步读写数据

        public async Task<bool> ReadBoolValueAsync(string key)
        {
            return await PLCDictionary.Get(key).ReadBoolAsync();
        }

        public async Task<short> ReadInt16ValueAsync(string key)
        {
            return await PLCDictionary.Get(key).ReadInt16Async();
        }

        public async Task<int> ReadInt32ValueAsync(string key)
        {
            return await PLCDictionary.Get(key).ReadInt32Async();
        }

        public async Task<float> ReadFloatValueAsync(string key)
        {
            return await PLCDictionary.Get(key).ReadFloatAsync();
        }

        public async Task<double> ReadDoubleValueAsync(string key)
        {
            return await PLCDictionary.Get(key).ReadDoubleAsync();
        }

        public async Task<string> ReadStringValueAsync(string key)
        {
            return await PLCDictionary.Get(key).ReadStringAsync();
        }

        public async Task<bool> WriteValueAsync(string key, object value)
        {
            return await PLCDictionary.Get(key).WriteAsync(value);
        }

        #endregion
    }
}
