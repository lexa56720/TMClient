using ApiTypes.Communication.Medias;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TMClient.Utils.Converters
{
    internal class UserAvatarSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value[0] is User user && value[1] is ImageSize size &&
                user.ProfilePicSmall != null && user.ProfilePicMedium != null && user.ProfilePicLarge != null)
            {
                ImageSourceConverter conv = new ImageSourceConverter();

                return size switch
                {
                    ImageSize.Small => conv.ConvertFromString(user.ProfilePicSmall),
                    ImageSize.Medium => conv.ConvertFromString(user.ProfilePicMedium),
                    ImageSize.Large => conv.ConvertFromString(user.ProfilePicLarge),
                    ImageSize.Original => conv.ConvertFromString(user.ProfilePicLarge),
                    _ => conv.ConvertFromString(user.ProfilePicSmall),
                };

            }
            return null;
        }


        public object ConvertBack(object[] value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
