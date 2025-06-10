using System.Windows;
using System.Windows.Controls;
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Views;
using SzlqTech.Core.Consts;

namespace SqlqTech.SharedView.Views
{
    /// <summary>
    /// ScannerSettingView.xaml 的交互逻辑
    /// </summary>
    [View(AppViews.ScannerSetting, AppLocalizations.ScanManagement, AppLocalizations.ConfigManagement, "scanner", "dashboard", Ordinal = 0)]
    public partial class ScannerSettingView : UserControl
    {
        public ScannerSettingView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cboxScanner.ItemsSource = Enum.GetNames(typeof(ScannerType));
        }
    }
}
