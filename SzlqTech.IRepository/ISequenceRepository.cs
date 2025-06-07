using SzlqTech.DbHelper;
using SzlqTech.Entity;

namespace SzlqTech.IRepository
{
    public interface ISequenceRepository : IBaseRepository<SysSequence>
    {
        /// <summary>
        /// 根据编码更新当前值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        int UpdateCurrentValueByCode(string code, long currentValue);
    }
}