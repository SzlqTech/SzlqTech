using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SqlqTech.Core.Vo
{
    public partial class ProductVo:BaseVo
    {
        [ObservableProperty]
        public string productName;

        [ObservableProperty]
        public string productCode;
    }
}
