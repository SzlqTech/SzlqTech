

using SzlqTech.Common.Extensions;

namespace SzlqTech.Equipment.Scanner
{
    public interface IScanner
    {
        /// <summary>
        /// 是否打开
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// 端口键
        /// </summary>
        string PortKey { get; }

        /// <summary>
        /// 端口名称
        /// </summary>
        string PortName { get; }

        /// <summary>
        /// 关闭扫描器连接
        /// </summary>
        void Close();

        /// <summary>
        /// 打开扫描器连接
        /// </summary>
        void Open();

        /// <summary>
        /// 数据接收事件
        /// </summary>
        event EventHandler<TEventArgs<ScanData>> DataReceived;

    }
}
