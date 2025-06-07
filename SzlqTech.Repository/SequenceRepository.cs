using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;

namespace SzlqTech.Repository
{
    public class SequenceRepository : BaseAuditableRepository<SysSequence>, ISequenceRepository
    {
        public int UpdateCurrentValueByCode(string code, long currentValue)
        {
            // return base.Execute("update Sequence set CurrentValue = @CurrentValue where Code = @Code",
            //     new SysSequence { Code = code, CurrentValue = currentValue });
            return UpdateColumns(s => s.CurrentValue == currentValue, s => s.Code == code);
        }
    }
}