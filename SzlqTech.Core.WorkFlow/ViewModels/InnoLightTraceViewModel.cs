using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MathNet.Numerics.Distributions;
using NPOI.SS.Formula.Functions;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using SqlqTech.Core.Vo;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Extensions;
using SzlqTech.Common.Helper;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.WorkFlow.Extensions;
using SzlqTech.Core.WorkFlow.Views;
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
        private readonly IMachineDataCollectService machineDataCollectService;
        private InnoLightWorkflow workflow;
        private readonly IEventAggregator aggregator;

        public InnoLightTraceViewModel(IHostDialogService dialog,IProductService productService,
            IMapper mapper,IMachineSettingService machineSettingService,IMachineDataCollectService machineDataCollectService)
        {
            Title = LocalizationService.GetString(AppLocalizations.DataCollection);
            this.dialog = dialog;
            this.productService = productService;
            this.mapper = mapper;
            this.machineSettingService = machineSettingService;
            this.machineDataCollectService = machineDataCollectService;
            workflow = ContainerLocator.Container.Resolve<InnoLightWorkflow>();
            workflow.PLCDataReceived -= OnMachineDataReceived;
            workflow.PLCDataReceived += OnMachineDataReceived;
            aggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            aggregator.ResgiterMachineDataModel(OnMachineDataReceived, "InnoLightTraceViewModel");
            this.OETrayDataVos = new ObservableCollection<ExpandoObject>();

        }

        private void OnMachineDataReceived(object? sender, TEventArgs<List<MachineLinkData>> e)
        {
            if(e.Data == null || e.Data.Count == 0) return;
            foreach (var item in e.Data)
            {
                var model= MachineLinks.FirstOrDefault(o => o.PortKey == item.PLCPortKey);
                if (model != null)
                {
                   model.IsLink = item.OperateResult.IsSuccess;
                }
            }
        }

        #region 属性
        [ObservableProperty]
        public string startContent = LocalizationService.GetString(AppLocalizations.Start);

        [ObservableProperty]
        public ObservableCollection<MachineLinkVo> machineLinks;

        [ObservableProperty]
        public ObservableCollection<ProductVo> productVos;

        [ObservableProperty]
        public ProductVo selectedProductVo;

        public bool IsEnableMachine = false; 

        public string CurrLanguage = XmlConfigHelper.GetValue("lang") ?? "zh-CN";
        #endregion

        #region PLC读取数据端口键

        public string FirstReadSignalKey = "FirstReadSignal";

        public string SecondReadSignalKey = "SecondReadSignal";

        public string ThirdReadSignalKey = "ThirdReadSignal";

        public string FourthReadSignalKey = "FourthReadSignal";

        public string FifthReadSignalKey = "FifthReadSignal";

        public string SixthReadSignalKey = "SixthReadSignal";

        public string SeventhReadSignalKey = "SeventhReadSignal";

        public string EighthReadSignalKey = "EighthReadSignal";

        public string NinthReadSignalKey = "NinthReadSignal";

        public string TenthReadSignalKey = "TenthReadSignal";

        public string ProductSignalKey = "ProductKey";

        #endregion

        #region 命令

        [RelayCommand]
        public async Task Start()
        {
            if (Valid())
            {
                var result = false;
                if (IsEnableMachine)
                {
                    await SetBusyAsync(async () =>
                    {
                        result = await workflow.WaitDataActionResultAsync(workflow.StartExecute);
                    });
                    if (result)
                    {
                        //启动成功发送产品号
                        SendMessage(LocalizationService.GetString(AppLocalizations.StartSuccess));
                        await workflow.WriteValueAsync(ProductSignalKey, SelectedProductVo.ProductCode);

                    }
                    else SendMessage(LocalizationService.GetString(AppLocalizations.StartError));
                }
                else
                {
                    SendMessage(LocalizationService.GetString(AppLocalizations.StartSuccess));
                }

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
            if (IsEnableMachine)
            {
                await SetBusyAsync(async () =>
                {
                    result = await workflow.WaitDataActionResultAsync(workflow.StopExecute);
                });
                if (result) SendMessage(LocalizationService.GetString(AppLocalizations.StopSuccess));
                else SendMessage(LocalizationService.GetString(AppLocalizations.StopError));
            }
            else
            {
                SendMessage(LocalizationService.GetString(AppLocalizations.StopSuccess));
            }

        }

        [RelayCommand]
        public void Add()
        {

            for (int i = 0; i < 20; i++)
            {
                dynamic item = new ExpandoObject();

                var itemDict = (IDictionary<string, object>)item; // 转换为字典接口

                // 动态添加属性
                itemDict["OKAmount"] = i.ToString();
                itemDict["NGAmount"] = (i + 2).ToString();
                itemDict["Code"] = SnowFlake.NewId;
               
                OETrayDataVos.Add(item);
            }

        }

        [RelayCommand]
        public void Loaded(object sender)
        {

            if (sender != null)
            {
                var control = sender as InnoLightTraceView;
                if (control != null)
                {
                    this.dataGrid = control.OETrayGrid;
                }
            }
            InitTrayLoadingGoods();

        }
        #endregion

        #region OE Tray上料

       

        [ObservableProperty]
        public ObservableCollection<ExpandoObject> oETrayDataVos;

        private DataGrid dataGrid;


      

        public void InitTrayLoadingGoods()
        {
            var machine = machineSettingService.GetFirstOrDefault(o=>o.PortName== "OETray");
            if (machine!=null)
            {
                var dataCollections = machineDataCollectService.List(o=>o.MachineId==machine.Id);
                if(OETrayDataVos==null|| OETrayDataVos.Count < 1)
                {
                    foreach (var item in dataCollections)
                    {
                        var title = string.Empty;
                        if (CurrLanguage == "zh-CN")
                        {
                            title = item.ZhHeaderTitle;
                        }
                        else if (CurrLanguage == "en-US")
                        {
                            title = item.EnHeaderTitle;
                        }
                        else if (CurrLanguage == "th-TH")
                        {
                            title = item.TaiHeaderTitle;
                        }
                        this.dataGrid.Columns.Add(new DataGridTextColumn() { Header = title, Binding = new Binding(item.BindingName) });
                    }

                } 

            }
        }

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
            MachineLinks = new ObservableCollection<MachineLinkVo>();
            foreach (var item in machineSettings)
            {
                MachineLinkVo vo= new MachineLinkVo()
                {
                    Name = item.Description??string.Empty,
                    PortKey= item.PortKey,
                    IsLink = false
                };
                MachineLinks.Add(vo);
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
       

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {      
            await Init();
            ProductVos =new ObservableCollection<ProductVo>();
            //OETrayDataVos = new ObservableCollection<ExpandoObject>();
            List<Product> products =await productService.ListAsync();
            IsEnableMachine =bool.TryParse(XmlConfigHelper.GetValue("IsEnableMachine").ToLower(),out bool res);
            if (products != null && products.Count > 0)
            {
                List<ProductVo> productsVo = mapper.Map<List<ProductVo>>(products);
                ProductVos.AddRange(productsVo);
            }
            
        }
    }

    public class PLCDataModel
    {
        public string PortKey { get; set; }

        public dynamic Value { get; set; }
    }
}
