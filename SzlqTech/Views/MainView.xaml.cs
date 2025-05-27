using Dm.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SzlqTech.Core.Services.App;
using SzlqTech.Themes.Controls;
using SzlqTech.ViewMdoels;

namespace SzlqTech.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IAppStartService appStartService;

        public MainView(IAppStartService appStartService)
        {
            InitializeComponent();
            this.appStartService = appStartService;

            HeaderBorder.MouseDown += (s, e) =>
            {
                if (e.ClickCount == 2) SetWindowState();
            };
            HeaderBorder.MouseMove += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            BtnMin.Click += BtnMin_Click;
            BtnMax.Click += BtnMax_Click;
            BtnClose.Click += BtnClose_Click;
        }


        private  void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //if (await dialog.Question(Local.Localize("AreYouSure")))
                appStartService.Exit();
        }

        private void BtnMax_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetWindowState();
        }

        private void BtnMin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowState = ((base.WindowState != System.Windows.WindowState.Minimized) ?
               System.Windows.WindowState.Minimized : System.Windows.WindowState.Normal);

            this.Hide();
        }

        private void SetWindowState()
        {
            this.WindowState = ((base.WindowState != System.Windows.WindowState.Maximized) ? System.Windows.WindowState.Maximized : System.Windows.WindowState.Normal);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource != null && e.OriginalSource is TabCloseItem tabItem)
            {
                if (this.DataContext is MainViewModel viewModel)
                {
                    viewModel.NavigationService.RemoveView(tabItem.Content);
                }     
            }
        }
    }
}
