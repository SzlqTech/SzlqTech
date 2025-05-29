using System.Windows;
using System.Windows.Controls;
using SzlqTech.Common.EnumType;

namespace SqlqTech.SharedView.Views
{
    /// <summary>
    /// ScannerSettingView.xaml 的交互逻辑
    /// </summary>
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
