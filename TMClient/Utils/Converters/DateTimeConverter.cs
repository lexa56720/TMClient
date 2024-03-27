using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TMClient.Utils.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (DateTime)value;
            var now = DateTime.Now;

            time = time.ToLocalTime();

            if (now.Year == time.Year)
            {
                if (now.DayOfYear == time.DayOfYear)
                    return  time.ToShortTimeString();
                else
                {
                    var difference = now - time;
                    if (difference.TotalHours < 24)
                        return time.ToString("Вчера в HH:mm");
                    return time.ToString("d MMMM в HH:mm");

                }
            }
            return time.ToString("d MMMM yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
