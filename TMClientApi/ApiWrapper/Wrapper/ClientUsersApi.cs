﻿using ApiTypes.Communication.Users;
using TMApi;
using ApiWrapper.Interfaces;
using TMApi.ApiRequests.Messages;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    internal class ClientUsersApi : IUsersApi
    {
        private Api Api { get; }
        private ApiConverter Converter { get; }
        private CacheManager Cache { get; }
        private User CurrentUser { get; }

        internal ClientUsersApi(Api api, ApiConverter converter, CacheManager cacheManager, User currentUser)
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
                CurrentUser.Name = name;
                return true;
            }
            return false;
        }
        public Task<bool> ChangeProfilePic(byte[] imageData)
        {
            return Api.Users.SetProfileImage(imageData);
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
                user = ApiConverter.Convert(apiUser);
                Cache.AddToCache(user);
            }
            return user;
        }

        public async ValueTask<User[]> GetUser(int[] userIds)
        {
            if (userIds.Length == 0)
                return [];
            var result = new List<User>(userIds.Length);
            var requestedUsers = new List<int>();
            for (int i = 0; i < userIds.Length; i++)
            {
                if (Cache.TryGetUser(userIds[i], out var user))
                    result.Add(user);
                else
                    requestedUsers.Add(userIds[i]);
            }
            if (result.Count == userIds.Length)
                return result.ToArray();

            var converted = Converter.Convert(await Api.Users.GetUser(requestedUsers.Distinct().ToArray()));
            if (converted.Length == 0)
                return [];
            Cache.AddToCache(converted);
            result.AddRange(converted);

            return userIds.Select(userId => result.First(c => c.Id == userId)).ToArray();
        }


        internal async Task<User[]> GetUserIgnoringCache(int[] userIds)
        {
            var result = Converter.Convert(await Api.Users.GetUser(userIds.Distinct().ToArray()));
            return userIds.Select(userId => result.First(c => c.Id == userId)).ToArray();
        }
    }
}
