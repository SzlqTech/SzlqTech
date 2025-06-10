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
using SzlqTech.Common.EnumType;
using SzlqTech.Common.Views;
using SzlqTech.Core.Consts;

namespace SqlqTech.SharedView.Views
{
    /// <summary>
    /// MachineSettingView.xaml 的交互逻辑
    /// </summary>
    [View(AppViews.MachineSetting, AppLocalizations.MachineManagement, AppLocalizations.ConfigManagement, "plc","dashboard", Ordinal = 0)]
    public partial class MachineSettingView : UserControl
    {
        public MachineSettingView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cboxPLC.ItemsSource= Enum.GetNames(typeof(MachineModel)).ToList();

        }
    }
}
