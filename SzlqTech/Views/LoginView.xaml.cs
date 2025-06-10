using Prism.Events;
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
using SzlqTech.Core.Events;

namespace SzlqTech.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(IEventAggregator aggregator)
        {
            InitializeComponent();
            btnClose.Click += (s, e) => { Environment.Exit(0); };
           
            //注册提示消息
            aggregator.ResgiterSnackBarMessage(arg =>
            {
                Snackbar.MessageQueue?.Enqueue(arg.Message);
            },"Login");
        }
    }
}
