using System.Windows.Controls;
using SzlqTech.Common.Views;
using SzlqTech.Core.Consts;

namespace SqlqTech.SharedView.Views
{
    /// <summary>
    /// SysConfigView.xaml 的交互逻辑
    /// </summary>
    [View(AppViews.SysConfig, AppLocalizations.SysConfig, AppLocalizations.ConfigManagement, "sysConfig", "dashboard", Ordinal = 0)]
    public partial class SysConfigView : UserControl
    {
        public SysConfigView()
        {
            InitializeComponent();
        }
    }
}
