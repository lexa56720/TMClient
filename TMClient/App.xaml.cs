using ApiTypes.Shared;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using TMApi;
using TMClient.Types;
using TMClient.Types.Storages;

namespace TMClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Api Api
        {
            get => api;
            set
            {
                api = value;
            }
        }
        private static Api? api = null!;
        public static User? CurrentUser { get; private set; }
        public static Configurator Settings { get; } = new Configurator("config.cfg", true);

        public static bool IsSaveAuth { get; set; } = true;
        public static bool IsAutoLogin { get; set; } = true;

        internal static UserDataStorage UserData { get; private set; } = new();
        internal static RequestStorage Requests { get; private set; } = new();
        internal static UpdateStorage? Updates { get; private set; }
        public static string AppFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/");
        public static string AuthFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/userdata/auth");

        public static async Task InitAppData()
        {
            CurrentUser = new User(Api.UserInfo.MainInfo);

            Updates = new UpdateStorage();
            await UserData.Load(Api.UserInfo);
            await Requests.Load();
        }

        public static void Logout()
        {
            CurrentUser = null!;
            App.Api = null!;
            App.IsAutoLogin = false;

            UserData.Clear();
            Requests.Clear();
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Api == null || IsSaveAuth == false)
            {
                if (File.Exists(Path.Combine(AuthFolder, "authdata.bin")))
                    File.Delete(Path.Combine(AuthFolder, "authdata.bin"));
                return;
            }


            var bytes = Api.SerializeAuthData();
            var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            Directory.CreateDirectory(AuthFolder);
            using var fs = File.Create(Path.Combine(AuthFolder, "authdata.bin"));
            fs.Write(protectedBytes);
        }
    }
}