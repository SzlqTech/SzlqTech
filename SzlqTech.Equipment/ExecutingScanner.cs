using SzlqTech.Common.EnumType;
using SzlqTech.Entity;
using SzlqTech.Equipment.Scanner;
using SzlqTech.IService;

namespace SzlqTech.Equipment
{
    public class ExecutingScanner : IExecutingScanner
    {
        private readonly IScannerSettingService scannerSettingService;

        public ExecutingScanner(IScannerSettingService scannerSettingService)
        {
            this.scannerSettingService = scannerSettingService;
        }
        public List<IScanner> ScannerList { get ; set ; }=new List<IScanner>();

        public virtual void GetScanners()
        {
            //清除扫描仪列表
            ScannerList.Clear();
            //获取使能的扫描仪
            List<ScannerSetting> scannerSettingList = scannerSettingService.List(o=>o.IsEnable==true);
            foreach (ScannerSetting scannerSetting in scannerSettingList)
            {
                switch ((ScannerType)scannerSetting.ScannerType)
                { 
                    case ScannerType.TCP通用相机采集:
                        {
                            IScanner scanner = new TCPCommonScanner(scannerSetting.PortName, scannerSetting.PortKey, scannerSetting.CodeLevel);
                            ScannerList.Add(scanner);
                            break;
                        }
                   
                }
            }
        }
    }
}
