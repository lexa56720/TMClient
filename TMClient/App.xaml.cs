using ApiTypes.Shared;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using TMApi;
using TMClient.Types;

namespace TMClient
{



    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static UserDataStorage? UserData { get; private set; }
        public static Api? Api { get; set; }

        public static Configurator Settings = new Configurator("config.cfg", true);
        public static string AppFolder => Path.Combine(
           Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/");
        public static string AuthFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TmApp/userdata/auth");

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Api != null)
            {
                var bytes = Api.GetAuthData();
                var protectedBytes= ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
                Directory.CreateDirectory(AuthFolder);
                using var fs = File.Create(Path.Combine(AuthFolder, "authdata.bin"));
                fs.Write(protectedBytes);
            }
        }
    }
}