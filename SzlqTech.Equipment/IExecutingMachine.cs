using NPOI.SS.Formula.Functions;
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
        /// plcz状态接受事件
        /// </summary>
        event EventHandler<TEventArgs<List<MachineLinkData>>>? PLCDataReceived;

        event EventHandler<TEventArgs<bool>>? MachineStatusReceived;

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


        #region 同步读取数据
        bool ReadBoolValue(string key);

        Int16 ReadInt16Value(string key);

        Int32 ReadInt32Value(string key);

        float ReadFloatValue(string key);

        Double ReadDoubleValue(string key);

        string ReadStringValue(string key);

        bool WriteValue(string key, object value);

        dynamic ReadValueByPortKey(string portKey);
        #endregion

        #region 异步读写数据
        Task<bool> ReadBoolValueAsync(string key);

        Task<Int16> ReadInt16ValueAsync(string key);

        Task<Int32> ReadInt32ValueAsync(string key);

        Task<float> ReadFloatValueAsync(string key);

        Task<Double> ReadDoubleValueAsync(string key);

        Task<string> ReadStringValueAsync(string key);

        Task<bool> WriteValueAsync(string key, object value);

        Task<dynamic> ReadValueByPortKeyAsync(string portKey);
        #endregion

    }
}
