using ApiTypes.Communication.Users;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi;
using TMApi.ApiRequests.Friends;
using TMClient.Types;
using TMClient.Utils;

namespace TMClientApi.Storages
{
    public class UserDataStorage
    {
        public ObservableCollection<User> Friends { get; } = new();

        public ObservableCollection<Chat> FriendChats { get; } = new();
        public ObservableCollection<Chat> MultiUserChats { get; } = new();

        private ConcurrentDictionary<int, Chat> CachedChats { get; } = new();
        private ConcurrentDictionary<int, User> CachedUsers { get; } = new();


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
            foreach (var dialogue in chats.dialogues)
            {
                CachedChats.TryAdd(dialogue.Id, dialogue);
                FriendChats.Add(dialogue);
            }
            foreach (var chat in chats.chats)
            {
                CachedChats.TryAdd(chat.Id, chat);
                FriendChats.Add(chat);
            }
        }
        private User[] LoadFriends(UserInfo currentUser)
        {
            return currentUser.Friends.Select(f => new User(f)).ToArray();
        }
        private async Task<(Chat[] dialogues, Chat[] chats)> LoadChats()
        {
            var dialogues = await App.Api.Chats.GetAllDialogues();
            var multiuserChats = await App.Api.Chats.GetAllNonDialogues();


            var ids = dialogues.Concat(multiuserChats).SelectMany(c => c.MemberIds).Distinct().ToArray();
            var users = await GetUser(ids);

            return (await Task.WhenAll(dialogues.Select(async c => new Chat(c, await GetUser(c.MemberIds)))),
               await Task.WhenAll(multiuserChats.Select(async c => new Chat(c, await GetUser(c.MemberIds)))));
        }

        public async ValueTask<User[]> GetUser(int[] ids)
        {
            var result = new List<User>();
            var requestedUsers = new List<int>();
            foreach (var id in ids)
                if (!CachedUsers.TryGetValue(id, out var user))
                    requestedUsers.Add(id);
                else
                    result.Add(user);

            var converted = ApiConverter.Convert(await App.Api.Users.GetUser(requestedUsers.ToArray()));
            converted.AsParallel().ForAll(c => CachedUsers.TryAdd(c.Id, c));

            result.AddRange(converted);
            return result.ToArray();
        }
        public async ValueTask<User?> GetUser(int id)
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


        public async ValueTask<Chat[]> GetChat(int[] ids)
        {
            var result = new List<Chat>();
            var requestedChats = new List<int>();
            foreach (var id in ids)
                if (!CachedChats.TryGetValue(id, out var chat))
                    requestedChats.Add(id);
                else
                    result.Add(chat);

            var converted = await ApiConverter.Convert(this, await App.Api.Chats.GetChat(requestedChats.ToArray()));
            converted.AsParallel().ForAll(c => CachedChats.TryAdd(c.Id, c));

            result.AddRange(converted);
            return result.ToArray();
        }
        public async ValueTask<Chat?> GetChat(int id)
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
