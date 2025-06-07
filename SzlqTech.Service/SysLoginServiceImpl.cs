using NLog;
using SSzlqTech.Entity;
using SzlqTech.Common.Context;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Helper;
using SzlqTech.Common.Views;
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysLoginServiceImpl :
        BaseAuditableServiceImpl<ISysUserRepository, SysUser>,
        ISysLoginService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ISysMenuRepository _sysMenuRepository = null!;

        private readonly ISysRoleRepository _sysRoleRepository = null!;

        private const string SaUser = "sa";

        private const string SaPassword = "123456";

        public SysLoginServiceImpl(ISysUserRepository baseRepository) : base(baseRepository)
        {
        }

        public SysLoginServiceImpl(ISysUserRepository baseRepository, ISysMenuRepository sysMenuRepository, ISysRoleRepository sysRoleRepository) :
            base(baseRepository)
        {
            _sysMenuRepository = sysMenuRepository;
            _sysRoleRepository = sysRoleRepository;
        }

        // private readonly SysUserMapper _sysUserMapper = new SysUserMapper();

        public void UserLogin(string username, string password, bool remember)
        {
            PermissionContext permissionContext;
            var sysUser = GetUserByLogin(username, password);
            if (sysUser == null)
                return;
            if (sysUser.Username.ToLower().Equals(SaUser))
            {
                permissionContext = GetPermissionContextByUserAndMenus(sysUser, new List<SysMenu>());
                permissionContext.RoleCode = "System";
                //UserContext.SetContext(permissionContext);
                logger.Info($"登录成功");
                return;
            }

            if (sysUser.Id == 0)
            {
                List<SysMenu> sysMenus = _sysMenuRepository!.SelectListOrderByCreateTime();
                permissionContext = GetPermissionContextByUserAndMenus(sysUser, sysMenus);
                permissionContext.RoleCode = "System";
            }
            else
            {
                var sysRole = _sysRoleRepository.SelectById(sysUser.RoleId);
                if (sysRole == null) throw new Exception("找不到角色");
                // List<SysMenu> sysMenus = _sysRoleMenuDAL.GetMenuByRoleId(sysUser.RoleId);
                List<SysMenu> sysMenus = _sysMenuRepository.SelectListByRoleId(sysUser.RoleId);

                permissionContext = GetPermissionContextByUserAndMenus(sysUser, sysMenus);
                permissionContext.RoleCode = sysRole.Code;
            }

            if (permissionContext == null)
            {
                throw new Exception("Permission context is null");
            }
            ProcessPermission(permissionContext);
            logger.Info($"登录成功");

            if (!remember)
            {
                LoginData.Save(username, null, remember);
            }
            else
            {
                LoginData.Save(username, password, remember);
            }
            //SA用户不存储密码信息
            // if (username.ToLower() != "sa")
        }

        /// <summary>
        /// 根据用户名和密码获取用户实体, 并判断密码是否正确
        /// 如果是sa用户, 就新建实体
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private SysUser? GetUserByLogin(string username, string password)
        {
            SysUser? sysUser;
            if (username.ToLower().Equals(SaUser))
            {
                if (password.ToLower().Equals(SaPassword))
                {
                    sysUser = new SysUser
                    {
                        Username = SaUser,
                        PersonName = SaUser,
                        Code = "sa",
                        Password = SaPassword
                    };
                    return sysUser;
                }
            }

            sysUser = BaseRepository.SelectFirstByUsername(username);
            if (sysUser == null)
            {
                throw new Exception("用户不存在");
            }
            if (sysUser.Status == 0)
            {
                throw new Exception("");
            }

            string md5Password = HashHelper.CreateMD5(password).ToLower();
            if (md5Password != sysUser.Password)
            {
                throw new Exception("密码错误");
            }
            sysUser.LastLoginTime = DateTime.Now;
            BaseRepository.UpdateById(sysUser);
            return sysUser;
        }

        /// <summary>
        /// 根据用户和菜单获取权限上下文
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="sysMenus"></param>
        /// <returns></returns>
        private PermissionContext GetPermissionContextByUserAndMenus(SysUser sysUser, List<SysMenu> sysMenus)
        {
            PermissionContext permissionContext = new()
            {
                UserCode = sysUser.Code,
                Username = sysUser.Username, //这里是用户姓名
                Session = Guid.NewGuid().ToString(),
                PersonName = sysUser.PersonName,
                RoleId = sysUser.RoleId,
                UserId = sysUser.Id,
                // EntryType 0:目录 1:菜单 2:模块 3:功能按钮
                ViewList = sysMenus.Where(m =>
                        m.EntryType is EntryType.Catalog or EntryType.Menu)
                        .OrderBy(m => m.ParentId)
                        .ThenBy(m => m.Ordinal).ToList()!,
                FuncList = sysMenus.Where(m =>
                        m.EntryType is EntryType.Button or EntryType.Operate)
                        .OrderBy(m => m.ParentId)
                        .ThenBy(m => m.Ordinal).ToList()
            };
            return permissionContext;
        }

        /// <summary>
        /// 处理权限上下文
        /// 权限上下文将菜单和按钮权限保存到View全局变量
        /// 权限按钮的绘制统一放到SieracTech.CommonUI里的BaseForm类
        /// </summary>
        /// <param name="context"></param>
        private void ProcessPermission(PermissionContext context)
        {
            // Stopwatch stopwatch = new Stopwatch();
            // stopwatch.Start();
            UserContext.SetContext(context);

            List<FuncStrip> funcStrips = context.FuncList.ConvertAll(o =>
            {
                // SysMenu parent = _sysMenuDAL.GetById(o.ParentId);
                SysMenu parent = context.ViewList.Find(m => m.Id == o.ParentId);
                return new FuncStrip
                {
                    Command = o.Command,
                    ParentUrl = parent.Url,
                    Id = o.Id.ToString(),
                    ViewId = o.ParentId.ToString(),
                    EntryType = o.EntryType,
                    Ordinal = o.Ordinal,
                    Text = o.Text,
                    TextEN = o.TextEN,
                    TextZH = o.TextZH,
                    ViewText = parent.Text
                };
            });
            List<ViewStrip> views = context.ViewList.ConvertAll(o => new ViewStrip
            {
                Code = o.Url,
                Url = o.Url,
                Description = o.Text,
                Id = o.Id.ToString(),
                Ordinal = o.Ordinal,
                ParentId = o.ParentId.ToString(),
                RootId = o.RootId.ToString(),
                Text = o.Text,
                TextEN = o.TextEN,
                TextZH = o.TextZH,
                EntryType = o.EntryType,
                Assembly = o.Assembly
            });

            ViewContext.SetContextStrip(views, funcStrips);
            // stopwatch.Stop();
            // LogHelper.Debug($"ProcessPermission time {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
    }
}