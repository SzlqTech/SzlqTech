using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using Prism.Regions;
using SqlqTech.SharedView.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Nlogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class ScannerSettingViewModel: NavigationViewModel
    {
        private readonly IScannerSettingService scannerSettingService;
        private readonly IMapper mapper;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ScannerSettingViewModel(IScannerSettingService scannerSettingService,IMapper mapper)
        {
            Title = LocalizationService.GetString(AppLocalizations.ScanManagement); ;     
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
                await SetBusyAsync(async () =>
                {
                    try
                    {
                        foreach (var item in ScannerSettingVos)
                        {
                            ScannerType type = default(ScannerType).GetValueByName(item.ScannerTypeName, true);
                            item.ScannerModel = type;
                            item.ScannerType = (short)type;
                            if (item.Id == 0)
                            {
                                item.Id = SnowFlake.NewLongId;
                            }
                        }
                        List<ScannerSetting> scannerSettings = mapper.Map<List<ScannerSetting>>(ScannerSettingVos);
                        await scannerSettingService.SaveOrUpdateBatchAsync(scannerSettings);
                        SendSuccessMsg();
                    }
                    catch (Exception ex)
                    {
                        SendErrorMsg();
                        logger.ErrorHandler($"保存扫描设备失败,失败原因:[{ex.Message}]");
                    }
                    finally
                    {
                       
                    }
                });             
            }
        }

        public bool Valid()
        {
            if(ScannerSettingVos.Any(o=>string.IsNullOrEmpty(o.ScannerTypeName))) return false;
            if(ScannerSettingVos.Any(o=>string.IsNullOrEmpty(o.PortKey)||!o.PortKey.Contains("."))) return false;
            if(ScannerSettingVos.GroupBy(o=>o.PortKey).Any(g=>g.Count()>1)) return false;
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
            SendDeleteSuccessMsg();
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
