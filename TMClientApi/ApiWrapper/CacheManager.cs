using PerformanceUtils.Collections;
using System.Diagnostics.CodeAnalysis;
using TMApi.ApiRequests.Chats;

namespace ApiWrapper.ApiWrapper
{
    internal class CacheManager
    {
        private LifeTimeDictionary<int, User> CachedUsers = new();

        private LifeTimeDictionary<int, Chat> CachedChats = new();

        private readonly TimeSpan UserLifetime;
        private readonly TimeSpan ChatLifetime;
        public CacheManager(TimeSpan userLifetime, TimeSpan chatLifetime)
        {
            UserLifetime = userLifetime;
            ChatLifetime = chatLifetime;
        }

        public void Clear()
        {
            CachedUsers.Clear();
            CachedChats.Clear();
        }

        public bool AddToCache(params User[] users)
        {
            return AddToCache(UserLifetime, users);
        }
        public bool AddToCache(params Chat[] chats)
        {
            return AddToCache(ChatLifetime, chats);
        }

        public bool UpdateCache(params User[] users)
        {
            return UpdateCache(UserLifetime, users);
        }
        public bool UpdateCache(params Chat[] chats)
        {
            return UpdateCache(ChatLifetime, chats);
        }

        public bool AddOrUpdateCache(params User[] users)
        {
            return AddOrUpdateCache(UserLifetime, users);
        }
        public bool AddOrUpdateCache(params Chat[] chats)
        {
            return AddOrUpdateCache(ChatLifetime, chats);
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
                if (CachedUsers.TryAdd(user.Id, user, UserLifetime))
                    continue;

                isSuccessful = false;
            }
            return isSuccessful;
        }
        public bool AddToCache(TimeSpan lifeTime, params Chat[] chats)
        {
            bool isSuccessful = true;
            foreach (var chat in chats)
            {
                if (CachedChats.TryAdd(chat.Id, chat, ChatLifetime))
                    continue;

                isSuccessful = false;
            }
            AddToCache(lifeTime, chats.SelectMany(c => c.Members).DistinctBy(c => c.Id).ToArray());
            return isSuccessful;
        }

        public bool AddOrUpdateCache(TimeSpan lifeTime, params User[] users)
        {
            bool isSuccessful = true;
            foreach (var user in users)
            {
                if (CachedUsers.TryAdd(user.Id, user, UserLifetime))
                    continue;

                isSuccessful = false;
                UpdateCache(lifeTime, user);
            }
            return isSuccessful;
        }
        public bool AddOrUpdateCache(TimeSpan lifeTime, params Chat[] chats)
        {
            bool isSuccessful = true;
            foreach (var chat in chats)
            {
                if (CachedChats.TryAdd(chat.Id, chat, ChatLifetime))
                    continue;

                isSuccessful = false;
                UpdateCache(lifeTime, chat);
            }
            AddToCache(lifeTime, chats.SelectMany(c => c.Members).DistinctBy(c => c.Id).ToArray());
            return isSuccessful;
        }

        public bool TryGetUser(int userId, [MaybeNullWhen(false)] out User user)
        {
            return CachedUsers.TryGetValue(userId, out user);
        }
        public bool TryGetChat(int chatId, [MaybeNullWhen(false)] out Chat chat)
        {
            return CachedChats.TryGetValue(chatId, out chat);
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

    }
}

