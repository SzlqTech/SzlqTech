using SzlqTech.Common.Extensions;
using SzlqTech.Equipment.Machine;

namespace SzlqTech.Equipment
{
    public interface IExecutingMachine
    {
        /// <summary>
        /// 数据接收事件
        /// </summary>
        event EventHandler<TEventArgs<MachineData>>? DataReceived;

        /// <summary>
        /// 注册设备事件
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void RegisterMachineEvent();

        /// <summary>
        /// 取消注册设备事件
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void UnRegisterMachineEvent();

        /// <summary>
        /// 获取所有的机器
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void GetMachines();

        /// <summary>
        /// 打开并连接机器
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>

        void OpenMachine();

        /// <summary>
        /// 关闭并断开机器
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void CloseMachine();

        /// <summary>
        /// 启动机器
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void StartMachine();

        /// <summary>
        /// 停止机器
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void StopMachine();

        bool MainMachineExecute();
    }
}
