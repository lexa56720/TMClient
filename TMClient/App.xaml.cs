using ApiTypes.Shared;
using ApiWrapper.ApiWrapper.Wrapper;
using ApiWrapper.Interfaces;
using System.Configuration.Provider;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TMClient.Model;
using TMClient.Utils;
using TMClient.View;
using TMClient.View.Auth;
using TMClient.ViewModel;

namespace TMClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IApi? Api;

        public static Dispatcher MainThread=>App.Current.Dispatcher;
        public App()
        {
            Preferences.Default.AuthPath = Path.Combine(
                       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/userdata/auth/authdata.bin");

            Preferences.Default.AppFolder = Path.Combine(
                       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/");

            Messenger.Subscribe(Messages.Logout, Logout);
        }
        private void ApplicationStart(object sender, StartupEventArgs? e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var window = new ImagePickerWindow(@"D:\1.jpg");
            MainWindow = window;
            window.Show();
            //var dialog = new MainAuthWindow();

            //if (dialog.ShowDialog() == true)
            //{
            //    Api = dialog.Api;
            //    ShutdownMode = ShutdownMode.OnMainWindowClose;
            //    var mainWindow = new MainWindow(Api);
            //    MainWindow = mainWindow;
            //    mainWindow.Show();
            //}
            //else
            //{
            //    Shutdown();
            //}
        }
        public void Logout()
        {
            Api?.Dispose();
            Api = null;
            ApplicationStart(this, null);
        }
        private async void ApplicationExit(object sender, ExitEventArgs e)
        {
            if (Api == null || Preferences.Default.IsSaveAuth == false)
            {
                if (File.Exists(Preferences.Default.AuthPath))
                    File.Delete(Preferences.Default.AuthPath);
            }
            else
            {
                Preferences.Default.Save();
                await Api.Save(Preferences.Default.AuthPath);
            }
        }
    }
}