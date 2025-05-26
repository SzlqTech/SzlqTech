
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public string userName;

        [ObservableProperty]
        public string password;

        [RelayCommand]
        public async Task Login()
        {
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
