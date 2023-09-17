using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Utils
{
    internal static class DateTimeExtension
    {
        static public string ToText(this DateTime time)
        {

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
                    return time.ToString("dd MMMM в HH:mm");

                }
            }
            return time.ToString("dd MMMM yyyy");

        }
    }
}
