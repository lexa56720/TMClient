using PerformanceUtils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClientApi.Types;

namespace TMClientApi.InternalApi
{
    internal class CacheManager
    {
        private LifeTimeDictionary<int, User> CachedUsers = new();

        private LifeTimeDictionary<int, Chat> CachedChats = new();


        public void Clear()
        {
            CachedUsers.Clear();
            CachedChats.Clear();
        }

        public async ValueTask<User?> GetUser(int userId)
        {
            CachedUsers.TryGetValue(userId, out var user);
            return user;
        }
        public async ValueTask<Chat?> GetChat(int chatId)
        {
            CachedChats.TryGetValue(chatId, out var chat);
            return chat;
        }

        public bool UpdateCache(User user, TimeSpan lifetime)
        {
            if (!CachedUsers.TryGetValue(user.Id, out var cachedUser))
                return false;

            cachedUser.Update(user);
            return CachedUsers.UpdateLifetime(user.Id, lifetime);
        }
        public bool UpdateCache(Chat chat, TimeSpan lifetime)
        {
            if (!CachedChats.TryGetValue(chat.Id, out var cachedChat))
                return false;

            cachedChat.Update(chat);
            return CachedChats.UpdateLifetime(chat.Id, lifetime);
        }

        public bool AddToCache(User user, TimeSpan lifetime)
        {
            return CachedUsers.TryAdd(user.Id, user, lifetime);
        }
        public bool AddToCache(Chat chat, TimeSpan lifetime)
        {
            return CachedChats.TryAdd(chat.Id, chat, lifetime);
        }
    }
}
