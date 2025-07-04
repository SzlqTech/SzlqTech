using System.Globalization;
using System.Windows.Data;

namespace SzlqTech.Core.WorkFlow.Converters
{
    public class StartButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return $"/SzlqTech.Core.WorkFlow;component/Assets/darkLight.png";
            var res = bool.Parse(value.ToString());
            if (res) return $"/SzlqTech.Core.WorkFlow;component/Assets/stop.png";
            return $"/SzlqTech.Core.WorkFlow;component/Assets/start.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
