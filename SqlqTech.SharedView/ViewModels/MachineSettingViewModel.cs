using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using SqlSugar;
using System.Collections.ObjectModel;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Nlogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class MachineSettingViewModel:NavigationViewModel
    {
        public IMachineSettingService SettingService { get; }
        public NavigationService NavigationService { get; set; }

        private readonly IMapper mapper;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MachineSettingViewModel(IMachineSettingService settingService,IMapper mapper, 
            NavigationService navigationService)
        {
            Title = LocalizationService.GetString(AppLocalizations.MachineManagement);    
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
        public async Task Save()
        {
            if (Valid())
            {
                await SetBusyAsync(async () =>
                {
                    try
                    {
                        foreach (var item in MachineSettingVos)
                        {
                            MachineModel model = default(MachineModel).GetValueByName(item.SelectedMachineType, true);
                            item.MachineModel = (short)model;
                            if (item.Id == 0)
                            {
                                item.Id = SnowFlake.NewLongId;
                            }
                        }
                        List<MachineSetting> list = mapper.Map<List<MachineSetting>>(MachineSettingVos);
                        if(await SettingService.SaveOrUpdateBatchAsync(list))
                         SendSuccessMsg();
                        else SendErrorMsg();
                    }
                    catch (Exception ex)
                    {
                        SendErrorMsg();
                        logger.ErrorHandler($"机器设置保存失败，失败原因:[{ex.Message}]");
                    }
                });           
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
        public void DataCollect(MachineSettingVo vo)
        {
            if (vo != null)
            {
                NavigationParameters para = new NavigationParameters();
                para.Add("Para", vo);
                NavigationService.Navigate(AppViews.MachineDataCollect, para);

            }
        }

        [RelayCommand]
        public void Delete(MachineSettingVo vo)
        {
            if (vo != null)
            {
                if (SettingService.Exist(o => o.Id == vo.Id))
                {
                    SettingService.Remove(o => o.Id == vo.Id);
                }
                MachineSettingVos.Remove(vo);
                SendDeleteSuccessMsg();
            }
        }

        public bool Valid()
        {
            if (MachineSettingVos==null|| MachineSettingVos.Count==0) return false;
            if (MachineSettingVos.Any(o => o.SelectedMachineType == null)) return false;         
            if (MachineSettingVos.Any(o=>string.IsNullOrEmpty(o.PortKey)||!o.PortKey.Contains("."))) return false;
            if(MachineSettingVos.GroupBy(o=>o.PortKey).Any(g=>g.Count()>1)) return false;
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
