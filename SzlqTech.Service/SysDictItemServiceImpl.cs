using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysDictItemServiceImpl : BaseAuditableServiceImpl<ISysDictItemRepository, SysDictItem>, ISysDictItemService
    {
        public SysDictItemServiceImpl(ISysDictItemRepository baseRepository) : base(baseRepository)
        {
        }

        public List<SysDictItem> ListByDictId(long dictId)
        {
            return BaseRepository.SelectListByDictId(dictId);
        }
    }
}