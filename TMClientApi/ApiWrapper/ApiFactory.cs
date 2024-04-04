using System.Net;
using System.Security.Cryptography;
using TMApi;
using ApiWrapper.ApiWrapper.Wrapper;
using ApiWrapper.Interfaces;
using System.Text;

namespace ApiWrapper.ApiWrapper
{
    public class ApiFactory
    {
        private readonly IPAddress Server;
        private readonly int InfoPort;

        private readonly TimeSpan UserLifetime;
        private readonly TimeSpan ChatLifetime;
        private readonly SynchronizationContext UIContext;

        public ApiFactory(IPAddress server, int infoPort, TimeSpan userLifetime, TimeSpan chatLifetime, SynchronizationContext uiContext)
        {
            Server = server;
            InfoPort = infoPort;
            UIContext = uiContext;
            UserLifetime = userLifetime;
            ChatLifetime = chatLifetime;
        }

        public async Task<ClientApi?> CreateByLogin(string login, string password)
        {
            var provider = await ApiProvider.CreateProvider(Server, InfoPort);
            if (provider == null)
                return null;

            var api = await provider.GetApiLogin(login, password);
            if (api == null)
                return null;

            var passwordHash = ApiTypes.Shared.HashGenerator.GenerateHashSmall(password);
            return await ClientApi.Init(new IPEndPoint(Server, provider.ImageGetPort),
                                        passwordHash, UserLifetime, ChatLifetime, api, UIContext);
        }
        public async Task<ClientApi?> CreateByRegistration(string username, string login, string password)
        {
            var provider = await ApiProvider.CreateProvider(Server, InfoPort);
            if (provider == null)
                return null;

            var api = await provider.GetApiRegistration(username, login, password);
            if (api == null)
                return null;

            var passwordHash = ApiTypes.Shared.HashGenerator.GenerateHashSmall(password);
            return await ClientApi.Init(new IPEndPoint(Server, provider.ImageGetPort),
                                        passwordHash, UserLifetime, ChatLifetime, api, UIContext);
        }
        public async Task<IApi?> Load(string path)
        {
            if (File.Exists(path))
            {
                var bytes = await File.ReadAllBytesAsync(path);
                try
                {
                    byte[] unprotectedBytes = [];
                    await Task.Run(() =>
                    {
                        unprotectedBytes = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                    }).WaitAsync(TimeSpan.FromSeconds(2));

                    var provider = await ApiProvider.CreateProvider(Server, InfoPort);
                    if (provider == null)
                        return null;

                    var api = await provider.DeserializeAuthData(unprotectedBytes);
                    if (api == null)
                        return null;

                    var passwordHash = ClientApi.DeserializePasswordHash(unprotectedBytes);
                    return await ClientApi.Init(new IPEndPoint(Server, provider.ImageGetPort),
                                                passwordHash, UserLifetime, ChatLifetime, api, UIContext);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }


    }
}
