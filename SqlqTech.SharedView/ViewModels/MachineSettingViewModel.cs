using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Masuit.Tools.Systems;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using SqlSugar;
using System.Collections.ObjectModel;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Entity;
using SzlqTech.IService;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class MachineSettingViewModel:NavigationViewModel
    {
        public IMachineSettingService SettingService { get; }
        public NavigationService NavigationService { get; set; }

        private readonly IMapper mapper;

        public MachineSettingViewModel(IMachineSettingService settingService,IMapper mapper, NavigationService navigationService)
        {
            Title = "机器列表";
            SettingService = settingService;
            this.mapper = mapper;
            NavigationService = navigationService;
        }

        [ObservableProperty]
        public ObservableCollection<MachineSettingVo> machineSettingVos;

        [ObservableProperty]
        public List<string> machineSettingNames;

        

        [RelayCommand]
        public void Add()
        {
            MachineSettingVo vo=new MachineSettingVo();
            MachineSettingVos.Add(vo);
        }

        [RelayCommand]
        public void Save()
        {
            if (Valid())
            {
                foreach (var item in MachineSettingVos)
                {
                    MachineModel model = default(MachineModel).GetValueByName(item.SelectedMachineType,true);
                    item.MachineModel = (short)model;
                }
                List<MachineSetting> list=mapper.Map<List<MachineSetting>>(MachineSettingVos);
                list.ForEach(o => o.Id = SnowFlakeNew.LongId);
                SettingService.SaveOrUpdateBatchAsync(list);
            }
        }

        [RelayCommand]
        public void Detail(MachineSettingVo vo)
        {
            if (vo != null)
            {
                NavigationParameters para = new NavigationParameters();
                para.Add("Para", vo);
                NavigationService.Navigate(AppViews.MachineDetail, para);
            }
        }

        [RelayCommand]
        public void Delete(MachineSettingVo vo)
        {

        }

        public bool Valid()
        {
            if (MachineSettingVos==null|| MachineSettingVos.Count==0) return false;
            if (MachineSettingVos.Any(o => o.SelectedMachineType == null)) return false;
            if (MachineSettingVos.Any(o=>string.IsNullOrEmpty(o.PortName)||!o.PortName.Contains("."))) return false;
            if (MachineSettingVos.Any(o => string.IsNullOrEmpty(o.Description))) return false;
            return true;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            MachineSettingVos = new ObservableCollection<MachineSettingVo>();
            List<MachineSetting> settings = await SettingService.ListAsync();
            List<MachineSettingVo> list=mapper.Map<List<MachineSettingVo>>(settings);
            foreach (var item in list)
            {
                MachineModel model = (MachineModel)item.MachineModel;
                item.SelectedMachineType = model.ToString();
            }
            MachineSettingVos.AddRange(list);
        }


    }
}
