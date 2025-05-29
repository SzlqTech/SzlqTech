using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class ScannerSettingViewModel: NavigationViewModel
    {
        private readonly IScannerSettingService scannerSettingService;
        private readonly IMapper mapper;

        public ScannerSettingViewModel(IScannerSettingService scannerSettingService,IMapper mapper)
        {
            Title = "扫描配置";
            ScannerSettingVos = new ObservableCollection<ScannerSettingVo>();
            this.scannerSettingService = scannerSettingService;
            this.mapper = mapper;
        }

        [ObservableProperty]
        public ObservableCollection<ScannerSettingVo> scannerSettingVos;

        [ObservableProperty]
        public ScannerSettingVo selectedScannerSettingVo;


        [RelayCommand]
        public void Add()
        {
            ScannerSettingVo vo = new ScannerSettingVo();
            ScannerSettingVos.Add(vo);
        }

        [RelayCommand]
        public async Task Save()
        {
            if (Valid())
            {
                foreach (var item in ScannerSettingVos)
                {
                    ScannerType type = default(ScannerType).GetValueByName(item.ScannerTypeName,true);
                    item.ScannerModel = type;
                    item.ScannerType=(short)type;
                    if (item.Id == 0)
                    {
                        item.Id = SnowFlakeNew.LongId;
                    }
                }
                List<ScannerSetting> scannerSettings = mapper.Map<List<ScannerSetting>>(ScannerSettingVos);
                await scannerSettingService.SaveOrUpdateBatchAsync(scannerSettings);
            }
        }

        public bool Valid()
        {
            if(ScannerSettingVos.Any(o=>string.IsNullOrEmpty(o.ScannerTypeName))) return false;
            if(ScannerSettingVos.Any(o=>string.IsNullOrEmpty(o.PortKey)||!o.PortKey.Contains("."))) return false;     
            return true;
        }

        [RelayCommand]
        public void Delete()
        {
            if (SelectedScannerSettingVo == null) return;
            if(scannerSettingService.Exist(o=>o.Id== SelectedScannerSettingVo.Id))
            {
                scannerSettingService.Remove(o=>o.Id==SelectedScannerSettingVo.Id);
            }
            ScannerSettingVos.Remove(SelectedScannerSettingVo);
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            ScannerSettingVos.Clear();
            List<ScannerSetting> scannerSettings =await scannerSettingService.ListAsync();
            if(scannerSettings==null|| scannerSettings.Count==0) return;
            List<ScannerSettingVo> vos = mapper.Map<List<ScannerSettingVo>>(scannerSettings);
            foreach (var item in vos)
            {
                ScannerType model = (ScannerType)item.ScannerType;
                item.ScannerTypeName = model.ToString();
            }
            ScannerSettingVos.AddRange(vos);
        }
    }
}
