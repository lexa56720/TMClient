using System.Windows.Controls;
using TMClient.ViewModel;
using TMClient.ViewModel.Pages;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Profile()
        {
            DataContext = new ProfileViewModel();
            InitializeComponent();
        }
    }
}
