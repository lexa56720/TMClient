using ClientApiWrapper.ApiWrapper;
using ClientApiWrapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model.Auth
{
    internal class BaseAuthModel
    {
        protected ApiFactory ApiProvider
        {
            get
            {
                return GetApiProvider();
            }
        }
        private static ApiFactory apiProvider = null!;
        private static SynchronizationContext UIContext;
        public BaseAuthModel()
        {
            UIContext = SynchronizationContext.Current;
        }

        public async Task<IApi?> TryGetApi()
        {
            try
            {
                return await ApiProvider.Load(Preferences.Default.AuthPath);
            }
            catch
            {
                return null;
            }
        }

        private static ApiFactory GetApiProvider()
        {
            var ip = IPAddress.Parse(Preferences.Default.ServerAddress);
            var infoPort = Preferences.Default.InfoPort;
            var cachedUserLifetime = TimeSpan.FromMinutes(Preferences.Default.CachedUserLifetimeMinutes);
            var cachedChatLifetime = TimeSpan.FromMinutes(Preferences.Default.CachedChatLifetimeMinutes);

            return new ApiFactory(ip, infoPort, cachedUserLifetime, cachedChatLifetime, App.Version, UIContext);
        }


        public bool IsLoginValid(string login)
        {
            return ApiTypes.Shared.DataConstraints.IsLoginLegal(login);
        }

        public bool IsPasswordValid(string pass)
        {
            return ApiTypes.Shared.DataConstraints.IsPasswordLegal(pass);
        }

        public void SaveIsAuth(bool value)
        {
            Preferences.Default.IsSaveAuth = value;
            Preferences.Default.Save();
        }
    }
}
