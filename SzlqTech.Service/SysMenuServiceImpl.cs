
using SqlSugar;
using System.Reflection;
using SzlqTech.Common.Context;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Views;
using SzlqTech.DbHelper;
using SzlqTech.Entity;
using SzlqTech.IRepository;
using SzlqTech.IService;

namespace SzlqTech.Service
{
    public class SysMenuServiceImpl : BaseAuditableServiceImpl<ISysMenuRepository, SysMenu>, ISysMenuService
    {
        public SysMenuServiceImpl(ISysMenuRepository baseRepository) : base(baseRepository)
        {
        }

        public List<SysMenu> GetLoginMenuList()
        {
            List<SysMenu> list = BaseRepository.SelectListOrderByCreateTime();
            return list;
        }

        /// <summary>
        /// 从二进制文件中通过反射的方式获取菜单
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<SysMenu> ReadFromFile(string fileName)
        {
            
            Assembly assembly = Assembly.LoadFrom(fileName);
            var types = assembly.GetTypes();
            var viewTypes = Array.FindAll(types, t => t.GetCustomAttributes(typeof(ViewAttribute), false).Length > 0);
            List<SysMenu> sysMenus = new List<SysMenu>();
            foreach (Type viewType in viewTypes)
            {
                ViewAttribute viewAttribute = (ViewAttribute)viewType.GetCustomAttributes(typeof(ViewAttribute), false)[0];
                // Type viewType = viewAttribute.ViewType;
                string parentText = viewAttribute.Parent;
                string parentENText = viewAttribute.ParentEN;
                string parentZHText = viewAttribute.ParentZH;
                string rootText = viewAttribute.Root;
                string rootENText = viewAttribute.RootEN;
                string rootZHText = viewAttribute.RootZH;

                if (string.IsNullOrEmpty(parentText) || string.IsNullOrEmpty(rootText))
                {
                    //throw new NotSupportedException();
                    continue;
                }
                SysMenu sysMenu = new SysMenu
                {
                    Id = SnowFlake.NewLongId,
                    Text = viewAttribute.Text,
                    TextEN = viewAttribute.TextEN,
                    TextZH = viewAttribute.TextZH,
                    EntryType = viewAttribute.EntryType,
                    Assembly = Path.GetFileNameWithoutExtension(fileName),
                    Url = viewType.FullName,
                    Ordinal = viewAttribute.Ordinal,
                    CreateTime = DateTime.Now,
                    CreateUser = UserContext.UserId,
                    // sysMenu.UpdateUser = UserContext.Username;
                    // sysMenu.UpdateTime = DateTime.Now;
                   // Remark = viewAttribute.Description,
                    Icon=viewAttribute.Description
                    
                };

                SysMenu parentMenu = BaseRepository.SelectFirstByText(parentText);
                if (parentMenu == null)
                {
                    parentMenu = sysMenus.Find(m => m.Text == parentText);
                }
                if (parentMenu == null)
                {
                    //目录
                    parentMenu = new SysMenu
                    {
                        Id = SnowFlake.NewLongId,
                        EntryType = EntryType.Catalog,
                        Text = parentText,
                        TextEN = parentENText,
                        TextZH = parentZHText,
                        Ordinal = 0,
                        RootText = parentText,
                        CreateTime = DateTime.Now,
                        CreateUser = UserContext.UserId,
                        Remark = viewAttribute.Description,
                        
                    };
                    parentMenu.RootId = parentMenu.Id;

                    sysMenus.Add(parentMenu);
                }
                sysMenu.ParentId = parentMenu.Id;
                sysMenu.ParentText = parentMenu.Text;

                if (rootText == parentText)
                {
                    sysMenu.RootId = parentMenu.Id;
                    sysMenu.RootText = parentMenu.Text;
                }
                else
                {
                    throw new NotImplementedException();
                }

                sysMenus.Add(sysMenu);

                // PropertyInfo[] properties = viewType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public);
                MethodInfo[] methodInfos = viewType.GetMethods(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public);

                foreach (MethodInfo methodInfo in methodInfos)
                {
                    object[] attrs = methodInfo.GetCustomAttributes(typeof(FuncAttribute), true);
                    if (attrs.Length > 0)
                    {
                        FuncAttribute funcAttribute = (FuncAttribute)attrs[0];
                        SysMenu funcMenu = new SysMenu
                        {
                            Id = SnowFlake.NewLongId,
                            ParentId = sysMenu.Id,
                            RootId = sysMenu.RootId,
                            Text = funcAttribute.Text,
                            TextEN = funcAttribute.TextEN,
                            TextZH = funcAttribute.TextZH,
                            EntryType = funcAttribute.EntryType,
                            Ordinal = funcAttribute.Ordinal,
                            Assembly = Path.GetFileNameWithoutExtension(fileName),
                            Url = string.Join(".", viewType.FullName, methodInfo.Name),
                            Command = methodInfo.Name,
                            ParentText = sysMenu.Text,
                            RootText = parentMenu.Text,
                            CreateTime = DateTime.Now,
                            CreateUser = UserContext.UserId,
                            Remark = funcAttribute.Description,
                            
                        };

                        sysMenus.Add(funcMenu);
                    }
                }
            }
            foreach (SysMenu menu in sysMenus)
            {
                BaseRepository.Delete(s => s.Text == menu.Text);
            }
            return sysMenus;
        }

        private string GetViewName(string url)
        {
            if(string.IsNullOrEmpty(url)||!url.Contains('.')) return string.Empty;
            string[] strs=url.Split('.');
            return strs[strs.Length - 1];
        }

        public bool RemoveSysMenuAndChild(long id)
        {
            return SqlHelper.RetBool(BaseRepository.Delete(o => o.Id == id || o.ParentId == id));
        }

        // public override bool Save(List<SysMenu> entities)
        // {
        //     List<SysMenu> catalogs = entities.FindAll(e => e.EntryType == EntryType.Catalog);
        //     if (catalogs.Count > 0)
        //     {
        //         catalogs.ForEach(e =>
        //         {
        //             long snowId = baseRepository.InsertReturnSnowflakeId(e);
        //             e.Id = snowId;
        //             e.RootId = snowId;
        //             baseRepository.UpdateById(e);
        //         });
        //         List<SysMenu> menus = entities.FindAll(e => e.EntryType == EntryType.Menu);
        //         menus.ForEach(e =>
        //         {
        //             string parentText = e.ParentText;
        //             SysMenu parent = catalogs.Find(o => o.Text == parentText);
        //             e.ParentId = parent.Id;
        //             e.RootId = parent.Id;
        //             long snowId = baseRepository.InsertReturnSnowflakeId(e);
        //             e.Id = snowId;
        //         });
        //         List<SysMenu> buttons = entities.FindAll(e => e.EntryType == EntryType.Button);
        //         buttons.ForEach(e =>
        //         {
        //             string parentText = e.ParentText;
        //             string rootText = e.RootText;
        //             SysMenu parent = menus.Find(o => o.Text == parentText);
        //             SysMenu root = catalogs.Find(o => o.Text == rootText);
        //             e.ParentId = parent.Id;
        //             e.RootId = root.Id;
        //             long snowId = baseRepository.InsertReturnSnowflakeId(e);
        //             e.Id = snowId;
        //         });
        //         return true;
        //     }
        //
        //     return false;
        //
        //
        // }
    }
}