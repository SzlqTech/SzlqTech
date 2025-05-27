using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Models;
using SzlqTech.Core.ViewModels;
using SzlqTech.Services.Sessions;

namespace SzlqTech.ViewMdoels
{
    public partial class MainViewModel : NavigationViewModel, IConfigureService
    {
        private readonly IRegionManager regionManager;

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        [ObservableProperty]
        public ObservableCollection<NavigationItem> navigationItems;

        [RelayCommand]
        public void Navigate()
        {

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
