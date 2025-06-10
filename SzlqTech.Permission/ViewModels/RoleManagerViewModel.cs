using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NPOI.SS.Formula.Functions;
using Prism.Regions;
using System.Collections.ObjectModel;
using SzlqTech.Common.Exceptions;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SzlqTech.Permission.ViewModels
{
    public partial class RoleManagerViewModel: NavigationViewModel
    {

        private readonly IMapper mapper; 
        private readonly ISysRoleService sysRoleService;
        public NavigationService NavigationService { get; }

        public RoleManagerViewModel(IMapper mapper, ISysRoleService sysRoleService, NavigationService navigationService)
        {
            Title=  LocalizationService.GetString(AppLocalizations.RoleManager);
            this.mapper = mapper;    
            this.sysRoleService = sysRoleService;
            NavigationService = navigationService;
        }

        [ObservableProperty]
        public ObservableCollection<SysRoleVo> roleVos;

       

        [RelayCommand]
        public void Add()
        {
            RoleVos.Add(new SysRoleVo() { });
        }

        [RelayCommand]
        public async Task Save()
        {
            if (Valid())
            {
                foreach (var item in RoleVos)
                {
                    if (item.Id == 0)
                    {
                        item.Id=SnowFlake.NewLongId;
                    }
                }
                List<SysRole> sysRoles = mapper.Map<List<SysRole>>(RoleVos);
                await sysRoleService.SaveOrUpdateBatchAsync(sysRoles);
                SendMessage("角色保存成功");
            }

        }

        [RelayCommand]
        public void Delete(SysRoleVo sysRoleVo)
        {
            if (sysRoleVo == null) return;
            if (sysRoleVo.Id > 0)
            {
                if (sysRoleService.Exist(s => s.Id == sysRoleVo.Id))
                {
                    sysRoleService.Remove(s => s.Id == sysRoleVo.Id);
                }
            }
            RoleVos.Remove(sysRoleVo);
        }

        [RelayCommand]
        public void MenuAssign(SysRoleVo sysRoleVo)
        {
            NavigationParameters para=new NavigationParameters();
            para.Add(AppSharedConsts.Parameter, sysRoleVo);
            NavigationService.Navigate(AppViews.MenuAssignView,para);
        }

        public bool Valid()
        {
            if (RoleVos == null || RoleVos.Count == 0) return false;
            if (RoleVos.Any(o => string.IsNullOrEmpty(o.Name) || string.IsNullOrEmpty(o.Code))) return false;
            if (RoleVos.GroupBy(o => o.Code).Any(g => g.Count() > 1)) return false; // 检查Code是否重复
            return true;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            RoleVos = new ObservableCollection<SysRoleVo>();        
            var roles = await sysRoleService.ListAsync();
           
            if (roles != null)
            {
                List<SysRoleVo> sysRoles = mapper.Map<List<SysRoleVo>>(roles);
                RoleVos.AddRange(sysRoles);
            }
        }
    }
}
