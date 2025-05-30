using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Inovance;
using HslCommunication.Profinet.Siemens;
using NLog;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Extensions;
using SzlqTech.Common.MultiThreads;
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
        #endregion

        public ExecutingMachine(IMachineSettingService machineSettingService,IMachineDetailService machineDetailService)
        {
            this.machineSettingService = machineSettingService;
            this.machineDetailService = machineDetailService;
        }


        #region 数据接受事件
        public event EventHandler<TEventArgs<MachineData>>? DataReceived;
        /// <summary>
        /// 引发设备数据接收事件
        /// </summary>
        /// <param name="data"></param>
        protected void RaiseDataReceived(MachineData data)
        {
            DataReceived?.Invoke(this, new TEventArgs<MachineData>(data));
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

            var machineSettings = machineSettingService.List(o => o.StatusEnable);
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

                    case MachineModel.InovanceAMNet:
                        var amNet = new InovanceTcpNet(InovanceSeries.AM, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, amNet);
                        break;

                    case MachineModel.InovanceEasyNet:
                        var easy = new InovanceTcpNet(InovanceSeries.H5U, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, easy);
                        break;

                    case MachineModel.InovanceH3UNet:
                        var h3u = new InovanceTcpNet(InovanceSeries.H5U, machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, h3u);
                        break;
                    case MachineModel.ModbusTcp:
                        var modbusTcp = new ModbusTcpNet(machineSetting.PortKey);
                        TCPDeviceDictionary.Add(machineSetting.PortKey, modbusTcp);
                        break;
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
            foreach (var pair in TCPDeviceDictionary)
            {
                var tcpDevice = pair.Value;
                tcpDevice.ConnectTimeOut = 1000;
                tcpDevice.ConnectServer().EnsureSuccess($"[{pair.Key}]连接网口");
                string code = pair.Key;
                var machineSetting = machineSettingService.GetByCode(code)!;
                var machineDetails = machineDetailService.List(m =>
                    m.MachineId == machineSetting.Id);
                foreach (var machineDetail in machineDetails)
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


        /// <summary>
        /// 3.注册PLC扫描接受事件
        /// </summary>
        public void RegisterMachineEvent()
        {
            foreach (var plcScanData in PLCScanDictionary.PLCScanDict.Select(pair => pair.Value))
            {
                plcScanData.DataReceived += PLCScanData_DataReceived;
            }
        }

        /// <summary>
        /// 4.发送启动命令
        /// </summary>
        public void StartMachine()
        {

        }
        #endregion

        #region 停止设备顺序
        /// <summary>
        /// 1.发送停止命令
        /// </summary>
        public void StopMachine()
        {

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
    }
}
