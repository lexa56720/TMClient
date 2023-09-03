using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMApi;

namespace TMClient.Model
{
    internal class SignInModel
    {
        public static async  Task<Api?> SignIn(string login, string password)
        {
            var ip = IPAddress.Parse(App.Settings.ConfigData["server-ip"]);
            var authPort = App.Settings.GetValue<int>("auth-port");
            var apiPort = App.Settings.GetValue<int>("api-port");
            var provider = new ApiProvider(ip, authPort, apiPort);
            return await provider.GetApiLogin(login, password);
        }
    }
}
