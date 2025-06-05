using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImTools;
using NLog;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Reflection.Metadata.Ecma335;
using SzlqTech.Common.Nlogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Core.ViewModels;
using SzlqTech.Localization;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class SysConfigViewModel:NavigationViewModel
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public SysConfigViewModel()
        {
            Title = LocalizationService.GetString(AppLocalizations.SysConfig); 
        }

        [ObservableProperty]
        public int langIndex;

        [ObservableProperty]
        public string currLangName;

        [ObservableProperty]
        public ObservableCollection<NavigationView> navViews;

        [ObservableProperty]
        public NavigationView selectedNavView;

        [RelayCommand]
        public void Save()
        {
            GetLangNameByIndex();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["lang"].Value = CurrLangName;
            config.AppSettings.Settings["View"].Value = SelectedNavView.Value;          
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            SendSuccessMsg();
            aggregator.SendUpdateLocalizationModel(true);
            logger.InfoHandler($"保存语言[{CurrLangName}]成功");


        }

        public void GetLangIndex(string name)
        {
            switch (name)
            {
                case "zh-CN": LangIndex = 0; break;
                case "en-US": LangIndex = 1; break;
                case "th-TH": LangIndex = 2; break;
            }
        }
        

        public void GetLangNameByIndex()
        {
            switch (LangIndex)
            {
                case 0: CurrLangName = "zh-CN"; break;
                case 1: CurrLangName = "en-US"; break;
                case 2: CurrLangName = "th-TH"; break;
            }
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            CurrLangName = ConfigurationManager.AppSettings["lang"]??"zh-CN";
            NavViews = new ObservableCollection<NavigationView>()
            {
                new NavigationView(){Value=AppViews.MachineSetting,Name=LocalizationService.GetString(AppLocalizations.MachineManagement)},
                new NavigationView(){Value=AppViews.ScannerSetting,Name=LocalizationService.GetString(AppLocalizations.ScanManagement)},
                new NavigationView(){Value=AppViews.SysConfig,Name=LocalizationService.GetString(AppLocalizations.SysConfig)},
                new NavigationView(){Value=AppViews.ProductView,Name=LocalizationService.GetString(AppLocalizations.ProductManagement)},
                new NavigationView(){Value=AppViews.InnoLight,Name=LocalizationService.GetString(AppLocalizations.DataCollection)},
                new NavigationView(){Value=AppViews.InnoLightDataRecordView,Name=LocalizationService.GetString(AppLocalizations.DataQuery)},
            };
            var viewName= ConfigurationManager.AppSettings["View"] ?? AppViews.InnoLight;
            var view = NavViews.FindFirst(s=>s.Value==viewName);
            SelectedNavView = view;
            GetLangIndex(CurrLangName);
            await Task.CompletedTask;
        }

    }

    public class NavigationView
    {
        public string Value { get; set; }

        public string Name { get; set; }
    }
}
