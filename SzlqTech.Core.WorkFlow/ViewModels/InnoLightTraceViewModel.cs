using CommunityToolkit.Mvvm.Input;
using Prism.Services.Dialogs;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.Session;
using SzlqTech.Core.ViewModels;
using SzlqTech.Localization;

namespace SzlqTech.Core.WorkFlow.ViewModels
{
    public partial class InnoLightTraceViewModel:NavigationViewModel
    {
        private readonly IHostDialogService dialog;

        public InnoLightTraceViewModel(IHostDialogService dialog)
        {
            Title = LocalizationService.GetString(AppLocalizations.InnoLight);
            this.dialog = dialog;
        }

        //[RelayCommand]
        //public async Task Test()
        //{
        //    DialogParameters para=new DialogParameters();
        //    para.Add(AppSharedConsts.Parameter, "当前物料缺失");
        //    var dialogResult = await dialog.ShowDialogAsync(AppViews.ErrorMessageView, para);
        //    if (dialogResult.Result == ButtonResult.OK)
        //    {

        //    }
        //}
    }
}
