
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;

namespace SzlqTech.Repository
{
    public class ProductRepository:BaseAuditableRepository<Product>,IProductRepository
    {
    }
}
