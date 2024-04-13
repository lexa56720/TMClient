using ApiTypes.Shared;
using ClientApiWrapper.ApiWrapper.Wrapper;
using ClientApiWrapper.Interfaces;
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
        public static Dispatcher MainThread => App.Current.Dispatcher;
        public App()
        {
            if (string.IsNullOrWhiteSpace(Preferences.Default.AuthPath))
                Preferences.Default.AuthPath = Path.Combine(
                       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/userdata/auth/authdata.bin");

            if (string.IsNullOrWhiteSpace(Preferences.Default.AppFolder))
                Preferences.Default.AppFolder = Path.Combine(
                       Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/");

            Preferences.Default.Save();

            Messenger.Subscribe(Messages.Logout, Logout);
        }
        private async void ApplicationStart(object sender, StartupEventArgs? e)
        {
            await OpenPage(true);
        }
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            using var sw=File.CreateText("logs.txt");
            sw.WriteLine(args.Exception);
            sw.Flush();
            sw.Close();
            MessageBox.Show("Возникло исключение. подробности в файле logs.txt");

            args.Handled = true;
            Api?.Dispose();
            Environment.Exit(0);
        }
        private async Task OpenPage(bool tryToLoadAuth)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var dialog = new MainAuthWindow(tryToLoadAuth);

            if (dialog.ShowDialog() == true)
            {
                Api = dialog.Api;
                await HandleAuthData();
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                var mainWindow = new MainWindow(Api);
                MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Api?.Dispose();
                Shutdown();
            }
        }
        public async void Logout()
        {
            Api?.Dispose();
            Api = null;
            await OpenPage(false);
        }
        private async void ApplicationExit(object sender, ExitEventArgs e)
        {
            await HandleAuthData();
        }

        private async Task HandleAuthData()
        {
            if (Preferences.Default.IsSaveAuth == false)
            {
                if (File.Exists(Preferences.Default.AuthPath))
                    File.Delete(Preferences.Default.AuthPath);
            }
            else if (Api != null)
                await Api.Save(Preferences.Default.AuthPath);
        }
    }
}