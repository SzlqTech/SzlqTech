using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzlqTech.Equipment.Scanner
{
    /// <summary>
    /// 扫描数据
    /// </summary>
    public class ScanData
    {
        /// <summary>
        /// 采集的编码数据
        /// </summary>
        public string Data { get; set; } = null!;

        public byte[]? Content { get; set; }

        /// <summary>
        /// 采集编码的端口键
        /// </summary>
        public string? PortKey { get; set; }

        /// <summary>
        /// 采集编码的端口名称
        /// </summary>
        public string? PortName { get; set; }

        /// <summary>
        /// 采集编码的层级
        /// </summary>
        public int CodeLevel { get; set; } = 0;
    }
}
