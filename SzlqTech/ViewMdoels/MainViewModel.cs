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
using SzlqTech.Services.Sessions;
using SzlqTech.Localization;
using System.Globalization;
using NLog;
using SzlqTech.Common.Nlogs;

namespace SzlqTech.ViewMdoels
{
    public partial class MainViewModel : NavigationViewModel, IConfigureService
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;

        public NavigationService NavigationService { get; set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MainViewModel(IRegionManager regionManager, NavigationService navigationService,IEventAggregator aggregator)
        {
            this.regionManager = regionManager;
            NavigationService = navigationService;
            this.aggregator = aggregator;
            aggregator.ResgiterBusyAsyncMessage(arg =>
            {
                IsOpen = arg.IsOpen;
            }, "Main");

            aggregator.ResgiterUpdateLocalizationModel(arg =>
            {
                if(arg.IsUpdate)
                {
                    InitConfig();
                }
            });
        }

        [ObservableProperty]
        public ObservableCollection<NavigationItem> navigationItems;

        [ObservableProperty]
        public bool isOpen;

        [ObservableProperty]
        public int langIndex;


        [RelayCommand]
        public void Navigate(NavigationItem item)
        {
            if (item == null) return;
            NavigationService.Navigate(item.PageViewName);
        }

        [RelayCommand]
        public void SelectionChanged()
        {
            if (LangIndex >= 0)
            {
                switch (LangIndex)
                {
                    case 0:
                    {
                        LocalizationService.CurrentCulture = new CultureInfo("zh-CN");
                        InitConfig();
                        break;
                    }
                        
                    case 1:
                    {
                        LocalizationService.CurrentCulture = new CultureInfo("en-US");
                        InitConfig();
                        break;
                    }
                       
                    case 2:
                    {
                        LocalizationService.CurrentCulture = new CultureInfo("th-TH");
                        InitConfig();
                        break;
                    }
                      
                }
            }
        }

        public void InitConfig()
        {
          
            NavigationItems = new ObservableCollection<NavigationItem>();
            NavigationItems.Add(new NavigationItem("dashboard", LocalizationService.GetString(AppLocalizations.ConfigManagement), "", "", new ObservableCollection<NavigationItem>()
            {
                new NavigationItem("PLC", LocalizationService.GetString(AppLocalizations.MachineManagement), AppViews.MachineSetting, ""),
                new NavigationItem("scanner", LocalizationService.GetString(AppLocalizations.ScanManagement), AppViews.ScannerSetting, ""),
                new NavigationItem("sysConfig", LocalizationService.GetString(AppLocalizations.SysConfig), AppViews.SysConfig, ""),

            }));
            NavigationItems.Add(new NavigationItem("workFlow", LocalizationService.GetString(AppLocalizations.WorkFlow), "", "", new ObservableCollection<NavigationItem>()
            {
                new NavigationItem("light",LocalizationService.GetString(AppLocalizations.InnoLight),AppViews.InnoLight,"")
            }));

        }

        public void Configure()
        {
            InitConfig();       
        }
    }
}
