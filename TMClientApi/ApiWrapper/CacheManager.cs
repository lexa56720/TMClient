using PerformanceUtils.Collections;
using TMApi.ApiRequests.Chats;

namespace ApiWrapper.ApiWrapper
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
        public bool AddToCache(params User[] users)
        {
            return AddToCache(userLifetime, users);
        }
        public bool AddToCache(params Chat[] chats)
        {
            return AddToCache(chatLifetime, chats);
        }
        public bool UpdateCache(params User[] users)
        {
            return UpdateCache(userLifetime, users);
        }
        public bool UpdateCache(params Chat[] chats)
        {
            return UpdateCache(chatLifetime, chats);
        }
        public bool UpdateCache(TimeSpan lifeTime, params User[] users)
        {
            bool isSuccessful = true;
            foreach (var user in users)
            {
                if (!CachedUsers.TryGetValue(user.Id, out var cachedUser))
                {
                    isSuccessful = false;
                    continue;
                }

                cachedUser.Update(user);
                if (!CachedUsers.UpdateLifetime(user.Id, lifeTime))
                    isSuccessful = false;
            }
            return isSuccessful;
        }
        public bool UpdateCache(TimeSpan lifeTime, params Chat[] chats)
        {
            bool isSuccessful = true;
            foreach (var chat in chats)
            {
                if (!CachedChats.TryGetValue(chat.Id, out var cachedChat))
                {
                    isSuccessful = false;
                    continue;
                }

                cachedChat.Update(chat);
                if (!CachedChats.UpdateLifetime(chat.Id, lifeTime))
                    isSuccessful = false;
            }
            return isSuccessful;
        }

        public bool AddToCache(TimeSpan lifeTime, params User[] users)
        {
            bool isSuccessful = true;
            foreach (var user in users)
            {
                if (CachedUsers.TryAdd(user.Id, user, userLifetime))
                    continue;

                isSuccessful = false;
                UpdateCache(lifeTime, user);
            }
            return isSuccessful;
        }
        public bool AddToCache(TimeSpan lifeTime, params Chat[] chats)
        {
            bool isSuccessful = true;
            foreach (var chat in chats)
            {
                if (CachedChats.TryAdd(chat.Id, chat, chatLifetime))
                    continue;

                isSuccessful = false;
                UpdateCache(lifeTime, chat);
            }
            return isSuccessful;
        }
    }
}
