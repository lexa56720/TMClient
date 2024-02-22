﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TMApi;
using ApiWrapper.ApiWrapper.Wrapper;
using ApiWrapper.Interfaces;

namespace ApiWrapper.ApiWrapper
{
    public class ApiFactory
    {
        private readonly IPAddress server;
        private readonly int authPort;
        private readonly int apiPort;
        private readonly TimeSpan userLifetime;
        private readonly TimeSpan chatLifetime;
        private readonly int longPollPort;
        private readonly TimeSpan longPollPeriod;

        public ApiFactory(IPAddress server, int authPort, int apiPort, TimeSpan userLifetime, TimeSpan chatLifetime,
                                             int longPollPort, TimeSpan longPollPeriod) 
        {
            this.server = server;
            this.authPort = authPort;
            this.apiPort = apiPort;
            this.userLifetime = userLifetime;
            this.chatLifetime = chatLifetime;
            this.longPollPort = longPollPort;
            this.longPollPeriod = longPollPeriod;
        }
        public async Task<ClientApi?> CreateByLogin(string login, string password)
        {
            var provider = new ApiProvider(server, authPort, apiPort, longPollPort, longPollPeriod);
            var api = await provider.GetApiLogin(login, password);
            if (api == null)
                return null;
            return new ClientApi(userLifetime, chatLifetime, api);
        }
        public  async Task<ClientApi?> CreateByRegistration( string username, string login, string password)
        {
            var provider = new ApiProvider(server, authPort, apiPort, longPollPort, longPollPeriod);
            var api = await provider.GetApiRegistration(username, login, password);
            if (api == null)
                return null;
            return new ClientApi(userLifetime, chatLifetime, api);
        }
        public  async Task<IApi?> Load(string path)
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
                    var provider = new ApiProvider(server, authPort, apiPort, longPollPort, longPollPeriod);
                    var api = await provider.DeserializeAuthData(unprotectedBytes);
                    if (api == null)
                        return null;
                    return new ClientApi(userLifetime, chatLifetime, api);
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