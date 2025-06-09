
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xaml.Behaviors.Media;
using SzlqTech.Entity;

namespace SzlqTech.Core.Vos
{
    public partial class SysUserVo : BaseVo
    {
        [ObservableProperty]
        public string userName;


        [ObservableProperty]
        public string passWord;

        [ObservableProperty]
        public string disPlayPassword;


        [ObservableProperty]
        public string confirmPassWord;


        [ObservableProperty]
        public bool isRemember;



        [ObservableProperty]
        public string code;

        [ObservableProperty]
        public string roleCode;

        [ObservableProperty]
        public string roleName;


        [ObservableProperty]
        public long roleId;

        [ObservableProperty]
        public SysRoleVo? selectedRoleVo;

        [ObservableProperty]
        public string selectedRoleCode;
    }
}
