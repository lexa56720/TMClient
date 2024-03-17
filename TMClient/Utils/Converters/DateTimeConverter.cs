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
            var time = (DateTime)value;
            var now = DateTime.Now;

            time = time.ToLocalTime();

            if (now.Year == time.Year)
            {
                if (now.DayOfYear == time.DayOfYear)
                    return "Сегодня в " + time.ToShortTimeString();
                else
                {
                    var difference = now - time;
                    if (difference.TotalHours < 48)
                        return time.ToString("Вчера в HH:mm");
                    return time.ToString("d MMMM в HH:mm");

                }
            }
            return time.ToString("d MMMM yyyy");
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
