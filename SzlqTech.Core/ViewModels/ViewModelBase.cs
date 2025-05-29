using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Events;
using Prism.Ioc;
using SzlqTech.Core.Events;


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
