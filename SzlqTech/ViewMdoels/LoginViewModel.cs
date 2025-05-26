
using CommunityToolkit.Mvvm.ComponentModel;
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
    }
}
