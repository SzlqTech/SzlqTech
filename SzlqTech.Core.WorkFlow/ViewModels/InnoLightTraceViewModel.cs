using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Regions;
using SqlqTech.Core.Vo;
using System.Collections.ObjectModel;
using SzlqTech.Common.Views;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.WorkFlow.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;

namespace SzlqTech.Core.WorkFlow.ViewModels
{
   
    public partial class InnoLightTraceViewModel:NavigationViewModel
    {
       private readonly IHostDialogService dialog;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public InnoLightTraceViewModel(IHostDialogService dialog,IProductService productService,IMapper mapper)
        {
            Title = LocalizationService.GetString(AppLocalizations.DataCollection);
            this.dialog = dialog;
            this.productService = productService;
            this.mapper = mapper;
        }

        [ObservableProperty]
        public ObservableCollection<MachineLinkVo> machineLinks;

        [ObservableProperty]
        public ObservableCollection<ProductVo> productVos;

        [ObservableProperty]
        public ProductVo selectedProductVo;


        #region OE Tray上料

        [ObservableProperty]
        public ObservableCollection<TrayLoadingGoodsVo> trayLoadingGoods;

        #endregion

        public void Init()
        {
            TrayLoadingGoods=new ObservableCollection<TrayLoadingGoodsVo>();

            //模拟采集数据
           
        }

        public void GetData()
        {
            //var trayGoods =new Faker<TrayLoadingGoodsVo>()
            //                .RuleFor(o=>o.NewId,Guid.NewGuid);
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
            Init();
            
        }
    }
}
