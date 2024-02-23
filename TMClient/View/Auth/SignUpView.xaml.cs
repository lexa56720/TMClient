using System.Windows.Controls;
using TMClient.ViewModel.Auth;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для RegisterView.xaml
    /// </summary>
    public partial class SignUpView : Page
    {
        public SignUpView()
        {
            InitializeComponent();
            DataContext = new SignUpViewModel();
        }
    }
}
