using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysSequenceServiceImpl : BaseServiceImpl<ISequenceRepository, SysSequence>, ISysSequenceService
    {
        public SysSequenceServiceImpl(ISequenceRepository baseRepository) : base(baseRepository)
        {
        }
    }
}