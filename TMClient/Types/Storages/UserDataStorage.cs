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

        public ObservableCollection<Chat> FriendChats { get; } = new();
        public ObservableCollection<Chat> MultiUserChats { get; } = new();

        public ConcurrentDictionary<int, Chat> CachedChats { get; } = new();
        public ConcurrentDictionary<int, User> CachedUsers { get; } = new();


        public void Clear()
        {
            Friends.Clear();
            MultiUserChats.Clear();
            FriendChats.Clear();

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

            var chats = await LoadChats();
            foreach (var dialogue in chats.Item1)
            {
                CachedChats.TryAdd(dialogue.Id, dialogue);
                FriendChats.Add(dialogue);
            }
            foreach (var chat in chats.Item2)
            {
                CachedChats.TryAdd(chat.Id, chat);
                FriendChats.Add(chat);
            }
        }
        private User[] LoadFriends(UserInfo currentUser)
        {
            return currentUser.Friends.Select(f => new User(f)).ToArray();
        }
        private async Task<(Chat[], Chat[])> LoadChats()
        {
            var dialogues = await App.Api.Chats.GetAllDialogues();
            var multiuserChats = await App.Api.Chats.GetAllNonDialogues();


            var ids = dialogues.Concat(multiuserChats).SelectMany(c => c.MemberIds).Distinct().ToArray();
            var users = (await App.Api.Users.GetUser(ids)).Select(u => new User(u));

            foreach (var user in users)
                CachedUsers.TryAdd(user.Id, user);

            return (await Task.WhenAll(dialogues.Select(async c => new Chat(c, await GetUser(c.MemberIds)))),
               await Task.WhenAll(multiuserChats.Select(async c => new Chat(c, await GetUser(c.MemberIds)))));
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
