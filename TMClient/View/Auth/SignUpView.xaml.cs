using ClientApiWrapper.Interfaces;
using System.Windows.Controls;
using TMClient.ViewModel.Auth;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для RegisterView.xaml
    /// </summary>
    public partial class SignUpView : Page
    {
        public SignUpView(Func<IApi?, bool> returnApi)
        {
            InitializeComponent();
            DataContext = new SignUpViewModel(returnApi);
        }
    }
}
