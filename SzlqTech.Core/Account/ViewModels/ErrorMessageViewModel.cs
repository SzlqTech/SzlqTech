using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Services.Dialogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.ViewModels;

namespace SzlqTech.Core.Account.ViewModels
{
    public partial class ErrorMessageViewModel : HostDialogViewModel
    {
        [ObservableProperty]
        public string content;

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters != null && parameters.ContainsKey(AppSharedConsts.Parameter))
            {
                Content = parameters.GetValue<string>(AppSharedConsts.Parameter);
            }
        }
    }
}
