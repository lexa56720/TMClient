using ApiTypes.Communication.Medias;
using ClientApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMClient.Utils;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserAvatar.xaml
    /// </summary>
    public partial class EntityImage : UserControl
    {
        public static readonly DependencyProperty EntityProperty =
        DependencyProperty.Register(nameof(Entity),
                                    typeof(NamedImageEntity),
                                    typeof(EntityImage),
                                    new PropertyMetadata(null, EntityChanged));
        public NamedImageEntity Entity
        {
            get => (NamedImageEntity)GetValue(EntityProperty);
            set
            {
                SetValue(EntityProperty, value);
            }
        }


        public static readonly DependencyProperty AvatarSizeProperty =
        DependencyProperty.Register(nameof(ImageSize),
                                    typeof(ImageSize),
                                    typeof(EntityImage),
                                    new PropertyMetadata(ImageSize.Medium));

        public ImageSize ImageSize
        {
            get => (ImageSize)GetValue(AvatarSizeProperty);
            set
            {
                SetValue(AvatarSizeProperty, value);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public EntityImage()
        {
            InitializeComponent();
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private static void EntityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EntityImage)d).EntityChanged();
        }
        private void EntityChanged()
        {
            ImageSize = GetImageSize();
        }
        private ImageSize GetImageSize()
        {
            double maxSize = Height > Width ? Height : Width;

            if (maxSize <= 64 || double.IsNaN(maxSize))
                return ImageSize.Small;
            if (maxSize <= 128)
                return ImageSize.Medium;
            return ImageSize.Large;
        }
    }
}
