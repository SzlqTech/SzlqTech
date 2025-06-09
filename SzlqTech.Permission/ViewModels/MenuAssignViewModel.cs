using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImTools;
using Masuit.Tools;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.ObjectModel;
using SzlqTech.Common.Exceptions;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SzlqTech.Permission.ViewModels
{
    public partial class MenuAssignViewModel:NavigationViewModel
    {
        private readonly IContainerProvider provider;
        private readonly ISysRoleService sysRoleService;
        private readonly IMapper mapper;
        private readonly ISysMenuService sysMenuService;
        private readonly ISysRoleMenuService sysRoleMenuService;

        public MenuAssignViewModel(IContainerProvider provider, ISysRoleService sysRoleService,
            IMapper mapper, ISysMenuService sysMenuService, ISysRoleMenuService sysRoleMenuService) 
        {
            Title = LocalizationService.GetString(AppLocalizations.MenuAssign);
            this.provider = provider;
            this.sysRoleService = sysRoleService;
            this.mapper = mapper;
            this.sysMenuService = sysMenuService;
            this.sysRoleMenuService = sysRoleMenuService;
            DataLoad();
            IsSelectedAllCommand = new DelegateCommand<MenuNodesVo>(IsSelectedAll);
            IsSelectedCommand = new DelegateCommand<MenuNodeItem>(IsSelected);
           
        }

        #region func


        [RelayCommand]
        private void Delete()
        {
            foreach (var menu in MenuNodes)
            {
                //全选
                if (menu.IsSelectedAll)
                {
                    //删除主菜单
                    sysMenuService.Remove(s => s.Text == menu.Title);
                    //删除子菜单
                    foreach (var menuItem in menu.MenuNodeItems)
                    {
                        sysMenuService.Remove(s => s.Text == menuItem.Text);
                    }
                }
                //没有全选 删除已经勾选的子菜单
                else
                {
                    foreach (var menuItem in menu.MenuNodeItems)
                    {
                        if (menuItem.IsSelected)
                        {
                            sysMenuService.Remove(s => s.Text == menuItem.Text);
                        }
                    }
                }
            }

            DataLoad();
            SendMessage("菜单删除成功");
        }

        [RelayCommand]

        private void Save()
        {
            List<SysRoleMenuVo> list = new List<SysRoleMenuVo>();
            SysRole sysRole = sysRoleService.GetByCode(CurrSysRole.Code);
            if (sysRole == null)
            {
                SendMessage("未查询到当前用户");
                return;
            }
            foreach (var menu in MenuNodes)
            {
                if (menu.IsSelected)
                {
                    SysMenu rootMenu = sysMenuService.GetFirstOrDefault(s => s.Text == menu.Title);
                    if (rootMenu == null)
                    {
                        break;
                    }
                    SysRoleMenuVo sysRoleMenuVo = new SysRoleMenuVo()
                    {

                        Id = SnowFlake.NewLongId,
                        RoleId = sysRole.Id,
                        MenuId = rootMenu.Id,
                        Status = 1,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                    };
                    list.Add(sysRoleMenuVo);
                }
                foreach (var menuItem in menu.MenuNodeItems)
                {
                    if (menuItem.IsSelected)
                    {
                        SysRoleMenuVo sysRoleMenuVo = new SysRoleMenuVo()
                        {
                            Id = SnowFlake.NewLongId,
                            RoleId = sysRole.Id,
                            MenuId = menuItem.Id,
                            Status = 1,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                        };
                        list.Add(sysRoleMenuVo);
                    }
                }
            }
            if (list.Count > 0)
            {
                List<SysRoleMenu> roleMenus = mapper.Map<List<SysRoleMenu>>(list);
                if (roleMenus != null)
                {
                    foreach (var roleMenu in roleMenus)
                    {
                        //删除重复的roleId绑定数据
                        sysRoleMenuService.Remove(s => s.RoleId == roleMenu.RoleId);
                    }
                    sysRoleMenuService.Save(roleMenus);
                }
            }
            SendMessage("菜单权限分配成功");
        }

        private void New()
        {
            DataLoad();
        }

        private void IsSelected(MenuNodeItem vo)
        {
            var menuNode = MenuNodes.FindFirst(s => s.Title == vo.ParentName);
            if (menuNode == null) return;
            int count = menuNode.MenuNodeItems.Count(s => s.IsSelected == true);
            int sum = menuNode.MenuNodeItems.Count;
            if (count > 0)
            {
                //item元素至少有一个被选中
                MenuNodes.FindFirst(s => s.Title == vo.ParentName).IsSelected = true;
            }
            else
            {
                //item元素没有一个被选中
                MenuNodes.FindFirst(s => s.Title == vo.ParentName).IsSelected = false;
            }
            //item元素全部被选中
            if (count == sum)
            {
                MenuNodes.FindFirst(s => s.Title == vo.ParentName).IsSelectedAll = true;
            }
            else
            {
                MenuNodes.FindFirst(s => s.Title == vo.ParentName).IsSelectedAll = false;
            }

        }

        private void IsSelectedAll(MenuNodesVo obj)
        {
            if (obj != null && MenuNodes != null)
            {
                MenuNodes.FindFirst(s => s.Title == obj.Title).MenuNodeItems.ForEach(s => s.IsSelected = obj.IsSelectedAll);
                MenuNodes.FindFirst(s => s.Title == obj.Title).IsSelected = obj.IsSelectedAll;
                MenuNodes.FindFirst(s => s.Title == obj.Title).IsSelectedAll = obj.IsSelectedAll;
            }
        }

        public void DataLoad()
        {
            MenuNodes = new ObservableCollection<MenuNodesVo>();
            //数据库加载
            List<SysMenu> parentList = sysMenuService.List(s => s.EntryType == 0);
            if (parentList == null) return;
            foreach (var item in parentList)
            {
                MenuNodesVo treeNodes = new MenuNodesVo(item.Text);
                List<SysMenu> menus = sysMenuService.List(s => s.ParentId == item.Id);
                if (menus == null) return;
                List<MenuNodeItem> list = new List<MenuNodeItem>();
                foreach (var menu in menus)
                {
                    MenuNodeItem treeNodeItem = new MenuNodeItem(menu.Text, menu.View, item.Text, menu.Icon, menu.Id);
                    list.Add(treeNodeItem);
                }
                MenuNodesVo node = new MenuNodesVo(item.Text, list.ToArray());
                MenuNodes.Add(node);
            }

        }
        #endregion

        #region  command
        public DelegateCommand<MenuNodesVo> IsSelectedAllCommand { get; set; }

        public DelegateCommand<MenuNodeItem> IsSelectedCommand { get; set; }

        public DelegateCommand<string> ExcuteCommand { get; set; }
        #endregion

        #region prop
        [ObservableProperty]
        private ObservableCollection<MenuNodesVo> menuNodes;

        [ObservableProperty]
        public SysRoleVo currSysRole;

        #endregion

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {

            if (navigationContext.Parameters.ContainsKey(AppSharedConsts.Parameter))
            {
                CurrSysRole = navigationContext.Parameters.GetValue<SysRoleVo>(AppSharedConsts.Parameter);
            }
                await Task.CompletedTask;
        }

    }
}
