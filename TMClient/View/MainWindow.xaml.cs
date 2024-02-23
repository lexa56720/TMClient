using ApiWrapper.Interfaces;
using TMClient.Controls;
using TMClient.Utils;
using TMClient.ViewModel;

namespace TMClient.View
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            Messenger.Subscribe(Messages.CloseMainWindow, () => App.Current.Dispatcher.Invoke(Close));
        }

    }
}