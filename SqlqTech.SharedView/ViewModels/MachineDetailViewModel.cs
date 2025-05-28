using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Core.ViewModels;
using SzlqTech.IService;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class MachineDetailViewModel:NavigationViewModel
    {
        private readonly IMachineDetailService machineDetailService;
        public MachineDetailViewModel(IMachineDetailService machineDetailService)
        {
            Title = "机器列表详情";
            MachineDetailVos = new ObservableCollection<MachineDetailVo>();
            this.machineDetailService = machineDetailService;
        }

        [ObservableProperty]
        public ObservableCollection<MachineDetailVo> machineDetailVos;
       

        public MachineSettingVo CurrMachineSettingVo { get; set; }

        [RelayCommand]
        public void Add()
        {
            MachineDetailVo vo=new MachineDetailVo();
            MachineDetailVos.Add(vo);
        }


        [RelayCommand]
        public void Delete()
        {

        }

        [RelayCommand]
        public void Save()
        {
            if(MachineDetailVos==null|| MachineDetailVos.Count==0) return;
            foreach(var vo in MachineDetailVos)
            {
                vo.ScanCycleValue = ConverterScanCycle(vo.ScanCycle);
            }
            
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

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            
            MachineSettingVo para = navigationContext.Parameters.GetValue<MachineSettingVo>("Para");
            if (para != null)
            {
                CurrMachineSettingVo = para;
            }
            await Task.CompletedTask;
        }
    }
}
