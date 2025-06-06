using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using System.Collections.ObjectModel;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.Datapage;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SzlqTech.Core.WorkFlow.ViewModels
{
    public partial class InnoLightDataRecordViewModel: NavigationViewModel
    {
        private readonly IQrCodeService qrCodeService;
        private readonly IMapper mapper;
        private readonly IDataPagerService dataPagerService;

        public InnoLightDataRecordViewModel(IQrCodeService qrCodeService,IMapper mapper,IDataPagerService dataPagerService)
        {
            Title = LocalizationService.GetString(AppLocalizations.DataQuery);
            this.qrCodeService = qrCodeService;
            this.mapper = mapper;
            this.dataPagerService = dataPagerService;
            dataPagerService.OnPageIndexChangedEventhandler += DataPagerService_OnPageIndexChangedEventhandler;
        }

        private void DataPagerService_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
           
        }

        [ObservableProperty]
        public ObservableCollection<QrCodeVo> qrCodes;
      

        [RelayCommand]
        public void Search(string str)
        {
            var codes= QrCodes.ToList().FindAll(o=>o.Code==str);
            if (codes == null) return;
            QrCodes.Clear();
            QrCodes.AddRange(codes);
        }


        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            QrCodes = new ObservableCollection<QrCodeVo>();
            List<QrCode> qrCodes =await qrCodeService.ListAsync();
            if (qrCodes == null || qrCodes.Count == 0) return;
            List<QrCodeVo> vos=mapper.Map<List<QrCodeVo>>(qrCodes);
            QrCodes.AddRange(vos);
        }
    }
}
