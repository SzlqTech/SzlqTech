using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Events;
using Prism.Regions;
using System.Collections.ObjectModel;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Core.Models;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Services.Sessions;
using SzlqTech.Localization;
using System.Globalization;
using NLog;
using ImTools;
using System.Configuration;
using SzlqTech.IService;
using SzlqTech.Entity;

namespace SzlqTech.ViewMdoels
{
    public partial class MainViewModel : NavigationViewModel, IConfigureService
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;
        private readonly ISysMenuService sysMenuService;
        private readonly ISysUserService sysUserService;
        private readonly ISysUserDetailService sysUserDetailService;
        private readonly ISysRoleService sysRoleService;
        private readonly ISysRoleMenuService sysRoleMenuService;

        public NavigationService NavigationService { get; set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MainViewModel(IRegionManager regionManager, NavigationService navigationService,IEventAggregator aggregator,
            ISysMenuService sysMenuService,
            ISysUserService sysUserService, ISysUserDetailService sysUserDetailService,
            ISysRoleService sysRoleService, ISysRoleMenuService sysRoleMenuService)
        {
            this.regionManager = regionManager;
            NavigationService = navigationService;
            this.aggregator = aggregator;
            this.sysMenuService = sysMenuService;
            this.sysUserService = sysUserService;
            this.sysUserDetailService = sysUserDetailService;
            this.sysRoleService = sysRoleService;
            this.sysRoleMenuService = sysRoleMenuService;
            aggregator.ResgiterBusyAsyncMessage(arg =>
            {
                IsOpen = arg.IsOpen;
            }, "Main");

            aggregator.ResgiterUpdateLocalizationModel(arg =>
            {
                if(arg.IsUpdate)
                {
                    InitConfig();
                }
            });
        }

        [ObservableProperty]
        public ObservableCollection<NavigationItem> navigationItems;

        [ObservableProperty]
        public bool isOpen;

        [ObservableProperty]
        public int langIndex;


        [RelayCommand]
        public void Navigate(NavigationItem item)
        {
            if (item == null) return;
            NavigationService.Navigate(item.PageViewName);
        }

        [RelayCommand]
        public void SelectionChanged()
        {
            if (LangIndex >= 0)
            {
                switch (LangIndex)
                {
                    case 0:
                    {
                        LocalizationService.CurrentCulture = new CultureInfo("zh-CN");
                        InitConfig();
                        break;
                    }
                        
                    case 1:
                    {
                        LocalizationService.CurrentCulture = new CultureInfo("en-US");
                        InitConfig();
                        break;
                    }
                       
                    case 2:
                    {
                        LocalizationService.CurrentCulture = new CultureInfo("th-TH");
                        InitConfig();
                        break;
                    }
                      
                }
            }
        }

        public void InitConfig()
        {
          

            NavigationItems = new ObservableCollection<NavigationItem>();
            if (AppCurrContext.UserName.ToLower() == "sa")
            {
                NavigationItems.Add(new NavigationItem("dashboard", LocalizationService.GetString(AppLocalizations.ConfigManagement), "", "", new ObservableCollection<NavigationItem>()
                {
                    new NavigationItem("user", LocalizationService.GetString(AppLocalizations.UserManager), AppViews.UserManagerView, ""),
                    new NavigationItem("role", LocalizationService.GetString(AppLocalizations.RoleManager), AppViews.RoleManagerView, ""),
                    new NavigationItem("menu", LocalizationService.GetString(AppLocalizations.MenuImport), AppViews.MenuImportView, ""),

                }));
            }
            else
            {
                //NavigationItems.Add(new NavigationItem("workFlow", LocalizationService.GetString(AppLocalizations.WorkFlow), "", "", new ObservableCollection<NavigationItem>()
                //{
                //    new NavigationItem("dataCollection",LocalizationService.GetString(AppLocalizations.DataCollection),AppViews.InnoLight,""),
                //    new NavigationItem("dataQuery",LocalizationService.GetString(AppLocalizations.DataQuery),AppViews.InnoLightDataRecordView,"")

                //}));
                //NavigationItems.Add(new NavigationItem("dashboard", LocalizationService.GetString(AppLocalizations.ConfigManagement), "", "", new ObservableCollection<NavigationItem>()
                //{
                //    new NavigationItem("PLC", LocalizationService.GetString(AppLocalizations.MachineManagement), AppViews.MachineSetting, ""),
                //    new NavigationItem("scanner", LocalizationService.GetString(AppLocalizations.ScanManagement), AppViews.ScannerSetting, ""),
                //    new NavigationItem("sysConfig", LocalizationService.GetString(AppLocalizations.SysConfig), AppViews.SysConfig, ""),
                //    new NavigationItem("product", LocalizationService.GetString(AppLocalizations.ProductManagement), AppViews.ProductView, ""),

                //}));

                //动态配置页面
                LoadNavigationItems();
            }

        }

       

        public void LoadNavigationItems()
        {
            //根据用户名查询角色管理权限
            var user = sysUserService.GetFirstOrDefault(s=>s.Username==AppCurrContext.UserName);
            if (user == null) return;     
          
            List<SysRoleMenu> list = sysRoleMenuService.List(s => s.RoleId == user.RoleId);
            //根据Role-Menu获取Menu的菜单
            List<SysMenu> menuList = new List<SysMenu>();
            foreach (var item in list)
            {
                SysMenu menu = sysMenuService.GetById(item.MenuId);
                if (menu != null)
                {
                    menuList.Add(menu);
                }
            }

            List<SysMenu> parentMenu = menuList.FindAll(s => s.EntryType == 0);
            foreach (var menu in parentMenu)
            {
                //主节点
                //根据主节点查询子节点
                List<SysMenu> childrenMenus = menuList.FindAll(s => s.EntryType != 0 && s.ParentId == menu.Id);
                if (childrenMenus == null || childrenMenus.Count == 0) return;
                NavigationItem treeNodes = new NavigationItem(menu.Text);
                List<NavigationItem> treeNodeList = new List<NavigationItem>();
                foreach (var child in childrenMenus)
                {
                    NavigationItem nodeItem = new NavigationItem(child.Icon, LocalizationService.GetString(child.Text), child.View, "");
                    treeNodeList.Add(nodeItem);
                }

                ObservableCollection<NavigationItem> navigationItems = new ObservableCollection<NavigationItem>();
                navigationItems.AddRange(treeNodeList);
                NavigationItems.Add(new NavigationItem(menu.ParentIcon, LocalizationService.GetString(menu.Text), "", "", navigationItems));
            }
        }

        public void Configure()
        {
            InitConfig();
            var viewName = ConfigurationManager.AppSettings["View"] ;
            if (viewName == null) return;
            for (int i = 0;i< NavigationItems.Count;i++)
            {
                var items = NavigationItems[i].Items.FindFirst(s => s.PageViewName == viewName);
                if (items != null)
                {
                    NavigationItems[i].IsExpanded = true;
                    items.IsSelected = true;
                    Navigate(items);
                }
            }
          
        }
    }
}
