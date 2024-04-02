using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TMClient.Utils.Converters
{
    class SearchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values.Last() is not string query || string.IsNullOrWhiteSpace(query))
                return Visibility.Visible;

            bool isFound = false;
            for (int i = 0; i < values.Length - 1; i++)
            {
                if (values[i] is string item && item.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                {
                    isFound = true;
                    break;
                }
            }
            return isFound ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
