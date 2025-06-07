using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysUserServiceImpl
        : BaseAuditableServiceImpl<ISysUserRepository, SysUser>, ISysUserService
    {
        public SysUserServiceImpl(ISysUserRepository baseRepository) : base(baseRepository)
        {
        }
        public List<SysUser> ListByCondition(string? code = null, string? name = null)
        {
            return BaseRepository.SelectListByCondition(code, name);
        }

        public bool UpdatePasswordByUsername(string username, string password)
        {
            return SqlHelper.RetBool(BaseRepository.UpdatePasswordByUsername(username, password));
        }

        public List<SysUser> ListEnable()
        {
            return BaseRepository.SelectListEnable();
        }

        public List<SysUser> ListExceptRoot()
        {
            return BaseRepository.SelectListExceptRoot();
        }

        public SysUser ListByUsername(string username)
        {
            return BaseRepository.SelectFirstByUsername(username);
        }
    }
}
