using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using System.Collections.ObjectModel;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;


namespace SzlqTech.Permission.ViewModels
{
    public partial class UserManagerViewModel: NavigationViewModel
    {
        private readonly IMapper mapper;
        private readonly ISysUserService sysUserService;
        public UserManagerViewModel(IMapper mapper,ISysUserService sysUserService)
        {
            Title = "用户管理";
            this.mapper = mapper;
            this.sysUserService = sysUserService;
        }


        [ObservableProperty]
        public ObservableCollection<SysUserVo> userVos;


        [RelayCommand]
        public void Add()
        {
            UserVos.Add(new SysUserVo() { });
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {

            UserVos = new ObservableCollection<SysUserVo>();
            List<SysUser> userList = await sysUserService.ListAsync();
            if (userList != null)
            {
                List<SysUserVo> sysUserVos = mapper.Map<List<SysUserVo>>(userList);
                UserVos.AddRange(sysUserVos);
            }

        }
    }
}
