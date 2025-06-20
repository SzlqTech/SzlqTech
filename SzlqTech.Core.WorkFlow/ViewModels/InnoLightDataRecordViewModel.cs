using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using SzlqTech.Common.Exceptions;
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
        public  IDataPagerService dataPager { get; set; }

        public InnoLightDataRecordViewModel(IQrCodeService qrCodeService,IMapper mapper,IDataPagerService dataPagerService)
        {
            Title = LocalizationService.GetString(AppLocalizations.DataQuery);
            this.qrCodeService = qrCodeService;
            this.mapper = mapper;
            this.dataPager = dataPagerService;
            dataPager.OnPageIndexChangedEventhandler -= DataPagerService_OnPageIndexChangedEventhandler;
            dataPager.OnPageIndexChangedEventhandler += DataPagerService_OnPageIndexChangedEventhandler;
        }

        private async void DataPagerService_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            await SetBusyAsync(async () =>
            {
                await dataPager.GetListAsync(qrCodeService, new QrCodeVo());
            });
           
        }

        [RelayCommand]
        public async Task Search(string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                await SetBusyAsync(async () =>
                {
                    await dataPager.GetListAsync(qrCodeService, new QrCodeVo());
                });
                return;
            }
            var item = qrCodeService.GetFirstOrDefault(x=>x.Code==str);
            if (item == null)
            {
                dataPager.GridModelList.Clear();
                dataPager.Total = 0;
            }
            else
            {
                QrCodeVo vo=mapper.Map<QrCodeVo>(item);
                dataPager.GridModelList.Clear();
                dataPager.GridModelList.Add(vo);
                dataPager.Total = 1;
            }
           
        }

        public async Task InitData()
        {

            List<QrCode> qrCodes = new List<QrCode>();
            for (int i = 0; i < 100; i++)
            {
                QrCode code = new QrCode()
                {
                    ProductName= $"产品{i+1}",
                    SN= i+1,
                    Code = SnowFlake.NewId,
                    Id = SnowFlake.NewLongId,
                    Station = i,
                    LeaveDate = DateTime.Now.AddMinutes(i).ToString("yyyy-MM-dd HH:mm:ss"),
                    EnterDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                qrCodes.Add(code);
            }
            await qrCodeService.SaveBatchAsync(qrCodes);
        }


        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
           
            await SetBusyAsync(async () =>
            {
                //await InitData();
                await dataPager.GetListAsync(qrCodeService, new QrCodeVo());
            });
        }
    }
}
