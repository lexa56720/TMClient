using ApiTypes.Communication.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TMApi;
using TMApi.ApiRequests.Friends;
using ApiWrapper.Interfaces;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    public class ClientApi : IApi
    {
        public UserInfo CurrentUser { get; private set; }
        public IUsersApi Users => users;
        private ClientUsersApi users;

        public IMessagesApi Messages => messages;
        private ClientMessagesApi messages;

        public IFriendsApi Friends => friends;
        private ClientFriendsApi friends;

        public IChatsApi Chats => chats;
        private ClientChatsApi chats;

        public ObservableCollection<Chat> Dialogs { get; private set; } = new();
        public ObservableCollection<Chat> MultiuserChats { get; private set; } = new();
        public ObservableCollection<User> FriendList { get; private set; } = new();
        public ObservableCollection<FriendRequest> FriendRequests { get; private set; } = new();
        public ObservableCollection<ChatInvite> ChatInvites { get; private set; } = new();


        private ApiConverter Converter { get; set; }
        private CacheManager Cache { get; set; }
        private LongPollManager LongPollManager { get; set; }

        private readonly Api Api;

        private bool IsDisposed = false;

        internal ClientApi(TimeSpan userLifetime, TimeSpan chatLifetime, Api api)
        {
            CurrentUser = api.UserInfo;
            Cache = new CacheManager(userLifetime, chatLifetime);
            Converter = new ApiConverter(this);

            chats = new ClientChatsApi(api, Converter, Cache);
            users = new ClientUsersApi(api, Converter, Cache, CurrentUser);

            messages = new ClientMessagesApi(api, Converter);
            friends = new ClientFriendsApi(api, Converter);

            LongPollManager = new LongPollManager(api.LongPolling, this);

            Api = api;
        }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                LongPollManager.Dispose();
                Api.Dispose();
                Cache.Clear();
                IsDisposed = true;
            }
        }


        public async Task Save(string path)
        {
            var bytes = Api.SerializeAuthData();
            var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            Directory.CreateDirectory(path);
            using var fs = File.Create(path);
            await fs.WriteAsync(protectedBytes);
        }
    }
}
