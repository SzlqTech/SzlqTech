using CommunityToolkit.Mvvm.ComponentModel;
using SzlqTech.Entity;

namespace SqlqTech.SharedView.Vo
{
    public partial class ProductVo:BaseVo
    {
        [ObservableProperty]
        public string productName;

        [ObservableProperty]
        public string productCode;
    }
}
