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

namespace SzlqTech.Core.WorkFlow.Views
{
    /// <summary>
    /// InnoLightTraceView.xaml 的交互逻辑
    /// </summary>
    public partial class InnoLightTraceView : UserControl
    {
        private readonly IEventAggregator aggregator;

        public InnoLightTraceView(IEventAggregator aggregator)
        {
            InitializeComponent();
            this.aggregator = aggregator;   
        }
    }
}
