using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;

namespace SzlqTech.Repository
{
    public class SysDictItemRepository : BaseAuditableRepository<SysDictItem>, ISysDictItemRepository
    {
        public List<SysDictItem> SelectListByDictId(long dictId)
        {
            return SelectList(o => o.DictId == dictId);
            // string sql = "Select * from SysDictItem where DictId=@DictId";
            // return base.Query<SysDictItem>(sql, new { DictId = dictId });
        }
    }
}