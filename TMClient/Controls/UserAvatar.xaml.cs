using ApiTypes.Communication.Medias;
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
    public partial class UserAvatar : UserControl
    {
        public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register(nameof(User),
                                    typeof(User),
                                    typeof(UserAvatar),
                                    new PropertyMetadata(null, UserChanged));
        public User User
        {
            get => (User)GetValue(UserProperty);
            set
            {
                SetValue(UserProperty, value);
            }
        }


        public static readonly DependencyProperty AvatarSizeProperty =
        DependencyProperty.Register(nameof(AvatarSize),
                                    typeof(ImageSize),
                                    typeof(UserAvatar),
                                    new PropertyMetadata(ImageSize.Medium));

        public ImageSize AvatarSize
        {
            get => (ImageSize)GetValue(AvatarSizeProperty);
            set
            {
                SetValue(AvatarSizeProperty, value);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public UserAvatar()
        {
            InitializeComponent();
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private static void UserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UserAvatar)d).UserChanged();
        }
        private void UserChanged()
        {
            AvatarSize = GetAvatarSize();
        }
        private ImageSize GetAvatarSize()
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
