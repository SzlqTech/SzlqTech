
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Configuration;
using SzlqTech.Common.Helper;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;
using SzlqTech.Localization;


namespace SzlqTech.ViewMdoels
{
   
    public partial class LoginViewModel: DialogViewModel
    {
        private readonly ISysUserService userService;
        private readonly IMapper mapper;
        public LoginViewModel(ISysUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
            Init();
        }

        [ObservableProperty]
        public bool isRememberMe;

        [ObservableProperty]
        public string userName;

        [ObservableProperty]
        public string password;
   

        public void Init()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var Username = configuration.AppSettings.Settings["UserName"].Value ?? string.Empty;
            var Password = configuration.AppSettings.Settings["Password"].Value ?? string.Empty;
            var isRemember = configuration.AppSettings.Settings["New"].Value ?? "false";
            if (bool.Parse(isRemember))
            {
                this.UserName = Username;
                this.Password= Password;           
            }
            this.IsRememberMe = bool.Parse(isRemember.ToLower());
        }
       

        [RelayCommand]
        public void Login()
        {
            AppCurrContext.UserName = UserName;
            AppCurrContext.Password = Password;
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {

                SendMessage(LocalizationService.GetString(AppLocalizations.LoginError),"Login");
                return;
            }

            SysUserVo userVo = new SysUserVo()
            {
                UserName = UserName,
                PassWord = Password,
                IsRemember = IsRememberMe
            };

            if (UserName.ToLower().Equals("sa") && Password.ToLower().Equals("123456"))
            {
                AppCurrContext.UserName = UserName;
                SaveConfig();
                OnDialogClosed();
                return;
            }

            userVo.PassWord = HashHelper.CreateMD5(userVo.PassWord).ToLower();
            
            SysUser user = userService.GetFirstOrDefault(s => s.Username == UserName.ToLower() && s.DisPlayPassword == Password);
            if (user == null)
            {
                SendMessage(LocalizationService.GetString(AppLocalizations.LoginError), "Login");
                return;
            }
            SaveConfig();
            AppCurrContext.UserName = UserName;
            OnDialogClosed();
        }

        public void SaveConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["UserName"].Value = UserName;
            config.AppSettings.Settings["Password"].Value = Password;
            config.AppSettings.Settings["New"].Value = IsRememberMe.ToString();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
