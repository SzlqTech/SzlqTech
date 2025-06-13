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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SzlqTech.Common.Views;
using SzlqTech.Core.Consts;

namespace SzlqTech.Core.WorkFlow.Views
{
    /// <summary>
    /// InnoLightDataRecord.xaml 的交互逻辑
    /// </summary>
    [View(AppViews.InnoLightDataRecordView, AppLocalizations.DataQuery, AppLocalizations.WorkFlow, "dataQuery", "workFlow", Ordinal = 1)]
    public partial class InnoLightDataRecord : UserControl
    {
        public InnoLightDataRecord()
        {
            InitializeComponent();
        }
    }
}
