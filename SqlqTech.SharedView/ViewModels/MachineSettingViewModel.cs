using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Core.Consts;
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
            Title = "机器列表";
            SettingService = settingService;
            this.mapper = mapper;
        }

        [ObservableProperty]
        public ObservableCollection<MachineSettingVo> machineSettingVos;
        

        [RelayCommand]
        public void Add()
        {
            MachineSettingVo vo=new MachineSettingVo();
            MachineSettingVos.Add(vo);
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            MachineSettingVos = new ObservableCollection<MachineSettingVo>();
            List<MachineSetting> settings = await SettingService.ListAsync();
            List<MachineSettingVo> list=mapper.Map<List<MachineSettingVo>>(settings);
            MachineSettingVos.AddRange(list);
        }


    }
}
