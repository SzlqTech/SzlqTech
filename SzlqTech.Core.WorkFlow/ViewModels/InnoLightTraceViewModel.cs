using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using SqlqTech.Core.Vo;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SzlqTech.Common.Extensions;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.WorkFlow.Extensions;
using SzlqTech.Core.WorkFlow.Vos;
using SzlqTech.Entity;
using SzlqTech.Equipment.Machine;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SzlqTech.Core.WorkFlow.ViewModels
{
   
    public partial class InnoLightTraceViewModel:NavigationViewModel
    {
        private readonly IHostDialogService dialog;
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IMachineSettingService machineSettingService;
        private InnoLightWorkflow workflow;
        private readonly IEventAggregator aggregator;

        public InnoLightTraceViewModel(IHostDialogService dialog,IProductService productService,IMapper mapper,IMachineSettingService machineSettingService)
        {
            Title = LocalizationService.GetString(AppLocalizations.DataCollection);
            this.dialog = dialog;
            this.productService = productService;
            this.mapper = mapper;
            this.machineSettingService = machineSettingService;
            workflow = ContainerLocator.Container.Resolve<InnoLightWorkflow>();
            workflow.PLCDataReceived -= OnMachineDataReceived;
            workflow.PLCDataReceived += OnMachineDataReceived;
            aggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            aggregator.ResgiterMachineDataModel(OnMachineDataReceived, "InnoLightTraceViewModel");
        }

        private void OnMachineDataReceived(object? sender, TEventArgs<PLCData> e)
        {
            
        }

        [ObservableProperty]
        public string startContent = LocalizationService.GetString(AppLocalizations.Start);

        [ObservableProperty]
        public ObservableCollection<MachineLinkVo> machineLinks;

        [ObservableProperty]
        public ObservableCollection<ProductVo> productVos;

        [ObservableProperty]
        public ProductVo selectedProductVo;


        [RelayCommand]
        public async Task Start()
        {
            if (Valid())
            {
                var result = false;
                await SetBusyAsync(async () =>
                {
                    result = await workflow.WaitDataActionResultAsync(workflow.StartExecute);
                });
                if (result) SendMessage(LocalizationService.GetString(AppLocalizations.StartSuccess));
                else SendMessage(LocalizationService.GetString(AppLocalizations.StartError));
            }         
        }

        public bool Valid()
        {
            if (SelectedProductVo == null)
            {
                SendMessage(LocalizationService.GetString(AppLocalizations.ProudctSelectedNull));
                return false;
            }
            return true;
        }

        [RelayCommand]
        public async Task Stop()
        {
            var result = false;
            await SetBusyAsync(async () =>
            {
                result = await workflow.WaitDataActionResultAsync(workflow.StopExecute);
            });
            if (result) SendMessage(LocalizationService.GetString(AppLocalizations.StopSuccess));
            else SendMessage(LocalizationService.GetString(AppLocalizations.StopError));
        }

        #region OE Tray上料

        [ObservableProperty]
        public ObservableCollection<TrayLoadingGoodsVo> trayLoadingGoods;

        #endregion


        #region IO

        private void OnMachineDataReceived(MachineDataModel model)
        {
            switch (model.MachineData.PortKey)
            {
                case "": break;
            }
        }

        #endregion


        public async Task Init()
        {
            List<MachineSetting> machineSettings =await machineSettingService.ListAsync(o=>o.IsEnable);
            if (machineSettings == null || machineSettings.Count == 0) return;
            foreach (var item in machineSettings)
            {

            }
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            MachineLinks = new ObservableCollection<MachineLinkVo>()
            {
                new MachineLinkVo(){Name="OE Tray上料",IsLink=false},
                new MachineLinkVo(){Name="载具换盘",IsLink=false},
                new MachineLinkVo(){Name="清洁+拔防尘塞",IsLink=false},
                new MachineLinkVo(){Name="OE点胶",IsLink=false},
                new MachineLinkVo(){Name="housing 上料",IsLink=true},
                new MachineLinkVo(){Name="点胶合装",IsLink=false},
                new MachineLinkVo(){Name="拧螺丝",IsLink=false},
                new MachineLinkVo(){Name="升降回流",IsLink=false},
                new MachineLinkVo(){Name="检测",IsLink=false},
                new MachineLinkVo(){Name="烤盘上下料",IsLink=false},             
            };
            ProductVos=new ObservableCollection<ProductVo>();
            List<Product> products =await productService.ListAsync();
            if(products != null && products.Count > 0)
            {
                List<ProductVo> productsVo = mapper.Map<List<ProductVo>>(products);
                ProductVos.AddRange(productsVo);
            }       
        }
    }
}
