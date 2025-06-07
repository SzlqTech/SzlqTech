using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IService
{
    public interface ISysDictItemService : IBaseAuditableService<SysDictItem>
    {
        List<SysDictItem> ListByDictId(long dictId);
    }
}