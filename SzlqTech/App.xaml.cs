using Hardcodet.Wpf.TaskbarNotification;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;
using SzlqTech.Core.Consts;
using SzlqTech.Core.Services.App;
using SzlqTech.Core.Services.Session;
using SzlqTech.Services.Sessions;
using SzlqTech.ViewMdoels;
using SzlqTech.Views;


namespace SzlqTech
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication, IAppTaskBar
    {
        protected override Window CreateShell() => null;

        private TaskbarIcon? taskBar;


        protected override void RegisterTypes(IContainerRegistry service)
        {
            service.RegisterForNavigation<MainView,MainViewModel>(AppViews.Main);
            service.RegisterDialog<LoginView,LoginViewModel>(AppViews.Login);
            service.RegisterSingleton<IAppStartService, MainStartService>();
            service.RegisterSingleton<IHostDialogService, DialogHostService>();
        }

        protected override  void OnInitialized()
        {
            Initialization();
            var appStart = ContainerLocator.Container.Resolve<IAppStartService>();
            appStart.CreateShell();
            base.OnInitialized();
        }

        public void Dispose() => taskBar?.Dispose();

        public void Initialization()
        {
            taskBar = (TaskbarIcon)FindResource("taskBar");
        }

        public void ShowBalloonTip(string title, string message, BalloonIcon balloonIcon)
        {
            taskBar.ShowBalloonTip(title, message, balloonIcon);
        }
    }

}
