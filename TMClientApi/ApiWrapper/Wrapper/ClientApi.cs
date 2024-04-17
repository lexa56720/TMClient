using ApiTypes.Communication.Users;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TMApi;
using ClientApiWrapper.Interfaces;
using ClientApiWrapper.Types;
using TMApi.ApiRequests.Users;
using System;
using ClientApiWrapper.Wrapper;
using ClientApiWrapper;
using System.Net;
using System.Text;
using System.Linq;
using ApiTypes.Communication.Info;

namespace ClientApiWrapper.ApiWrapper.Wrapper
{
    public class ClientApi : IApi, IUserInfo
    {
        public User Info { get; private set; }

        public string PasswordHash { get; private set; } = string.Empty;

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
        internal readonly ClientUsersApi users;

        public IMessagesApi Messages => messages;
        private readonly ClientMessagesApi messages;

        public IFriendsApi Friends => friends;
        private readonly ClientFriendsApi friends;

        public IChatsApi Chats => chats;
        internal readonly ClientChatsApi chats;

        public IDataValidator DataValidator => dataValidator;
        internal readonly ClientDataValidator dataValidator;

        public ObservableCollection<Chat> MultiuserChats { get; private set; }
            = new OrderedObservableCollection<Chat>(new ChatComparer(), (c) => c, nameof(Chat.LastMessage));
        public ObservableCollection<Friend> FriendList { get; private set; }
            = new OrderedObservableCollection<Friend>(new FriendComparer(), (f) => f.Dialogue, nameof(Friend.Dialogue.LastMessage));


        public ObservableCollection<FriendRequest> FriendRequests { get; private set; } = new();
        public ObservableCollection<ChatInvite> ChatInvites { get; private set; } = new();

        private ApiConverter Converter { get; set; }
        private CacheManager Cache { get; set; }
        private LongPollManager LongPollManager { get; set; }
        public ServerInfo ServerInfo { get; }

        private readonly Api Api;

        private bool IsDisposed = false;

        private ClientApi(ServerInfo info, IPEndPoint fileServer, TimeSpan userLifetime,
                          TimeSpan chatLifetime, Api api, SynchronizationContext uiContext)
        {
            ApiConverter.FileServer = fileServer;
            Info = ApiConverter.Convert(api.UserInfo.MainInfo, true);

            Cache = new CacheManager(userLifetime, chatLifetime);
            Cache.AddToCache(Info);

            Converter = new ApiConverter(this, Cache);

            users = new ClientUsersApi(api, Converter, Cache, Info);
            messages = new ClientMessagesApi(api, Converter, uiContext);
            chats = new ClientChatsApi(api, Converter, Cache);
            friends = new ClientFriendsApi(api, Converter);
            dataValidator = new ClientDataValidator(info);
            LongPollManager = new LongPollManager(api.LongPolling, this, Cache, uiContext);
            users.PasswordChanged += UsersPasswordChanged;
            ServerInfo = info;
            Api = api;
        }



        internal static async Task<ClientApi?> Init(ServerInfo info, IPEndPoint fileServer, string passwordHash, TimeSpan userLifetime,
                                                    TimeSpan chatLifetime, Api api, SynchronizationContext uiContext)
        {
            var clientApi = new ClientApi(info, fileServer, userLifetime, chatLifetime, api, uiContext)
            {
                PasswordHash = passwordHash
            };

            var chats = await InitChats(clientApi, api.UserInfo.Chats);
            clientApi.Cache.AddOrUpdateCache(TimeSpan.MaxValue, chats);

            var friends = await InitFriends(clientApi, api.UserInfo.Friends, chats);
            clientApi.Cache.AddOrUpdateCache(TimeSpan.MaxValue, friends);

            var requests = await InitRequests(clientApi, api.UserInfo.FriendRequests);
            clientApi.Cache.AddOrUpdateCache(TimeSpan.MaxValue, requests.Select(r => r.From).ToArray());

            var invites = await InitInvites(clientApi, api.UserInfo.ChatInvites);
            clientApi.Cache.AddOrUpdateCache(TimeSpan.MaxValue, invites.Select(i => i.Chat).ToArray());
            clientApi.Cache.AddOrUpdateCache(TimeSpan.MaxValue, invites.Select(i => i.Inviter).ToArray());

            return clientApi;
        }
        private static async Task<Chat[]> InitChats(ClientApi api, int[] chatIds)
        {
            var chats = await api.Chats.GetChat(chatIds);
            for (int i = 0; i < chats.Length; i++)
            {
                if (!chats[i].IsDialogue)
                    api.MultiuserChats.Add(chats[i]);
            }
            return chats;
        }
        private static async Task<User[]> InitFriends(ClientApi api, ApiUser[] apiFriends, Chat[] chats)
        {
            var friends = await api.Users.GetUser(apiFriends.Select(f => f.Id).ToArray());
            foreach (var friend in friends)
                api.FriendList.Add(new Friend(friend, chats.Single(c => c.IsDialogue && c.Members.Any(m => m.Id == friend.Id))));
            return friends;
        }
        private static async Task<FriendRequest[]> InitRequests(ClientApi api, int[] requestIds)
        {
            var requests = await api.Friends.GetFriendRequest(requestIds);
            api.FriendRequests = new ObservableCollection<FriendRequest>(requests);
            return requests.ToArray();
        }
        private static async Task<ChatInvite[]> InitInvites(ClientApi api, int[] invitesIds)
        {
            var invites = await api.Chats.GetChatInvite(invitesIds);
            api.ChatInvites = new ObservableCollection<ChatInvite>(invites);
            return invites.ToArray();
        }


        public void Dispose()
        {
            if (IsDisposed)
                return;

            users.PasswordChanged -= UsersPasswordChanged;
            LongPollManager.Dispose();
            Api.Dispose();
            Cache.Clear();
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }
        private void UsersPasswordChanged(object? sender, string e)
        {
            PasswordHash = ApiTypes.Shared.HashGenerator.GenerateHashSmall(e);
        }
        public async Task Save(string path)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            bw.Write(Api.SerializeAuthData());

            var passwordBytes = Encoding.ASCII.GetBytes(PasswordHash);
            bw.Write(passwordBytes);
            bw.Write(passwordBytes.Length);

            await ms.FlushAsync();
            var bytes = ms.ToArray();
            var protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            using var fs = File.Create(path);
            await fs.WriteAsync(protectedBytes);
        }

        internal static string DeserializePasswordHash(byte[] unprotectedBytes)
        {
            using var ms = new MemoryStream(unprotectedBytes);
            using var br = new BinaryReader(ms);

            ms.Seek(-sizeof(int), SeekOrigin.End);
            var length = br.ReadInt32();
            ms.Seek(-(sizeof(int) + length), SeekOrigin.End);
            var hashBytes = br.ReadBytes(length);
            return Encoding.ASCII.GetString(hashBytes);
        }
    }
}
