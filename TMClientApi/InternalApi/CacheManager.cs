using PerformanceUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi.ApiRequests.Chats;
using TMApi.ApiRequests.Users;
using TMClientApi.Types;

namespace TMClientApi.InternalApi
{
    internal class CacheManager
    {
        private LifeTimeDictionary<int, User> CachedUsers = new();

        private LifeTimeDictionary<int, Chat> CachedChats = new();

        private readonly TimeSpan userLifetime;
        private readonly TimeSpan chatLifetime;
        public CacheManager(TimeSpan userLifetime, TimeSpan chatLifetime)
        {
            this.userLifetime = userLifetime;
            this.chatLifetime = chatLifetime;
        }

        public void Clear()
        {
            CachedUsers.Clear();
            CachedChats.Clear();
        }

        public User? GetUser(int userId)
        {
            CachedUsers.TryGetValue(userId, out var user);
            return user;
        }
        public Chat? GetChat(int chatId)
        {
            CachedChats.TryGetValue(chatId, out var chat);
            return chat;
        }
        public bool TryGetUser(int userId, out User? user)
        {
            return CachedUsers.TryGetValue(userId, out user);
        }
        public bool TryGetChat(int chatId, out Chat? chat)
        {
            return CachedChats.TryGetValue(chatId, out chat);

        }
        public bool UpdateCache(params User[] users)
        {
            bool isSuccessful = true;
            foreach (var user in users)
            {
                if (!CachedUsers.TryGetValue(user.Id, out var cachedUser))
                    isSuccessful = false;

                cachedUser.Update(user);
                if (!CachedUsers.UpdateLifetime(user.Id, userLifetime))
                    isSuccessful = false;
            }
            return isSuccessful;
        }
        public bool UpdateCache(params Chat[] chats)
        {
            bool isSuccessful = true;
            foreach (var chat in chats)
            {
                if (!CachedChats.TryGetValue(chat.Id, out var cachedChat))
                    return false;

                cachedChat.Update(chat);
                if (!CachedChats.UpdateLifetime(chat.Id, chatLifetime))
                    isSuccessful = false;
            }
            return isSuccessful;
        }



        public bool AddToCache(params User[] users)
        {
            bool isSuccessful = true;
            foreach (var user in users)
            {
                if (!CachedUsers.TryAdd(user.Id, user, userLifetime))
                    isSuccessful = false;
            }
            return isSuccessful;
        }
        public bool AddToCache(params Chat[] chats)
        {
            bool isSuccessful = true;
            foreach (var chat in chats)
            {
                if (!CachedChats.TryAdd(chat.Id, chat, chatLifetime))
                    isSuccessful = false;
            }
            return isSuccessful;
        }
    }
}
