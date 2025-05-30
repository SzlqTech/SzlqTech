using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace SzlqTech.Localization
{
    [MarkupExtensionReturnType(typeof(string))]
    public class LocExtension : MarkupExtension
    {
        public string Key { get; set; }
        public object[] Args { get; set; }

        public LocExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding(nameof(LocBindingSource.Value))
            {
                Source = new LocBindingSource(Key, Args)
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
