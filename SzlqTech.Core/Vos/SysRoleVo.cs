using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.Vos
{
    public partial class SysRoleVo: BaseVo
    {
        [ObservableProperty]
        public string? name;
    }
}
