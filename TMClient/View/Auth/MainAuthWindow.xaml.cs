using ApiWrapper.Interfaces;
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
        public IApi Api { get; set; }
        public MainAuthWindow(bool loadAuth)
        {
            InitializeComponent();
            Title = "Авторизация";
            DataContext = new MainAuthViewModel(AuthCompleted, loadAuth);
        }

        private bool AuthCompleted(IApi? api)
        {    
            if (api == null)
                return false;

            DialogResult = true;
            Api = api;
            Close();
            return true;
        }
    }
}
