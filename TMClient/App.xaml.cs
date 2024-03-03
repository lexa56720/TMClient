using ApiTypes.Shared;
using ApiWrapper.ApiWrapper.Wrapper;
using ApiWrapper.Interfaces;
using System.Configuration.Provider;
using System.IO;
using System.Windows;
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
        private IApi? Api { get; set; } = null;

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
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var dialog = new MainAuthWindow();

            if (dialog.ShowDialog() == true)
            {
                var mainWindow = new MainWindow(dialog.Api);
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
        public void Logout()
        {
            Api?.Dispose();
            Api = null;
            ApplicationStart(this,null);
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