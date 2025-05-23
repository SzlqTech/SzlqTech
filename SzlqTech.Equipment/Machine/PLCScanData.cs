using HslCommunication.Core;
using SzlqTech.Common.EnumType;

namespace SzlqTech.Equipment.Machine
{
    /// <summary>
    /// PLC扫描项目
    /// </summary>
    public class PLCScanData
    {
        private dynamic? data;

        public PLCScanData() { }

        public PLCScanData(string portKey, IReadWriteDevice plc, string address, DataType dataType)
        {
            PortKey = portKey;
            PLC = plc;
            Address = address;
            DataType = dataType;
        }

        // public PLCScanItem(string portKey, string address, Type dataType, dynamic data)
        // {
        //     PortKey = portKey;
        //     Address = address;
        //     this.data = data;
        //     DataType = dataType;
        // }


        // [Name("标签")] 
        // [Index(0)] 
        public string PortKey { get; set; } = null!;

        public IReadWriteDevice PLC { get; set; } = null!;

        // [Name("地址")]
        // [Index(1)]
        public string Address { get; set; } = null!;

        // public string Length { get; set; } = null!;

        // [Name("类型")]
        // [Index(2)]
        public DataType DataType { get; set; }

        /// <summary>
        /// 扫描周期-建议配置为100ms或者1000ms
        /// </summary>
        // [Name("扫描周期")]
        // [Index(3)]
        public int ScanCycle { get; set; } = 1000;

        // public string WriteAddress { get; set; } = null!;

        public string? Message { get; set; }

        /// <summary>
        /// 开始采集和停止采集
        /// </summary>
        // [Ignore]
        public bool Enable { get; set; } = true;

        // [Ignore]
        public dynamic? Data
        {
            get => data;
            set
            {
                // ???
                // PLC数据无变化时，大概率数据不会初始化
                // if (true)
                if (data != value)
                {
                    data = value;
                    DataReceived?.Invoke(this, null);
                }
            }
        }

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event EventHandler DataReceived = null!;

    }
}
