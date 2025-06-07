using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;

namespace SzlqTech.Repository
{
    public class SysDictRepository : BaseAuditableRepository<SysDict>, ISysDictRepository
    {
        // public List<SysDict> GetAllOrderByCreateTime()
        // {
        //     string sql = "Select * from SysDict order by CreateTime desc";
        //     return DapperBase.Instance.Query<SysDict>(sql).ToList();
        // }
    }
}