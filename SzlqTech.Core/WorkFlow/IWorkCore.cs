
using HslCommunication.Secs;
using SzlqTech.Common.EnumType;
using SzlqTech.Entity;

namespace SzlqTech.Core.WorkFlow
{
    public interface IWorkCore
    {
        /// <summary>
        /// 启动流程
        /// </summary>
        bool StartExecute();

      

        /// <summary>
        /// 停止流程
        /// </summary>
        bool StopExecute();

        #region 同步读取数据
        bool ReadBoolValue(string key);

        Int16 ReadInt16Value(string key);

        Int32 ReadInt32Value(string key);

        float ReadFloatValue(string key);

        Double ReadDoubleValue(string key);

        string ReadStringValue(string key);

        bool WriteValue(string key, object value);

        bool WriteValueByMachine(MachineDetail detail,DataType dataType, object value);
        #endregion

        #region 异步读写数据
        Task<bool> ReadBoolValueAsync(string key);

        Task<Int16> ReadInt16ValueAsync(string key);

        Task<Int32> ReadInt32ValueAsync(string key);

        Task<float> ReadFloatValueAsync(string key);

        Task<Double> ReadDoubleValueAsync(string key);

        Task<string> ReadStringValueAsync(string key);

        Task<bool> WriteValueAsync(string key, object value);

        Task<bool> WriteValueByMachineAsync(MachineDetail detail, DataType dataType, object value);
        #endregion

        SecsHsms GetSecsHsms(string portKey);
    }
}
