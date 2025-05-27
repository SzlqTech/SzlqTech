using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Core.ViewModels;
using SzlqTech.Entity;
using SzlqTech.IService;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class MachineSettingViewModel:NavigationViewModel
    {
        public IMachineSettingService SettingService { get; }
        private readonly IMapper mapper;

        public MachineSettingViewModel(IMachineSettingService settingService,IMapper mapper)
        {
            SettingService = settingService;
            this.mapper = mapper;
        }

        [ObservableProperty]
        public ObservableCollection<MachineSettingVo> machineSettings;
        

        [RelayCommand]
        public void Add()
        {

        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            MachineSettings = new ObservableCollection<MachineSettingVo>();
            List<MachineSetting> settings = await SettingService.ListAsync();
            List<MachineSettingVo> list=mapper.Map<List<MachineSettingVo>>(settings);
            MachineSettings.AddRange(list);
        }


    }
}
