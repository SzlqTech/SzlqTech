using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using System.Collections.ObjectModel;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Helper;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;


namespace SzlqTech.Permission.ViewModels
{
    public partial class UserManagerViewModel: NavigationViewModel
    {
        private readonly IMapper mapper;
        private readonly ISysUserService sysUserService;
        private readonly ISysRoleService sysRoleService;

        public UserManagerViewModel(IMapper mapper,ISysUserService sysUserService,ISysRoleService sysRoleService )
        {
            Title = LocalizationService.GetString(AppLocalizations.UserManager);
            this.mapper = mapper;
            this.sysUserService = sysUserService;
            this.sysRoleService = sysRoleService;
        }


        [ObservableProperty]
        public ObservableCollection<SysUserVo> userVos;

        [ObservableProperty]
        public SysUserVo? selectedUserVo;


        [RelayCommand]
        public void Add()
        {
            UserVos.Add(new SysUserVo() { });
        }

        [RelayCommand]
        public async Task Save()
        {
           if(Valid())
           {
                foreach (var item in UserVos)
                {
                    if (item.Id == 0)
                    {
                        item.Id = SnowFlake.NewLongId;
                        item.PassWord = HashHelper.CreateMD5(item.DisPlayPassword); 
                    }
                    item.RoleId = item.SelectedRoleVo.Id;
                }
                List<SysUser> sysUsers = mapper.Map<List<SysUser>>(UserVos);
                await sysUserService.SaveOrUpdateBatchAsync(sysUsers);
           }
           
        }

        [RelayCommand]
        public void Delete(SysUserVo sysUserVo)
        {
            if (sysUserVo == null) return;
            if (sysUserVo.Id > 0)
            {
                if (sysUserService.Exist(s => s.Id == sysUserVo.Id))
                {
                    sysUserService.Remove(s => s.Id == sysUserVo.Id);

                }
            }
            UserVos.Remove(sysUserVo);
        }

        public bool Valid()
        {
            if (UserVos == null || UserVos.Count == 0) return false;
            if (UserVos.Any(o => string.IsNullOrEmpty(o.UserName) || string.IsNullOrEmpty(o.Code))) return false;
            if(UserVos.GroupBy(o => o.Code).Any(g => g.Count() > 1)) return false; // 检查Code是否重复
            if(UserVos.Any(o=>o.SelectedRoleVo==null)) return false; // 检查是否选择了角色
            return true;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            UserVos = new ObservableCollection<SysUserVo>();          
            List<SysUser> userList = await sysUserService.ListAsync();        
            if (userList != null)
            {
                List<SysUserVo> sysUserVos = mapper.Map<List<SysUserVo>>(userList);
                foreach (var item in sysUserVos)
                {
                    if (item.RoleId > 0)
                    {
                        var role = sysRoleService.GetFirstOrDefault(s => s.Id == item.RoleId);
                        if (role != null)
                        {
                            item.SelectedRoleVo = mapper.Map<SysRoleVo>(role);
                            item.SelectedRoleCode=item.SelectedRoleVo.Code;
                        }                                  
                    }
                }
                UserVos.AddRange(sysUserVos);
            }         
        }
             
    }
}
