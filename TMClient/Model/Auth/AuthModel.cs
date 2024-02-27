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
                apiProvider ??= GetApiProvider();
                return apiProvider;
            }
        }
        private static ApiFactory apiProvider = null!;


        public static async Task<IApi?> TryGetApi()
        {
            return await ApiProvider.Load(Preferences.Default.AuthPath);
        }
        private static ApiFactory GetApiProvider()
        {
            var ip = IPAddress.Parse(Preferences.Default.ServerAddress);
            var authPort = Preferences.Default.AuthPort;
            var apiPort = Preferences.Default.ApiPort;
            var longPollPort = Preferences.Default.LongPollPort;
            var longPollPeriod = TimeSpan.FromMinutes(Preferences.Default.LongPollPeriodMinutes);
            var cachedUserLifetime = TimeSpan.FromMinutes(Preferences.Default.CachedUserLifetimeMinutes);
            var cachedChatLifetime = TimeSpan.FromMinutes(Preferences.Default.CachedChatLifetimeMinutes);
              
            return new ApiFactory(ip, authPort, apiPort, cachedUserLifetime,cachedChatLifetime, 
                                  longPollPort, longPollPeriod,SynchronizationContext.Current);
        }
    }
}
