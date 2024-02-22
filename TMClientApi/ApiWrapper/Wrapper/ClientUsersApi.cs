using ApiTypes.Communication.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    internal class ClientUsersApi : IUsersApi
    {
        private Api Api { get; }
        private ApiConverter Converter { get; }
        private CacheManager Cache { get; }
        private UserInfo CurrentUser { get; }

        internal ClientUsersApi(Api api, ApiConverter converter, CacheManager cacheManager, UserInfo currentUser)
        {
            Api = api;
            Converter = converter;
            Cache = cacheManager;
            CurrentUser = currentUser;
        }

        public async Task<bool> ChangeName(string name)
        {
            if (await Api.Users.ChangeName(name))
            {
                CurrentUser.MainInfo.Name = name;
                return true;
            }
            return false;
        }

        public async Task<User[]> GetByName(string name)
        {
            var users = Converter.Convert(await Api.Users.GetByName(name));
            Cache.UpdateCache(users);
            return users;
        }

        public async ValueTask<User?> GetUser(int userId)
        {
            if (!Cache.TryGetUser(userId, out var user))
            {
                var apiUser = await Api.Users.GetUser(userId);
                if (apiUser == null)
                    return null;
                user = Converter.Convert(apiUser);
                Cache.AddToCache(user);
            }
            return user;
        }

        public async ValueTask<User[]> GetUser(int[] userIds)
        {
            var result = new List<User>(userIds.Length);
            var requestedUsers = new List<int>();
            for (int i = 0; i < userIds.Length; i++)
            {
                if (Cache.TryGetUser(userIds[i], out var user))
                    result[i] = user;
                else
                    requestedUsers.Add(userIds[i]);
            }
            var converted = Converter.Convert(await Api.Users.GetUser(requestedUsers.Distinct().ToArray()));
            Cache.AddToCache(converted);
            result.AddRange(converted);

            return userIds.Select(userId => converted.First(c => c.Id == userId)).ToArray();
        }
    }
}
