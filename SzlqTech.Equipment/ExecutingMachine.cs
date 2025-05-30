using HslCommunication.Core.Net;
using NLog;
using SzlqTech.Common.Extensions;
using SzlqTech.Equipment.Machine;
using SzlqTech.IService;

namespace SzlqTech.Equipment
{
    public class ExecutingMachine : IExecutingMachine
    {
        public event EventHandler<TEventArgs<MachineData>>? DataReceived;

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
        public Dictionary<string, NetworkDeviceBase> TcpDeviceDictionary { get; }= new Dictionary<string, NetworkDeviceBase>();

        

        public ExecutingMachine(IMachineSettingService machineSettingService,IMachineDetailService machineDetailService)
        {
            this.machineSettingService = machineSettingService;
            this.machineDetailService = machineDetailService;
        }



        public void CloseMachine()
        {
            
        }

        public void GetMachines()
        {
            
        }

        public bool MainMachineExecute()
        {
            return true;
        }

        public void OpenMachine()
        {
            
        }

        public void RegisterMachineEvent()
        {
            
        }

        public void StartMachine()
        {
            
        }

        public void StopMachine()
        {
            
        }

        public void UnRegisterMachineEvent()
        {
            
        }
    }
}
