using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using Prism.Regions;
using SqlqTech.SharedView.Views;
using SqlqTech.SharedView.Vo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Nlogs;
using SzlqTech.Core.ViewModels;
using SzlqTech.Entity;
using SzlqTech.IService;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class MachineDataCollectViewModel: NavigationViewModel
    {
        private readonly IMapper mapper;
        private readonly IMachineDataCollectService machineDataCollectService;
        private readonly IMachineDetailService machineDetailService;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MachineDataCollectViewModel(IMapper mapper,IMachineDataCollectService machineDataCollectService,IMachineDetailService machineDetailService)
        {
            Title= "数据采集列表";
            this.mapper = mapper;
            this.machineDataCollectService = machineDataCollectService;
            this.machineDetailService = machineDetailService;
        }

        public MachineSettingVo CurrMachineSettingVo { get; set; }

        public DataGridComboBoxColumn cbox { get; set; }

        [ObservableProperty]
        public ObservableCollection<MachineCollectDataVo> machineCollectDataVos;

        [ObservableProperty]
        public MachineCollectDataVo selectedMachineDateVo;

        public List<MachineDetailVo> MachineDetailVos { get; set; } = new List<MachineDetailVo>();

        [RelayCommand]
        public void Add()
        {
            MachineCollectDataVo vo = new MachineCollectDataVo() { IsEnable=true,IsSysDate=false};
            MachineCollectDataVos.Add(vo);
        }

        [RelayCommand]
        public async Task Delete()
        {
            if (SelectedMachineDateVo == null) return;
            if (machineDataCollectService.Exist(o => o.Id == SelectedMachineDateVo.Id))
            {
                await machineDataCollectService.RemoveAsync(o => o.Id == SelectedMachineDateVo.Id);
            }
            MachineCollectDataVos.Remove(SelectedMachineDateVo);
            SendDeleteSuccessMsg();
        }

        [RelayCommand]
        public async Task Save()
        {
            if (!Valid()) return;
            await SetBusyAsync(async () =>
            {
                try
                {
                    Parallel.ForEach(MachineCollectDataVos, item =>
                    {
                        if (item.Id == 0)
                        {
                            item.Id = SnowFlake.NewLongId;
                        }
                        if (item.MachineId == 0)
                        {
                            item.MachineId = CurrMachineSettingVo.Id;
                        }
                    });                
                    List<MachineDataCollect> list = mapper.Map<List<MachineDataCollect>>(MachineCollectDataVos);         
                    await machineDataCollectService.SaveOrUpdateBatchAsync(list);
                    SendSuccessMsg();
                }
                catch (Exception ex)
                {
                    SendErrorMsg();
                    logger.ErrorHandler($"机器详情保存失败，失败原因:[{ex.Message}]");
                }
            });

        }

        [RelayCommand]
        public void Loaded(object sender)
        {
            if (sender != null)
            {
                var control = sender as MachineDataCollectView;
                if (control != null)
                {
                    this.cbox = control.cbox;
                    if(MachineDetailVos != null && MachineDetailVos.Count > 0)
                    {
                        cbox.ItemsSource = MachineDetailVos;             
                    }
                    else
                    {
                        cbox.ItemsSource = null;
                    }
                }
            }
        }

        public bool Valid()
        {

            if (MachineCollectDataVos == null || MachineCollectDataVos.Count == 0) return false;
            if (MachineCollectDataVos.Any(o => string.IsNullOrEmpty(o.ZhHeaderTitle) && string.IsNullOrEmpty(o.EnHeaderTitle)&& string.IsNullOrEmpty(o.TaiHeaderTitle))) return false;
            if (MachineCollectDataVos.Any(o => string.IsNullOrEmpty(o.BindingName))) return false;
            if (MachineCollectDataVos.Any(o => string.IsNullOrEmpty(o.PortKey))) return false;
            return true;
        }


        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            MachineCollectDataVos=new ObservableCollection<MachineCollectDataVo>();
            MachineSettingVo para = navigationContext.Parameters.GetValue<MachineSettingVo>("Para");
            if (para != null)
            {
                CurrMachineSettingVo = para;
                List<MachineDataCollect> details = await machineDataCollectService.ListAsync(o => o.MachineId == CurrMachineSettingVo.Id);
                List<MachineDetail> list = await machineDetailService.ListAsync(o => o.MachineId == CurrMachineSettingVo.Id);
                MachineDetailVos= mapper.Map<List<MachineDetailVo>>(list);
                if (cbox != null)
                {
                    cbox.ItemsSource = MachineDetailVos;
                }
                if (details != null)
                {
                    List<MachineCollectDataVo> vos = mapper.Map<List<MachineCollectDataVo>>(details);
                    MachineCollectDataVos.Clear();
                    MachineCollectDataVos.AddRange(vos);
                }
            }
            
        }
    }
}
