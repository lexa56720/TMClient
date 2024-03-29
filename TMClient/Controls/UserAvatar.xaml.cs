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

        public ImageSize AvatarSize
        {
            get => avatarSize;
            set
            {
                avatarSize = value;
                OnPropertyChanged(nameof(AvatarSize));
            }
        }
        private ImageSize avatarSize;


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
            ((UserAvatar)d).UserChanged((User)e.NewValue);
        }
        private void UserChanged(User newUser)
        {
            AvatarSize = GetAvatarSize(newUser);
        }
        private ImageSize GetAvatarSize(User user)
        {
            double maxSize = Height > Width ? Height : Width;

            if (maxSize <= 64)
                return ImageSize.Small;
            if (maxSize <= 128)
                return ImageSize.Medium;
            return ImageSize.Large;
        }
    }
}
