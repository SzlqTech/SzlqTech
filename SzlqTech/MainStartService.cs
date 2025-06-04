
using SzlqTech.Core.Services.App;
using Prism.Ioc;
using SzlqTech.Core.Services.Session;
using Prism.Services.Dialogs;
using SzlqTech.Core.Consts;
using Prism.Regions;
using System.Windows;
using NLog;
using System.IO;
using System.Windows.Threading;

namespace SzlqTech
{
    public class MainStartService : IAppStartService
    {
       
        public void CreateShell()
        {
            //var container = ContainerLocator.Container;
            //if (!Authorization()) Exit();
            //var shell = container.Resolve<object>(AppViews.Main);
            //if (shell is Window view)
            //{
            //    var regionManager = container.Resolve<IRegionManager>();
            //    RegionManager.SetRegionManager(view, regionManager);
            //    RegionManager.UpdateRegions();

            //    if (view.DataContext is INavigationAware navigationAware)
            //    {
            //        navigationAware.OnNavigatedTo(null);
            //        App.Current.MainWindow = view;
            //    }
            //}
        }

        private bool Authorization()
        {
            //var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
            //return dialogService.ShowWindow(AppViews.Login).Result==ButtonResult.OK;

            var validationResult = Validation();
            //if (validationResult == ButtonResult.Retry)
            //    return Authorization();

            return validationResult == ButtonResult.OK;

            static ButtonResult Validation()
            {
                var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
                return dialogService.ShowWindow(AppViews.Login).Result;
            }
        }

        public void Exit()
        {
            if (System.Windows.Application.Current is IAppTaskBar appTaskBar)
                appTaskBar.Dispose();
            Application.Current.Shutdown(0);
            //Environment.Exit(0);
        }

        public void Logout()
        {
            App.Current.MainWindow.Hide();   
            App.Current.MainWindow.Show();
            if (App.Current.MainWindow.DataContext is INavigationAware navigationAware)
                navigationAware.OnNavigatedTo(null);
        }

       
    }
}
