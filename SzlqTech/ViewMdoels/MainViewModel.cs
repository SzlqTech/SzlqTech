using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Events;
using Prism.Regions;

using System.Collections.ObjectModel;

using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Core.Models;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Events;
using SzlqTech.Services.Sessions;

namespace SzlqTech.ViewMdoels
{
    public partial class MainViewModel : NavigationViewModel, IConfigureService
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;

        public NavigationService NavigationService { get; set; }

        public MainViewModel(IRegionManager regionManager, NavigationService navigationService,IEventAggregator aggregator)
        {
            this.regionManager = regionManager;
            NavigationService = navigationService;
            this.aggregator = aggregator;
            aggregator.ResgiterBusyAsyncMessage(arg =>
            {
                IsOpen = arg.IsOpen;
            }, "Main");
        }

        [ObservableProperty]
        public ObservableCollection<NavigationItem> navigationItems;

        [ObservableProperty]
        public bool isOpen;


        [RelayCommand]
        public void Navigate(NavigationItem item)
        {
            if (item == null) return;
            NavigationService.Navigate(item.PageViewName);
        }

        public void InitConfig()
        {
            NavigationItems = new ObservableCollection<NavigationItem>();
            NavigationItems.Add(new NavigationItem("dashboard", "配置管理", "", "", new ObservableCollection<NavigationItem>()
            {
                new NavigationItem("PLC", "机器配置", AppViews.MachineSetting, ""),
                new NavigationItem("scanner", "扫描配置", AppViews.ScannerSetting, ""),
                
            }));

        }

        public void Configure()
        {
            InitConfig();
        }
    }
}
