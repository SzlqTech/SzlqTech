
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Core.ViewModels;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class MachineDetailViewModel:NavigationViewModel
    {
        public MachineDetailViewModel()
        {
            Title = "机器列表详情";
        }

        [ObservableProperty]
        public ObservableCollection<MachineDetailVo> machineDetailVos;

        public MachineSettingVo CurrMachineSettingVo { get; set; }


        [RelayCommand]
        public void Delete()
        {

        }

        [RelayCommand]
        public void Save()
        {

        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            MachineDetailVos = new ObservableCollection<MachineDetailVo>();
            MachineSettingVo para = navigationContext.Parameters.GetValue<MachineSettingVo>("Para");
            if (para != null)
            {
                CurrMachineSettingVo = para;
            }
            await Task.CompletedTask;
        }
    }
}
