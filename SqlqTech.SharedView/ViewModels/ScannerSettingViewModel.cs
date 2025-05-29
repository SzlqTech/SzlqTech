using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlqTech.SharedView.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Core.ViewModels;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class ScannerSettingViewModel: NavigationViewModel
    {
        public ScannerSettingViewModel()
        {
            Title = "扫描配置";
        }

        [ObservableProperty]
        public ObservableCollection<ScannerSettingVo> scannerSettingVos;

        [RelayCommand]
        public void Add()
        {

        }

        [RelayCommand]
        public void Delete()
        {

        }
    }
}
