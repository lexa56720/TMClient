using ApiTypes.Communication.Users;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TMApi;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;
using TMApi.ApiRequests.Users;
using ClientApiWrapper.Types;
using System;

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

            var chats = await clientApi.Chats.GetChat(api.UserInfo.Chats);
            clientApi.Cache.AddToCache(TimeSpan.FromTicks(int.MaxValue), chats);
            foreach (var chat in chats)
                if (!chat.IsDialogue)
                    clientApi.MultiuserChats.Add(chat);


            var friends = clientApi.Converter.Convert(api.UserInfo.Friends);
            clientApi.Cache.AddToCache(TimeSpan.FromTicks(int.MaxValue), friends);
            foreach (var friend in friends)
                clientApi.FriendList.Add(new Friend(friend, chats.Single(c => c.Members.Any(m => m.Id == friend.Id))));

           
            var requests = await clientApi.Friends.GetFriendRequest(api.UserInfo.FriendRequests);
            foreach (var request in requests)
                clientApi.FriendRequests.Add(request);


            var invites = await clientApi.Chats.GetChatInvite(api.UserInfo.ChatInvites);
            foreach (var invite in invites)
                clientApi.ChatInvites.Add(invite);

            return clientApi;
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
