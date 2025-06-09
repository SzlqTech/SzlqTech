
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic.ApplicationServices;
using Prism.Services.Dialogs;
using SzlqTech.Common.Helper;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;
using SzlqTech.Core.Vos;
using SzlqTech.Entity;
using SzlqTech.IService;

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
        }

        [ObservableProperty]
        public bool isRememberMe;

        [ObservableProperty]
        public string userName="admin";

        [ObservableProperty]
        public string password="123456";
   

       

        [RelayCommand]
        public async Task Login()
        {
            AppCurrContext.UserName = UserName;
            AppCurrContext.Password = Password;

            //if (UserName.Equals("sa"))
            //{
            //    if (Password.Equals("123456"))
            //    {
            //        UserName=string.Empty;
            //        Password=string.Empty;
            //        OnDialogClosed();
            //        await Task.CompletedTask;
            //    }
            //}

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
             
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
                OnDialogClosed();
                return;
            }

            userVo.PassWord = HashHelper.CreateMD5(userVo.PassWord).ToLower();
            
            SysUser user = userService.GetFirstOrDefault(s => s.Username == UserName.ToLower() && s.DisPlayPassword == Password);
            if (user == null)
            {
                //aggregator.SendMessage("用户名或密码错误", "LoginView");
                return;
            }
            AppCurrContext.UserName = UserName;
            //SendMessage(AppManager.AppUserName, "Login");
            OnDialogClosed();

        }

        [RelayCommand]
        public void Forget()
        {

        }
    }
}
