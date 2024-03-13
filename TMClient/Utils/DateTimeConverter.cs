using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TMClient.Utils
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToText();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (string)value;

            if (time.Contains("Сегодня в "))
                return DateTime.Parse(time.Replace("Сегодня в ", null));

            if (time.Contains("Вчера в "))
            {
                return DateTime.ParseExact(time, "Вчера в HH:mm", null);
            }
            var formats = new string[]
            {
                "Вчера в HH:mm",
                "d MMMM в HH:mm",
                "d MMMM yyyy"
            };
            if (DateTime.TryParseExact(time, formats, null, DateTimeStyles.None, out var result))
            {
                return result;
            }
            return DateTime.MinValue;
        }
    }
}
