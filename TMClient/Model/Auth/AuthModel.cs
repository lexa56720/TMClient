using ApiWrapper.ApiWrapper;
using ApiWrapper.Interfaces;
using System.IO;
using System.Net;

namespace TMClient.Model.Auth
{
    internal class AuthModel
    {
        public static ApiFactory ApiProvider
        {
            get
            {
                if (apiProvider == null)
                    apiProvider = GetApiProvider();
                return apiProvider;
            }
        }
        private static ApiFactory apiProvider = null!;


        public static async Task<IApi?> TryGetApi()
        {
            var path = Path.Combine(App.AuthPath, "authdata.bin");
            return await ApiProvider.Load(path);
        }
        public static async Task Update()
        {
            App.Settings.GetValue<int>("version");
        }
        private static ApiFactory GetApiProvider()
        {
            var ip = IPAddress.Parse(App.Settings.ConfigData["server-ip"]);
            var authPort = App.Settings.GetValue<int>("auth-port");
            var apiPort = App.Settings.GetValue<int>("api-port");
            var cachedUserLifetime = TimeSpan.FromMinutes(App.Settings.GetValue<int>("cached-user-lifetime-minutes"));
            var cachedChatLifetime = TimeSpan.FromMinutes(App.Settings.GetValue<int>("cached-chat-lifetime-minutes"));
            var longPollPort = App.Settings.GetValue<int>("long-poll-port");
            var longPollPeriod = TimeSpan.FromMinutes(App.Settings.GetValue<int>("long-poll-period-minutes"));
            return new ApiFactory(ip, authPort, apiPort, cachedUserLifetime,
                                  cachedChatLifetime, longPollPort, longPollPeriod,SynchronizationContext.Current);
        }
    }
}
