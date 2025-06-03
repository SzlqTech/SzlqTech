using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Regions;
using System.Configuration;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Core.ViewModels;
using SzlqTech.Localization;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class SysConfigViewModel:NavigationViewModel
    {
        public SysConfigViewModel()
        {
            Title = LocalizationService.GetString(AppLocalizations.SysConfig); 
        }

        [ObservableProperty]
        public int langIndex;

        [ObservableProperty]
        public string currLangName;

        [RelayCommand]
        public void Save()
        {
            GetLangNameByIndex();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["lang"].Value = CurrLangName;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            SendSuccessMsg();
            aggregator.SendUpdateLocalizationModel(true);
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
            GetLangIndex(CurrLangName);
            await Task.CompletedTask;
        }

    }
}
