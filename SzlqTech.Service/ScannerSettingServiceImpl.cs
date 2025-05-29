
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class ScannerSettingServiceImpl
        : BaseAuditableServiceImpl<IScannerSettingRepository, ScannerSetting>, IScannerSettingService
    {
        public ScannerSettingServiceImpl(IScannerSettingRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
