
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;

namespace SzlqTech.ViewMdoels
{
    public partial class LoginViewModel: DialogViewModel
    {
        public LoginViewModel()
        {
            
        }

        [ObservableProperty]
        public bool isRememberMe;

        [ObservableProperty]
        public string userName="sa";

        [ObservableProperty]
        public string password="123456";

        [RelayCommand]
        public async Task Login()
        {
            AppCurrContext.UserName = UserName;
            AppCurrContext.Password = Password;

            if (UserName.Equals("sa"))
            {
                if (Password.Equals("123456"))
                {
                    UserName=string.Empty;
                    Password=string.Empty;
                    OnDialogClosed();
                    await Task.CompletedTask;
                }
            }
        }

        [RelayCommand]
        public void Forget()
        {

        }
    }
}
