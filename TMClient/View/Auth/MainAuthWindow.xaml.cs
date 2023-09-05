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
using System.Windows.Shapes;
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
            Messenger.Subscribe(Messages.CloseAuth, () => Application.Current.Dispatcher.Invoke(Close));

            DataContext = new MainAuthViewModel();
        }

    }
}
