using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;

using System.Collections.ObjectModel;

using SzlqTech.Core.Consts;
using SzlqTech.Core.Models;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;

using SzlqTech.Services.Sessions;

namespace SzlqTech.ViewMdoels
{
    public partial class MainViewModel : NavigationViewModel, IConfigureService
    {
        private readonly IRegionManager regionManager;
        public NavigationService NavigationService { get; set; }

        public MainViewModel(IRegionManager regionManager, NavigationService navigationService)
        {
            this.regionManager = regionManager;
            NavigationService = navigationService;
        }

        [ObservableProperty]
        public ObservableCollection<NavigationItem> navigationItems;

      

        [RelayCommand]
        public void Navigate(NavigationItem item)
        {
            if (item == null) return;
            NavigationService.Navigate(item.PageViewName);
        }

        public void InitConfig()
        {
            NavigationItems = new ObservableCollection<NavigationItem>();
            NavigationItems.Add(new NavigationItem("dashboard", "系统设置", "", "", new ObservableCollection<NavigationItem>()
            {
                new NavigationItem("PLC", "机器配置", AppViews.MachineSetting, ""),           
            }));

        }

        public void Configure()
        {
            InitConfig();
        }
    }
}
