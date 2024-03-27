using System.Globalization;
using System.Windows.Data;

namespace TMClient.Utils.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class LoginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"@{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value)[1..];
        }
    }
}
