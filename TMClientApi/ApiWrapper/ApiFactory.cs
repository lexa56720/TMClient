using System.Net;
using System.Security.Cryptography;
using TMApi;
using ClientApiWrapper.ApiWrapper.Wrapper;
using ClientApiWrapper.Interfaces;
using System.Text;
using TMApi.API;

namespace ClientApiWrapper.ApiWrapper
{
    public class ApiFactory
    {
        private readonly IPAddress Server;
        private readonly int InfoPort;

        private readonly TimeSpan UserLifetime;
        private readonly TimeSpan ChatLifetime;
        private readonly int Version;
        private readonly SynchronizationContext UIContext;

        public ApiFactory(IPAddress server, int infoPort, TimeSpan userLifetime, TimeSpan chatLifetime, int version, SynchronizationContext uiContext)
        {
            Server = server;
            InfoPort = infoPort;
            UIContext = uiContext;
            UserLifetime = userLifetime;
            ChatLifetime = chatLifetime;
            Version = version;
        }

        public async Task<ClientApi?> CreateByLogin(string login, string password)
        {
            ApiProvider provider;
            try
            {
                provider = await GetProvider();
            }
            catch
            {
                throw;
            }
            if (provider == null)
                throw new Exception("Сервер недоступен");
            if (provider.ServerInfo.Version != Version)
                throw new Exception("Версия клиента устарела");

            var api = await provider.GetApiLogin(login, password);
            if (api == null)
                return null;

            var passwordHash = ApiTypes.Shared.HashGenerator.GenerateHashSmall(password);
            return await ClientApi.Init(provider.ServerInfo, new IPEndPoint(Server, provider.ServerInfo.FileGetPort),
                                        passwordHash, UserLifetime, ChatLifetime, api, UIContext);
        }
        public async Task<ClientApi?> CreateByRegistration(string username, string login, string password)
        {
            ApiProvider provider;
            try
            {
                provider = await GetProvider();
            }
            catch
            {
                throw;
            }

            var api = await provider.GetApiRegistration(username, login, password);
            if (api == null)
                return null;

            var passwordHash = ApiTypes.Shared.HashGenerator.GenerateHashSmall(password);
            return await ClientApi.Init(provider.ServerInfo, new IPEndPoint(Server, provider.ServerInfo.FileGetPort),
                                        passwordHash, UserLifetime, ChatLifetime, api, UIContext);
        }
        public async Task<IApi?> Load(string path)
        {

            if (File.Exists(path))
            {
                ApiProvider provider;
                try
                {
                    provider = await GetProvider();
                }
                catch
                {
                    throw;
                }

                var bytes = await File.ReadAllBytesAsync(path);
                try
                {
                    byte[] unprotectedBytes = [];
                    await Task.Run(() =>
                    {
                        unprotectedBytes = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                    }).WaitAsync(TimeSpan.FromSeconds(2));


                    var api = await provider.DeserializeAuthData(unprotectedBytes);
                    if (api == null)
                        return null;

                    var passwordHash = ClientApi.DeserializePasswordHash(unprotectedBytes);
                    return await ClientApi.Init(provider.ServerInfo, new IPEndPoint(Server, provider.ServerInfo.FileGetPort),
                                                passwordHash, UserLifetime, ChatLifetime, api, UIContext);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        private async Task<ApiProvider> GetProvider()
        {
            var provider = await ApiProvider.CreateProvider(Server, InfoPort);
            if (provider == null)
                throw new Exception("Сервер недоступен");
            if (provider.ServerInfo.Version != Version)
                throw new Exception("Версия клиента устарела");
            return provider;
        }

    }
}
