using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TMApi;

namespace TMClient.Model.Auth
{
    internal class AuthModel
    {
        public static ApiProvider ApiProvider
        {
            get
            {
                if (apiProvider == null)
                    apiProvider = GetApiProvider();
                return apiProvider;
            }
        }
        private static ApiProvider apiProvider = null!;


        public static async Task<Api?> TryGetApi()
        {
            var path = Path.Combine(App.AuthFolder, "authdata.bin");
            if (File.Exists(path))
            {
                var bytes = await File.ReadAllBytesAsync(path);
                try
                {
                    byte[] unprotectedBytes=Array.Empty<byte>();
                    await Task.Run(() =>
                       {
                           unprotectedBytes = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                       }).WaitAsync(TimeSpan.FromSeconds(2));
                    return await ApiProvider.Load(unprotectedBytes);

                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        public static async Task Update()
        {
            App.Settings.GetValue<int>("version");
        }
        private static ApiProvider GetApiProvider()
        {
            var ip = IPAddress.Parse(App.Settings.ConfigData["server-ip"]);
            var authPort = App.Settings.GetValue<int>("auth-port");
            var apiPort = App.Settings.GetValue<int>("api-port");
            return new ApiProvider(ip, authPort, apiPort);
        }
    }
}
