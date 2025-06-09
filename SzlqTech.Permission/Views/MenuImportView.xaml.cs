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

namespace SzlqTech.Permission.Views
{
    /// <summary>
    /// MenuImportView.xaml 的交互逻辑
    /// </summary>
    [View("菜单导入", "系统管理", "BookOpenBlankVariant", Ordinal = 0)]
    public partial class MenuImportView : UserControl
    {
        public MenuImportView()
        {
            InitializeComponent();
        }
    }
}
