using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Masuit.Tools;
using Masuit.Tools.Systems;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;
using SzlqTech.Core.ViewModels;
using SzlqTech.Entity;
using SzlqTech.IService;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class MachineDetailViewModel:NavigationViewModel
    {
        private readonly IMachineDetailService machineDetailService;
        private readonly IMapper mapper;

        public MachineDetailViewModel(IMachineDetailService machineDetailService,IMapper mapper)
        {
            Title = "机器列表详情";
            MachineDetailVos = new ObservableCollection<MachineDetailVo>();
            this.machineDetailService = machineDetailService;
            this.mapper = mapper;
        }

        [ObservableProperty]
        public ObservableCollection<MachineDetailVo> machineDetailVos;

        [ObservableProperty]
        public MachineDetailVo selectedMachineDetailVo;


        public MachineSettingVo CurrMachineSettingVo { get; set; }

        [RelayCommand]
        public void Add()
        {
            MachineDetailVo vo=new MachineDetailVo();
            MachineDetailVos.Add(vo);
        }


        [RelayCommand]
        public async Task Delete()
        {
            if(SelectedMachineDetailVo==null) return;
            if(machineDetailService.Exist(o=>o.Id== SelectedMachineDetailVo.Id))
            {
              await  machineDetailService.RemoveAsync(o=>o.Id==SelectedMachineDetailVo.Id);
            }
            MachineDetailVos.Remove(SelectedMachineDetailVo);
        }

        [RelayCommand]
        public async Task Save()
        {
            Valid();
            foreach (var item in MachineDetailVos)
            {
                if (item.Id == 0)
                {
                    item.Id = SnowFlakeNew.LongId;
                }
                if (item.MachineId == 0)
                {
                    item.MachineId = CurrMachineSettingVo.Id;
                }
                
                var data = default(DataType).GetValueByName(item.DataTypeName,true);
                item.DataType = (short)data;
            }
            List<MachineDetail> list = mapper.Map<List<MachineDetail>>(MachineDetailVos);     
           
            
            await machineDetailService.SaveOrUpdateBatchAsync(list);
        }

        public bool Valid()
        {
           
            if (MachineDetailVos == null || MachineDetailVos.Count == 0) return false;         
            foreach (var vo in MachineDetailVos)
            {
                vo.ScanCycleValue = ConverterScanCycle(vo.ScanCycle);
            }
           
            if (MachineDetailVos.Any(o=>string.IsNullOrEmpty(o.Address))) return false;
            if (MachineDetailVos.Any(o => string.IsNullOrEmpty(o.DataTypeName))) return false;      
            return true;
        }

        public int ConverterScanCycle(int index)
        {
            switch (index)
            {
                case 0:return 10;
                case 1: return 20;
                case 2: return 50;
                case 3: return 100;
                case 4: return 200;
                case 5: return 500;
            }
            return 10;
        }

        public int ConverterScanCycleIndex(int value)
        {
            switch (value)
            {
                case 10: return 0;
                case 20: return 1;
                case 50: return 2;
                case 100: return 3;
                case 200: return 4;
                case 500: return 5;
            }
            return 10;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            
            MachineSettingVo para = navigationContext.Parameters.GetValue<MachineSettingVo>("Para");
            if (para != null)
            {
                CurrMachineSettingVo = para;
                List<MachineDetail> details =await machineDetailService.ListAsync(o=>o.MachineId==CurrMachineSettingVo.Id);
                if (details != null)
                {
                    List<MachineDetailVo> vos = mapper.Map<List<MachineDetailVo>>(details);
                    vos.ForEach(o => o.ScanCycle = ConverterScanCycleIndex(o.ScanCycleValue));
                    MachineDetailVos.Clear();
                    MachineDetailVos.AddRange(vos);
                }
            }
            await Task.CompletedTask;
        }
    }
}
