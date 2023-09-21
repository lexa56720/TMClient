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
        public required User CurrentUser { get; init; }
        public Dictionary<int, User> Friends { get; set; } = new();

        public Dictionary<int, Chat> Chats { get; set; } = new();

        public ConcurrentDictionary<int, User> Users { get; set; } = new();


        [SetsRequiredMembers]
        public UserDataStorage(UserInfo currentUser)
        {
            CurrentUser = new User(currentUser.MainInfo);
        }

        public async Task Load(UserInfo currentUser)
        {
            var friends = LoadFriends(currentUser);
            foreach (var friend in friends)
            {
                Users.TryAdd(friend.Id, friend);
                Friends.TryAdd(friend.Id, friend);
            }

            var chats = await LoadChats(currentUser);
            foreach (var chat in chats)
                Chats.Add(chat.Id, chat);
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
            {
                Users.TryAdd(user.Id, user);
                Friends.TryAdd(user.Id, user);
            }

            return await Task.WhenAll(chats.Select(async c => new Chat(c, await GetUser(c.MemberIds))));
        }

        public async Task<User[]> GetUser(int[] ids)
        {
            ConcurrentBag<User> requestedUsers = new ConcurrentBag<User>();
            var tasks = new List<Task>();
            foreach (var id in ids)
                if (!Users.TryGetValue(id, out var user))
                    tasks.Add(new Task(async () =>
                    {
                        var apiUser = await App.Api.Users.GetUser(id);
                        if (apiUser != null)
                        {
                            var convertedUser = new User(apiUser);
                            Users.TryAdd(apiUser.Id, convertedUser);
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
            if (Users.TryGetValue(id, out var user) && user != null)
                return user;

            var apiUser = await App.Api.Users.GetUser(id);
            if (apiUser != null)
            {
                user = new User(apiUser);
                Users.TryAdd(user.Id, user);
                return user;
            }
            return null;
        }


        public async Task<Chat?> GetChat(int id)
        {
            if (Chats.TryGetValue(id, out var chat) && chat != null)
                return chat;

            var apiChat = await App.Api.Chats.GetChat(id);
            if (apiChat != null)
            {
                chat = new Chat(apiChat, await GetUser(apiChat.MemberIds));
                Chats.TryAdd(chat.Id, chat);
                return chat;
            }
            return null;
        }
    }
}
