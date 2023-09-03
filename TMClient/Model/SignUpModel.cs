using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMApi;

namespace TMClient.Model
{
    internal class SignUpModel
    {
        public static async Task<Api?> Registration(string name, string login, string password)
        {
            var ip = IPAddress.Parse(App.Settings.ConfigData["server-ip"]);
            var authPort = App.Settings.GetValue<int>("auth-port");
            var apiPort = App.Settings.GetValue<int>("api-port");
            var provider = new ApiProvider(ip, authPort, apiPort);
            return await provider.GetApiRegister(name, login, password);
        }
    }
}
