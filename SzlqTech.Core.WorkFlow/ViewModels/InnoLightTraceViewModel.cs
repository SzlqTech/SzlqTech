using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using NLog;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using SqlqTech.Core.Vo;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SzlqTech.Common.Exceptions;
using SzlqTech.Common.Extensions;
using SzlqTech.Common.Helper;
using SzlqTech.Common.Nlogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Enums;
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
        private readonly IDataCollectService dataCollectService;
        private InnoLightWorkflow workflow;
        private readonly IEventAggregator aggregator;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public InnoLightTraceViewModel(IHostDialogService dialog,IProductService productService,
            IMapper mapper,IMachineSettingService machineSettingService,IMachineDataCollectService machineDataCollectService,
            IDataCollectService dataCollectService
            )
        {
            Title = LocalizationService.GetString(AppLocalizations.DataCollection);
            this.dialog = dialog;
            this.productService = productService;
            this.mapper = mapper;
            this.machineSettingService = machineSettingService;
            this.machineDataCollectService = machineDataCollectService;
            this.dataCollectService = dataCollectService;
            workflow = ContainerLocator.Container.Resolve<InnoLightWorkflow>();
            workflow.PLCDataReceived -= OnMachineDataReceived;
            workflow.PLCDataReceived += OnMachineDataReceived;
            workflow.MachineStatusRecevied -= OnMachineStatusRecevied;
            workflow.MachineStatusRecevied += OnMachineStatusRecevied;
            aggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            aggregator.ResgiterMachineDataModel(OnMachineDataReceived, "InnoLightTraceViewModel");
            this.OETrayDataVos = new ObservableCollection<ExpandoObject>();
            this.VehicleReamingDataVos = new ObservableCollection<ExpandoObject>();
            this.ClearingDataVos = new ObservableCollection<ExpandoObject>();
            this.OEDispensingDataVos=new ObservableCollection<ExpandoObject>();
            this.HousingLoadingDataVos = new ObservableCollection<ExpandoObject>();
            this.GluingAssemblyDataVos= new ObservableCollection<ExpandoObject>();
            this.TightenScrewsDataVos = new ObservableCollection<ExpandoObject>();
            this.LiftingRefluxDataVos = new ObservableCollection<ExpandoObject>();
            this.DetectionDataVos = new ObservableCollection<ExpandoObject>();
            this.BakingTrayDataVos = new ObservableCollection<ExpandoObject>();
            IsOpen = false;
        }

        private void OnMachineStatusRecevied(object? sender, TEventArgs<bool> e)
        {
            IsOpen = e.Data;
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

        public Dictionary<string, List<PLCDataModel>> DicPLCDatas = new Dictionary<string, List<PLCDataModel>>();
        #endregion

        #region PLC读取数据端口键

        public const string FirstReadSignalKey = "FirstReadSignal";

        public const string SecondReadSignalKey = "SecondReadSignal";

        public const string ThirdReadSignalKey = "ThirdReadSignal";

        public const string FourthReadSignalKey = "FourthReadSignal";

        public const string FifthReadSignalKey = "FifthReadSignal";

        public const string SixthReadSignalKey = "SixthReadSignal";

        public const string SeventhReadSignalKey = "SeventhReadSignal";

        public const string EighthReadSignalKey = "EighthReadSignal";

        public  const string NinthReadSignalKey = "NinthReadSignal";

        public const string TenthReadSignalKey = "TenthReadSignal";

        public const string ProductSignalKey = "ProductKey";

        public const string FirstMachineName= "OETray";

        public const string SecondMachineName = "VehicleReaming";

        public const string ThirdMachineName = "Clearing";

        public const string FourthMachineName = "OEDispensing";

        public const string FifthMachineName = "housingLoading";

        public const string SixthMachineName = "GluingAssembly";

        public const string SeventhMachineName = "TightenScrews";

        public const string EighthMachineName = "LiftingReflux";

        public const string NinthMachineName = "Detection";

        public const string TenthMachineName = "BakingTray";
        #endregion

        #region 命令

        public Dictionary<string, MachineSetting> DicMachineSetting = new Dictionary<string, MachineSetting>();

        [ObservableProperty]
        
        public bool isOpen;

        public bool CanButtonExcute => !IsOpen;

        public bool CanClose => IsOpen;

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
                        await workflow.WriteValueAsync(ProductSignalKey,Convert.ToInt32(SelectedProductVo.ProductCode));

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
            if (!IsOpen) return;
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
        public async Task Loaded(object sender)
        {

            if (sender != null)
            {
                var control = sender as InnoLightTraceView;
                if (control != null)
                {
                    this.dataGrid = control.OETrayGrid;
                    this.vehicleReamingDataGrid = control.SecondGrid;
                    this.clearingDataGrid = control.thirddGrid;
                    this.oEDispensingdataGrid = control.fourthGrid;
                    this.housingLoadingDataGrid = control.fifthGrid;
                    this.gluingAssemblyDataGrid = control.sixthGrid;
                    this.tightenScrewsdataGrid = control.seventhGrid;
                    this.liftingRefluxDataGrid = control.eighthGrid;
                    this.detectionDataGrid = control.ninthGrid;
                    this.bakingTrayDataGrid = control.tenthGrid;
                }
            }
            await InitDataGrid();

        }
        #endregion

        #region DataGrid相关
        /// <summary>
        /// 加载datagrid列
        /// </summary>
        public async Task InitDataGrid()
        {
            DicPLCDatas.Clear();
            var groupedResults = (machineSettingService.List())
                                 .GroupBy(m => m.PortName)
                                 .ToList();
            DicMachineSetting.Clear();

            var tasks = groupedResults.Select(async group =>
            {
                try
                {
                    List<PLCDataModel> PLCDatas = new List<PLCDataModel>();
                    var portName = group.Key;
                    var machine = (await machineSettingService.ListAsync(o => o.PortName == portName)).FirstOrDefault();
                    if (machine != null)
                    {
                        DicMachineSetting.Add(portName, machine);
                        var dataCollections = await machineDataCollectService.ListAsync(o => o.MachineId == machine.Id);
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
                            PLCDatas.Add(new PLCDataModel()
                            {
                                Title = title,
                                PortKey = item.PortKey,
                                BindingName = item.BindingName,
                                MachineId = machine.Id,
                                MachineName = machine.PortName
                            });
                        }
                        DicPLCDatas.Add(portName, PLCDatas);
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorHandler($"加载datagrid数据错误，错误原因:{ex.Message}");
                }
            });

            //加载动画
            await SetBusyAsync(async () =>
            {
                await Task.WhenAll(tasks);
            });

            LoadDataGridColumms();
            await InitDataGridDatas();
        }

        //加载Datagrid数据列
        public void LoadDataGridColumms()
        {
            InitDataGridColumns(FirstMachineName, dataGrid);
            InitDataGridColumns(SecondMachineName, vehicleReamingDataGrid);
            InitDataGridColumns(ThirdMachineName, clearingDataGrid);
            InitDataGridColumns(FourthMachineName, oEDispensingdataGrid);
            InitDataGridColumns(FifthMachineName, housingLoadingDataGrid);
            InitDataGridColumns(SixthMachineName, gluingAssemblyDataGrid);
            InitDataGridColumns(SeventhMachineName, tightenScrewsdataGrid);
            InitDataGridColumns(EighthMachineName, liftingRefluxDataGrid);
            InitDataGridColumns(NinthMachineName, detectionDataGrid);
            InitDataGridColumns(TenthMachineName, bakingTrayDataGrid);
        }

        /// <summary>
        /// 初始化DataGrid列
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="grid"></param>
        public void InitDataGridColumns(string portName, DataGrid grid)
        {
            grid.Columns?.Clear();
            var plcDatas = GetPLCDatasByName(portName);
            foreach (var item in plcDatas)
            {
                var column = new DataGridTextColumn
                {
                    Header = item.Title,
                    Binding = new Binding(item.BindingName)
                };
                //按单元格内容自适应
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                grid?.Columns?.Add(column);
            }       
        }

        /// <summary>
        /// data读取
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name">机器名称</param>
        private void ReadData(MachineDataModel model, string name, ObservableCollection<ExpandoObject> datas)
        {
            
            List<PLCDataModel> PLCDatas = GetPLCDatasByName(name);
            if (model != null && model.MachineData.Data is bool value)
            {
                if (value)
                {
                  foreach (var item in PLCDatas)
                  {
                    if (item.PortKey == DataCollectEnum.SysDate.ToString()) continue;                
                    dynamic? data = workflow.ReadData(item.PortKey);
                    if (data != null) item.Value = data;
                       
                   }     
                    RreshDataGrid(name,datas);
                }
            }
        }

        /// <summary>
        /// 刷新从PLC读取的DataGrid数据
        /// </summary>
        /// <param name="name">机器名称</param>
        private void RreshDataGrid(string name, ObservableCollection<ExpandoObject> datas)
        {
            List<PLCDataModel> PLCDatas = GetPLCDatasByName(name);           
            dynamic obj = new ExpandoObject();
            
            foreach (var item in PLCDatas)
            {
                var itemDict = (IDictionary<string, object>)obj; // 转换为字典接口
                if (item.PortKey == DataCollectEnum.SysDate.ToString())
                {
                    itemDict[item.BindingName] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (item.Value != null)
                {
                    // 动态添加属性
                    itemDict[item.BindingName] = item.Value;
                }          
            }
            SaveDataGrid(PLCDatas, name);
            Application.Current.Dispatcher.Invoke(() =>
            {
                datas?.Add(obj);
            });

        }

        /// <summary>
        /// 存储数据到数据库
        /// </summary>
        private void SaveDataGrid(List<PLCDataModel> plcModels,string name)
        {
            //存储数据 测试
            List<DataCollectModel> model = new List<DataCollectModel>();
            DicMachineSetting.TryGetValue(name,out MachineSetting machine);
            //foreach (var item in plcModels)
            //{
            //    model.Add(new DataCollectModel() { Key = item.BindingName, Value = item.Value});
            //}

            Parallel.ForEach(plcModels, item =>
            {
                model.Add(new DataCollectModel() { Key = item.BindingName, Value = item.Value });
            });

            List<string> keys = model.Select(x => x.Key).ToList();
            List<string> values = model.Select(x => $"{x.Value}").ToList();

            DataCollectVo vo = new DataCollectVo()
            {
                Key = JsonConvert.SerializeObject(keys),
                Value = JsonConvert.SerializeObject(values),
                Id = SnowFlake.NewLongId,
                MachineId = machine?.Id,
                Name = machine?.PortName
            };
            var dataCollect = mapper.Map<DataCollect>(vo);
            dataCollectService.SaveOrUpdate(dataCollect);
        }

        /// <summary>
        /// 加载从数据库读取的数据
        /// </summary>
        private void LoadDataGrid(List<DataCollectVo> vos, ObservableCollection<ExpandoObject> datas)
        {
            foreach (var vo in vos)
            {
                List<string> keys = JsonConvert.DeserializeObject<List<string>>(vo.Key);
                List<string> values = JsonConvert.DeserializeObject<List<string>>(vo.Value);
                if (keys.Count != values.Count) return;
                List<DataCollectModel> model = new List<DataCollectModel>();
                dynamic obj = new ExpandoObject();
                int i = 0;
                foreach (var item in keys)
                {
                    try
                    {
                        var itemDict = (IDictionary<string, object>)obj; // 转换为字典接口
                        itemDict[item] = values[i];
                        i++;
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorHandler($"加载数据错误，原因:{ex.Message}"); ;
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    datas.Add(obj);
                });
            }
           
        }

        /// <summary>
        /// 获取PLC数据列表
        /// </summary>
        /// <param name="name">机器名称</param>
        /// <returns></returns>
        public List<PLCDataModel> GetPLCDatasByName(string name)
        {
            List<PLCDataModel> PLCDatas = new List<PLCDataModel>();
            if (DicPLCDatas.TryGetValue(name, out List<PLCDataModel>? plcDataList))
            {
                // 成功获取到值，plcDataList 就是对应的 List<PLCDataModel>
                PLCDatas = plcDataList;
            }
            else
            {
                logger.ErrorHandler($"Key:{name} 不存在！");
            }
            return PLCDatas;
        }
        #endregion

        #region dataGrid 定义

        /// <summary>
        /// OE Tray上料
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> oETrayDataVos;

        /// <summary>
        /// 载具换盘
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> vehicleReamingDataVos;

        /// <summary>
        /// 清洁+拔防尘塞
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> clearingDataVos;

        /// <summary>
        ///OE点胶
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> oEDispensingDataVos;

        /// <summary>
        ///housing 上料
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> housingLoadingDataVos;

        /// <summary>
        ///点胶合装
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> gluingAssemblyDataVos;

        /// <summary>
        /// 拧螺丝
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> tightenScrewsDataVos;

        /// <summary>
        ///升降回流
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> liftingRefluxDataVos;

        /// <summary>
        ///检测
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> detectionDataVos;

        /// <summary>
        ///烤盘上下料
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<ExpandoObject> bakingTrayDataVos;

        private DataGrid dataGrid;

        private DataGrid vehicleReamingDataGrid;

        private DataGrid clearingDataGrid;

        private DataGrid oEDispensingdataGrid;

        private DataGrid housingLoadingDataGrid;

        private DataGrid gluingAssemblyDataGrid;

        private DataGrid tightenScrewsdataGrid;

        private DataGrid liftingRefluxDataGrid;

        private DataGrid detectionDataGrid;

        private DataGrid bakingTrayDataGrid;

        #endregion

        #region IO

        private void OnMachineDataReceived(MachineDataModel model)
        {
            switch (model.MachineData.PortKey)
            {
                case FirstReadSignalKey: ReadData(model, FirstMachineName,OETrayDataVos); break;
                case SecondReadSignalKey: ReadData(model, SecondMachineName, VehicleReamingDataVos); break;
                case ThirdReadSignalKey: ReadData(model, ThirdMachineName, ClearingDataVos); break;
                case FourthReadSignalKey: ReadData(model, FourthMachineName, OEDispensingDataVos); break;
                case FifthReadSignalKey: ReadData(model, FifthMachineName, HousingLoadingDataVos); break;
                case SixthReadSignalKey: ReadData(model, SixthMachineName, GluingAssemblyDataVos); break;
                case SeventhReadSignalKey: ReadData(model, SeventhMachineName, TightenScrewsDataVos); break;
                case EighthReadSignalKey: ReadData(model, EighthMachineName, LiftingRefluxDataVos); break;
                case NinthReadSignalKey: ReadData(model, NinthMachineName, DetectionDataVos); break;
                case TenthReadSignalKey: ReadData(model, TenthMachineName, BakingTrayDataVos); break;
            }
        }

        #endregion

        #region 初始化数据

        /// <summary>
        /// 初始化PLC连接状态
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            List<MachineSetting> machineSettings = await machineSettingService.ListAsync(o => o.IsEnable);
            if (machineSettings == null || machineSettings.Count == 0) return;
            MachineLinks = new ObservableCollection<MachineLinkVo>();
            foreach (var item in machineSettings)
            {
                MachineLinkVo vo = new MachineLinkVo()
                {
                    Name = item.Description ?? string.Empty,
                    PortKey = item.PortKey,
                    IsLink = false
                };
                MachineLinks.Add(vo);
            }

          
        }

        private async Task InitDataGridDatas()
        {
            //从数据库加载当天的数据
            List<DataCollect> dataCollects =await dataCollectService.ListAsync(O => Convert.ToDateTime(O.CreateTime).Date == DateTime.Now.Date);
            var groups = dataCollects.GroupBy(x => x.Name).ToList(); ;
            foreach (var group in groups)
            {
                List<DataCollectVo> vos = mapper.Map<List<DataCollectVo>>(group);
                switch (group.Key) 
                {
                    case FirstMachineName:
                        LoadDataGrid(vos,OETrayDataVos);
                        break;
                    case SecondMachineName:
                        LoadDataGrid(vos, VehicleReamingDataVos);
                        break;
                    case ThirdMachineName:
                        LoadDataGrid(vos, ClearingDataVos);
                        break;
                    case FourthMachineName:
                        LoadDataGrid(vos, OEDispensingDataVos);
                        break;
                    case FifthMachineName:
                        LoadDataGrid(vos, HousingLoadingDataVos);
                        break;
                    case SixthMachineName:
                        LoadDataGrid(vos, GluingAssemblyDataVos);
                        break;
                    case SeventhMachineName:
                        LoadDataGrid(vos, TightenScrewsDataVos);
                        break;
                    case EighthMachineName:
                        LoadDataGrid(vos, LiftingRefluxDataVos);
                        break;
                    case NinthMachineName:
                        LoadDataGrid(vos, DetectionDataVos);
                        break;
                    case TenthMachineName:
                        LoadDataGrid(vos, BakingTrayDataVos);
                        break;
                       

                }

            }
        }



        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await Init();
            ProductVos = new ObservableCollection<ProductVo>();       
            List<Product> products = await productService.ListAsync();
            IsEnableMachine = bool.TryParse(XmlConfigHelper.GetValue("IsEnableMachine").ToLower(), out bool res);
            if (products != null && products.Count > 0)
            {
                List<ProductVo> productsVo = mapper.Map<List<ProductVo>>(products);
                ProductVos.AddRange(productsVo);
            }

        } 
        #endregion
    }

   
}
