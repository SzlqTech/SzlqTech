using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SzlqTech.Core.WorkFlow.Vos
{
    public partial class MachineLinkVo:BaseVo
    {
        [ObservableProperty]
        public bool isLink;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string? address;

        [ObservableProperty]
        public string? portKey;
    }
}
