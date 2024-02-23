using System.Windows;
using TMClient.Controls;
using TMClient.Utils;
using TMClient.ViewModel.Auth;

namespace TMClient.View.Auth
{
    /// <summary>
    /// Логика взаимодействия для MainAuthWindow.xaml
    /// </summary>
    public partial class MainAuthWindow : ModernWindow
    {
        public MainAuthWindow()
        {
            InitializeComponent();
            Title = "Авторизация";
            Messenger.Subscribe(Messages.CloseAuth, () => Application.Current.Dispatcher.Invoke(Close));

            DataContext = new MainAuthViewModel();
        }

    }
}
