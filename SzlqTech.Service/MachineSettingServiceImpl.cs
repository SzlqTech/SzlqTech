using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class MachineSettingServiceImpl
        : BaseAuditableServiceImpl<IMachineSettingRepository, MachineSetting>, IMachineSettingService
    {
        public MachineSettingServiceImpl(IMachineSettingRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
