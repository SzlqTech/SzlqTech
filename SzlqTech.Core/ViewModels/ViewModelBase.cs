using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Events;
using Prism.Ioc;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Events;
using SzlqTech.Localization;


namespace SzlqTech.Core.ViewModels
{
    
    public partial class ViewModelBase: ObservableObject
    {
        public ViewModelBase()
        {           
            aggregator=ContainerLocator.Container.Resolve<IEventAggregator>();
        }
        [ObservableProperty]
       // [NotifyCanExecuteChangedFor(nameof(IsNotBusy))]
        public bool isBusy;
        public readonly IEventAggregator aggregator;

        public bool IsNotBusy => !IsBusy;

        public void SendSuccessMsg()
        {
            SendMessage(LocalizationService.GetString(AppLocalizations.SuccessMsg));
        }

        public void SendErrorMsg()
        {
            SendMessage(LocalizationService.GetString(AppLocalizations.ErrorMsg));
        }


        public void SendDeleteSuccessMsg()
        {
            SendMessage(LocalizationService.GetString(AppLocalizations.DeleteSuccessMsg));
        }

        public void SendDeleteErrorMsg()
        {
            SendMessage(LocalizationService.GetString(AppLocalizations.DeleteErrorMsg));
        }


        public void SendMessage(string msg)
        {
            aggregator.SendSnackBarMessage(msg);
        }

        public virtual async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            aggregator.SendBusyAsyncMessage(new BusyAsyncModel() { IsOpen = IsBusy });
            try
            {
                await func();
            }
            finally
            {
                IsBusy = false;
                aggregator.SendBusyAsyncMessage(new BusyAsyncModel() { IsOpen = IsBusy });
            }
        }
    }
}
