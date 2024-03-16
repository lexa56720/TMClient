using ApiTypes.Communication.Users;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TMApi;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;
using TMApi.ApiRequests.Users;
using System;
using ApiWrapper.Wrapper;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    public class ClientApi : IApi, IUserInfo
    {
        public User Info { get; private set; }

        public event EventHandler<Message[]> NewMessages
        {
            add
            {
                LongPollManager.NewMessages += value;
            }
            remove
            {
                LongPollManager.NewMessages -= value;
            }
        }
        public event EventHandler<int[]> ReadedMessages
        {
            add
            {
                LongPollManager.ReadedMessages += value;
            }
            remove
            {
                LongPollManager.ReadedMessages -= value;
            }
        }
        public IUsersApi Users => users;
        private readonly ClientUsersApi users;

        public IMessagesApi Messages => messages;
        private readonly ClientMessagesApi messages;

        public IFriendsApi Friends => friends;
        private readonly ClientFriendsApi friends;

        public IChatsApi Chats => chats;
        private readonly ClientChatsApi chats;

        public ObservableCollection<Chat> MultiuserChats { get; private set; } = new();
        public ObservableCollection<Friend> FriendList { get; private set; } = new();
        public ObservableCollection<FriendRequest> FriendRequests { get; private set; } = new();
        public ObservableCollection<ChatInvite> ChatInvites { get; private set; } = new();

        private ApiConverter Converter { get; set; }
        private CacheManager Cache { get; set; }
        private LongPollManager LongPollManager { get; set; }

        private readonly Api Api;

        private bool IsDisposed = false;

        private ClientApi(TimeSpan userLifetime, TimeSpan chatLifetime, Api api, SynchronizationContext uiContext)
        {
            Info = ApiConverter.Convert(api.UserInfo.MainInfo);

            Cache = new CacheManager(userLifetime, chatLifetime);
            Cache.AddToCache(Info);
            Converter = new ApiConverter(this);

            chats = new ClientChatsApi(api, Converter, Cache);
            users = new ClientUsersApi(api, Converter, Cache, Info);

            messages = new ClientMessagesApi(api, Converter);
            friends = new ClientFriendsApi(api, Converter);

            LongPollManager = new LongPollManager(api.LongPolling, this, uiContext);
            Api = api;
        }
        internal static async Task<ClientApi?> Init(TimeSpan userLifetime, TimeSpan chatLifetime, Api api, SynchronizationContext uiContext)
        {
            var clientApi = new ClientApi(userLifetime, chatLifetime, api, uiContext);

            var chats = await InitChats(clientApi, api.UserInfo.Chats);
            clientApi.Cache.AddToCache(TimeSpan.FromMilliseconds(int.MaxValue), chats);

            var friends = InitFriends(clientApi, api.UserInfo.Friends, chats);
            clientApi.Cache.AddToCache(TimeSpan.FromMilliseconds(int.MaxValue), friends);

            await InitRequests(clientApi, api.UserInfo.FriendRequests);

            await InitInvites(clientApi, api.UserInfo.ChatInvites);

            return clientApi;
        }
        private static async Task<Chat[]> InitChats(ClientApi api, int[] chatIds)
        {
            var chats = await api.Chats.GetChat(chatIds);
            var lastMessages = await api.Messages.GetLastMessages(chatIds);
            for (int i = 0; i < chats.Length; i++)
            {
                if (!chats[i].IsDialogue)
                    api.MultiuserChats.Add(chats[i]);
                chats[i].LastMessage = lastMessages[i];
                if (lastMessages[i] != null && lastMessages[i].IsOwn)
                    chats[i].UnreadCount = 0;
            }
            return chats;
        }
        private static User[] InitFriends(ClientApi api, ApiUser[] apiFriends, Chat[] chats)
        {
            var friends = api.Converter.Convert(apiFriends);
            foreach (var friend in friends)
                api.FriendList.Add(new Friend(friend, chats.Single(c => c.IsDialogue && c.Members.Any(m => m.Id == friend.Id))));
            return friends;
        }
        private static async Task InitRequests(ClientApi api, int[] requestIds)
        {
            var requests = await api.Friends.GetFriendRequest(requestIds);
            api.FriendRequests = new ObservableCollection<FriendRequest>(requests);
        }
        private static async Task InitInvites(ClientApi api, int[] invitesIds)
        {
            var invites = await api.Chats.GetChatInvite(invitesIds);
            api.ChatInvites = new ObservableCollection<ChatInvite>(invites);
        }


        public void Dispose()
        {
            if (IsDisposed)
                return;

            LongPollManager.Dispose();
            Api.Dispose();
            Cache.Clear();
            IsDisposed = true;
        }

        public async Task Save(string path)
        {
            var bytes = Api.SerializeAuthData();
            var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            using var fs = File.Create(path);
            await fs.WriteAsync(protectedBytes);
        }
    }
}
