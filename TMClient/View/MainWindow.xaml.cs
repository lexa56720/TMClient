using ApiWrapper.Interfaces;
using TMClient.Controls;
using TMClient.Model;
using TMClient.Utils;
using TMClient.ViewModel;

namespace TMClient.View
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {

        public MainWindow(IApi api)
        {
            BaseViewModel.CurrentUser = api;
            BaseModel.Api = api;
            InitializeComponent();

            DataContext = new MainViewModel();
            Messenger.Subscribe(Messages.CloseMainWindow, () => App.MainThread.Invoke(Close));
        }

    }
}