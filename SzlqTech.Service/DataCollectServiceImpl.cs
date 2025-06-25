
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class DataCollectServiceImpl
        : BaseAuditableServiceImpl<IDataCollectRepository, DataCollect>, IDataCollectService
    {
        public DataCollectServiceImpl(IDataCollectRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
