
using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.Vos
{
    public partial class SysUserVo : BaseVo
    {
        [ObservableProperty]
        private string userName;


        [ObservableProperty]
        private string passWord;


        [ObservableProperty]
        private string confirmPassWord;


        [ObservableProperty]
        private bool isRemember;



        [ObservableProperty]
        private string code;

       


    }
}
