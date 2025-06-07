
using SzlqTech.DbHelper;
using SzlqTech.Entity;


namespace SzlqTech.IRepository
{
    public interface ISysDictItemRepository : IBaseAuditableRepository<SysDictItem>
    {
        /// <summary>
        /// 根据字典id查询字典项目
        /// </summary>
        /// <param name="dictId">字典id</param>
        /// <returns></returns>
        List<SysDictItem> SelectListByDictId(long dictId);
    }
}