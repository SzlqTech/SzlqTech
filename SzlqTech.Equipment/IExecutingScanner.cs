using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Equipment.Scanner;

namespace SzlqTech.Equipment
{
    public interface IExecutingScanner
    {
        List<IScanner> ScannerList { get; set; }

        /// <summary>
        /// 按配置获取采集器
        /// </summary>
        void GetScanners();
    }
}
