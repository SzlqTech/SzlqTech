using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.Vos
{
    public partial class SysRoleMenuVo:BaseVo
    {
        [ObservableProperty]
        private long roleId;

        [ObservableProperty]
        private long menuId;

       
    }
}
