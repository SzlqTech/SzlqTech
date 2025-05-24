using CommunityToolkit.Mvvm.ComponentModel;


namespace SzlqTech.Core.ViewModels
{
    
    public partial class ViewModelBase: ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(IsNotBusy))]
        public bool isBusy;

        
        public bool IsNotBusy => !IsBusy;

     

        public virtual async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            try
            {
                await func();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
