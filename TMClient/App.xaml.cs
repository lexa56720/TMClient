using ApiTypes.Shared;
using ApiWrapper.ApiWrapper.Wrapper;
using ApiWrapper.Interfaces;
using System.IO;
using System.Windows;
using TMClient.Model;
using TMClient.Utils;
using TMClient.ViewModel;

namespace TMClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IApi? Api
        {
            get => api;
            set
            {
                if (api != null)
                    api.NewMessages -= NewMessages;
                if (value != null)
                    value.NewMessages += NewMessages;

                api = value;
                BaseModel.Api = value;
                BaseViewModel.CurrentUser = value;
            }
        }

        private static void NewMessages(object? sender, Message[] e)
        {
            Messenger.Send(Messages.NewMessagesArived, e);
        }

        private static IApi? api = null;
        public static Configurator Settings { get; } = new Configurator("config.cfg", true);

        public static bool IsSaveAuth { get; set; } = true;
        public static bool IsAutoLogin { get; set; } = true;
        public static string AppFolder { get; private set; } = string.Empty;
        public static string AuthPath { get; private set; } = string.Empty;

        public App()
        {
            AuthPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/userdata/auth/authdata.bin");

            AppFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/");

            Messenger.Subscribe<IApi>(Messages.AuthCompleted, (o, e) => Api = e);
        }

        public static void Logout()
        {
            Api?.Dispose();
            Api = null;
            IsAutoLogin = false;
        }
        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Api == null || IsSaveAuth == false)
            {
                if (File.Exists(AuthPath))
                    File.Delete(AuthPath);
            }
            else
            {
                await Api.Save(AuthPath);
            }
        }
    }
}