using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysDepartmentServiceImpl : BaseAuditableServiceImpl<ISysDepartmentRepository, SysDepartment>, ISysDepartmentService
    {
        public SysDepartmentServiceImpl(ISysDepartmentRepository baseRepository) : base(baseRepository)
        {
        }
    }
}