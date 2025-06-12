using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImTools;
using NLog;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Configuration;
using SzlqTech.Common.Nlogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Core.ViewModels;
using SzlqTech.Localization;
using SzlqTech.IService;
using SzlqTech.Entity;

namespace SqlqTech.SharedView.ViewModels
{
    public partial class SysConfigViewModel:NavigationViewModel
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ISysUserService sysUserService;
        private readonly ISysRoleMenuService sysRoleMenuService;
        private readonly ISysMenuService sysMenuService;

        public SysConfigViewModel(ISysUserService sysUserService,ISysRoleMenuService sysRoleMenuService,ISysMenuService sysMenuService)
        {
            Title = LocalizationService.GetString(AppLocalizations.SysConfig);
            this.sysUserService = sysUserService;
            this.sysRoleMenuService = sysRoleMenuService;
            this.sysMenuService = sysMenuService;
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
            SendMessage(LocalizationService.GetString(AppLocalizations.SuccessMsg) +","+ LocalizationService.GetString(AppLocalizations.PleaseRestorSystem));
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
            //NavViews = new ObservableCollection<NavigationView>()
            //{
            //    new NavigationView(){Value=AppViews.MachineSetting,Name=LocalizationService.GetString(AppLocalizations.MachineManagement)},
            //    new NavigationView(){Value=AppViews.ScannerSetting,Name=LocalizationService.GetString(AppLocalizations.ScanManagement)},
            //    new NavigationView(){Value=AppViews.SysConfig,Name=LocalizationService.GetString(AppLocalizations.SysConfig)},
            //    new NavigationView(){Value=AppViews.ProductView,Name=LocalizationService.GetString(AppLocalizations.ProductManagement)},
            //    new NavigationView(){Value=AppViews.InnoLight,Name=LocalizationService.GetString(AppLocalizations.DataCollection)},
            //    new NavigationView(){Value=AppViews.InnoLightDataRecordView,Name=LocalizationService.GetString(AppLocalizations.DataQuery)},
            //};
            await LoadViews();
            var viewName= ConfigurationManager.AppSettings["View"] ?? AppViews.InnoLight;
            var view = NavViews.FindFirst(s=>s.Value==viewName);
            SelectedNavView = view;
            GetLangIndex(CurrLangName);
            await Task.CompletedTask;
        }

        public async Task LoadViews()
        {
            NavViews = new ObservableCollection<NavigationView>();
            var user = sysUserService.GetFirstOrDefault(o=>o.Username== AppCurrContext.UserName);
            if (user == null) return;
            List<SysRoleMenu> sysRoleMenus =await sysRoleMenuService.ListAsync(o=>o.RoleId==user.RoleId);
            foreach (var sysRoleMenu in sysRoleMenus)
            {
                SysMenu menuu = sysMenuService.GetFirstOrDefault(s=>s.Id== sysRoleMenu.MenuId);
                if (menuu == null||menuu.EntryType==0) continue;
                var item = new NavigationView() { Value = menuu.View, Name = LocalizationService.GetString(menuu.Text) };
                NavViews.Add(item);
            }
        }

    }

    public class NavigationView
    {
        public string Value { get; set; }

        public string Name { get; set; }
    }
}
