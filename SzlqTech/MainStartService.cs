
using SzlqTech.Core.Services.App;
using Prism.Ioc;
using SzlqTech.Core.Services.Session;
using Prism.Services.Dialogs;
using SzlqTech.Core.Consts;
using Prism.Regions;
using System.Windows;

namespace SzlqTech
{
    public class MainStartService : IAppStartService
    {
        public void CreateShell()
        {
            var container = ContainerLocator.Container;
            if (!Authorization()) Exit();
            var shell = container.Resolve<object>(AppViews.Main);
            if (shell is Window view)
            {
                var regionManager = container.Resolve<IRegionManager>();
                RegionManager.SetRegionManager(view, regionManager);
                RegionManager.UpdateRegions();

                if (view.DataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(null);
                    App.Current.MainWindow = view;
                }
            }
        }

        private bool Authorization()
        {
            var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
            return dialogService.ShowWindow(AppViews.Login).Result==ButtonResult.OK;
        }

        public void Exit()
        {
            
        }

        public void Logout()
        {
           
        }
    }
}
