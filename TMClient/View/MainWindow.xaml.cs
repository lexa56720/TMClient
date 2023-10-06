using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMApi;
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

        private MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            Messenger.Subscribe(Messages.CloseMainWindow, () => App.Current.Dispatcher.Invoke(Close));
        }

        public static async Task<MainWindow> GetInstance(Api? api)
        {
            if (api == null)
                Environment.Exit(53);
            App.Api = api;
            await App.InitAppData();

            return new MainWindow();
        }

    }
}