using ApiTypes.Shared;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using TMApi;

namespace TMClient
{



    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Api? Api { get; set; }

        public static Configurator Settings = new Configurator("config.cfg", true);

        public static string AuthFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/TmApp/userdata/auth");

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Api != null)
            {
                var bytes = Api.GetAuthData();
                ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
                Directory.CreateDirectory(AuthFolder);
                using var fs = File.Create(Path.Combine(AuthFolder, "authdata.bin"));
                fs.Write(bytes);
            }
        }
    }
}