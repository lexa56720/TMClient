using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для UserDisplay.xaml
    /// </summary>
    public partial class UserDisplay : UserControl
    {
        public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register(nameof(User),
                                    typeof(User),
                                    typeof(UserDisplay),
                                    new PropertyMetadata(null));
        public User User
        {
            get => (User)GetValue(UserProperty);
            set
            {
                SetValue(UserProperty, value);
            }
        }

        public static readonly DependencyProperty IsProfileLinkEnabledProperty =
        DependencyProperty.Register(nameof(IsProfileLinkEnabled),
                                    typeof(bool),
                                    typeof(UserDisplay),
                                    new PropertyMetadata(true));
        public bool IsProfileLinkEnabled
        {
            get => (bool)GetValue(IsProfileLinkEnabledProperty);
            set
            {
                SetValue(IsProfileLinkEnabledProperty, value);
            }
        }


        public UserDisplay()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Messenger.Send(Messages.ModalOpened, true);
            var mainwindow = App.Current.MainWindow;
            var profileCard = new View.ProfileCard(User)
            {
                Owner = mainwindow,
                ShowInTaskbar = false
            };
            profileCard.ShowDialog();
            Messenger.Send(Messages.ModalClosed, true);
        }
    }
}
