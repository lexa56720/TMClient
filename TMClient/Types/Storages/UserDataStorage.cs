using ApiTypes.Communication.Users;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi.ApiRequests.Friends;

namespace TMClient.Types
{
    public class UserDataStorage
    {
        public ObservableCollection<User> Friends { get; } = new();
        public ObservableCollection<Chat> Chats { get;  } = new();

        public ConcurrentDictionary<int, Chat> CachedChats { get; } = new();
        public ConcurrentDictionary<int, User> CachedUsers { get; } = new();


        public void Clear()
        {
            Friends.Clear();
            Chats.Clear();

            CachedChats.Clear();
            CachedUsers.Clear();
        }

        public async Task Load(UserInfo currentUser)
        {
            var friends = LoadFriends(currentUser);
            foreach (var friend in friends)
            {
                CachedUsers.TryAdd(friend.Id, friend);
                Friends.Add(friend);
            }

            var chats = await LoadChats(currentUser);
            foreach (var chat in chats)
            {
                CachedChats.TryAdd(chat.Id, chat);
                Chats.Add(chat);
            }
        }
        private User[] LoadFriends(UserInfo currentUser)
        {
            return currentUser.Friends.Select(f => new User(f)).ToArray();
        }
        private async Task<Chat[]> LoadChats(UserInfo currentUser)
        {
            var chats = await App.Api.Chats.GetChat(currentUser.Chats);
            var ids = chats.SelectMany(c => c.MemberIds).Distinct().ToArray();
            var users = (await App.Api.Users.GetUser(ids)).Select(u => new User(u));

            foreach (var user in users)
                CachedUsers.TryAdd(user.Id, user);

            return await Task.WhenAll(chats.Select(async c => new Chat(c, await GetUser(c.MemberIds))));
        }

        public async Task<User[]> GetUser(int[] ids)
        {
            ConcurrentBag<User> requestedUsers = new ConcurrentBag<User>();
            var tasks = new List<Task>();
            foreach (var id in ids)
                if (!CachedUsers.TryGetValue(id, out var user))
                    tasks.Add(new Task(async () =>
                    {
                        var apiUser = await App.Api.Users.GetUser(id);
                        if (apiUser != null)
                        {
                            var convertedUser = new User(apiUser);
                            CachedUsers.TryAdd(apiUser.Id, convertedUser);
                            requestedUsers.Add(convertedUser);
                        }
                    }));
                else
                    requestedUsers.Add(user);

            await Task.WhenAll(tasks);
            return requestedUsers.ToArray();
        }
        public async Task<User?> GetUser(int id)
        {
            if (CachedUsers.TryGetValue(id, out var user) && user != null)
                return user;

            var apiUser = await App.Api.Users.GetUser(id);
            if (apiUser != null)
            {
                user = new User(apiUser);
                CachedUsers.TryAdd(user.Id, user);
                return user;
            }
            return null;
        }

        public async Task<Chat?> GetChat(int id)
        {
            if (CachedChats.TryGetValue(id, out var chat) && chat != null)
                return chat;

            var apiChat = await App.Api.Chats.GetChat(id);
            if (apiChat != null)
            {
                chat = new Chat(apiChat, await GetUser(apiChat.MemberIds));
                CachedChats.TryAdd(chat.Id, chat);
                return chat;
            }
            return null;
        }
    }
}
