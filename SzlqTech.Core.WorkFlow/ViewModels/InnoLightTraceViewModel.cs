using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.WorkFlow.Vos;
using SzlqTech.Localization;

namespace SzlqTech.Core.WorkFlow.ViewModels
{
    public partial class InnoLightTraceViewModel:NavigationViewModel
    {
       private readonly IHostDialogService dialog;

        public InnoLightTraceViewModel(IHostDialogService dialog)
        {
            Title = LocalizationService.GetString(AppLocalizations.InnoLight);
            this.dialog = dialog;
        }

        [ObservableProperty]
        public ObservableCollection<MachineLinkVo> machineLinks;


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


        public override Task OnNavigatedToAsync(NavigationContext navigationContext = null)
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
            Init();
            return Task.CompletedTask;
        }
    }
}
