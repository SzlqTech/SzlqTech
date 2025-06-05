using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class ProductServiceImpl
        : BaseAuditableServiceImpl<IProductRepository, Product>, IProductService
    {
        public ProductServiceImpl(IProductRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
