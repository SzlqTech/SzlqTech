
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class QrCodeServiceImpl
        : BaseAuditableServiceImpl<IQrCodeRepository, QrCode>, IQrCodeService
    {
        public QrCodeServiceImpl(IQrCodeRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
