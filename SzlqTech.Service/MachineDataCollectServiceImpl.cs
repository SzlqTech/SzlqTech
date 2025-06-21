
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class MachineDataCollectServiceImpl
       : BaseAuditableServiceImpl<IMachineDataCollectRepostory, MachineDataCollect>, IMachineDataCollectService
    {
        public MachineDataCollectServiceImpl(IMachineDataCollectRepostory baseRepository) : base(baseRepository)
        {
        }
    }
}
