
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class MachineDetailServiceImpl :
        BaseAuditableServiceImpl<IMachineDetailRepository, MachineDetail>, IMachineDetailService
    {
        public MachineDetailServiceImpl(IMachineDetailRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
