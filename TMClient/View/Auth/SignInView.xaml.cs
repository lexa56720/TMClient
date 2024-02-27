using ApiWrapper.Interfaces;
using System.Windows.Controls;
using TMClient.ViewModel.Auth;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для AuthView.xaml
    /// </summary>
    public partial class AuthView : Page
    {
        public AuthView(Func<IApi?, bool> returnApi)
        {
            InitializeComponent();
            DataContext = new SingInViewModel(returnApi);
        }

    }
}
