using ApiTypes.Communication.Medias;
using ClientApiWrapper.Types;
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
    internal class EntityImageSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value[0] is NamedImageEntity entity && value[1] is ImageSize size &&
                entity.ImageSmall != null && entity.ImageMedium != null && entity.ImageLarge != null)
            {
                ImageSourceConverter conv = new ImageSourceConverter();

                return size switch
                {
                    ImageSize.Small => conv.ConvertFromString(entity.ImageSmall),
                    ImageSize.Medium => conv.ConvertFromString(entity.ImageMedium),
                    ImageSize.Large => conv.ConvertFromString(entity.ImageLarge),
                    ImageSize.Original => conv.ConvertFromString(entity.ImageLarge),
                    _ => conv.ConvertFromString(entity.ImageSmall),
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
