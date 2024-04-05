using System.Windows.Controls;
using TMClient.ViewModel;
using TMClient.ViewModel.Pages;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для Notifications.xaml
    /// </summary>
    public partial class Notifications : Page
    {
        public Notifications()
        {
            InitializeComponent();
            DataContext = new NotificationsViewModel();
        }
    }
}
