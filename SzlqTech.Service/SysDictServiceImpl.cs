using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysDictServiceImpl : BaseAuditableServiceImpl<ISysDictRepository, SysDict>, ISysDictService
    {
        public SysDictServiceImpl(ISysDictRepository baseRepository) : base(baseRepository)
        {
        }
    }
}