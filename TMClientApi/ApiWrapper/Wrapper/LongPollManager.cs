using TMApi.ApiRequests;
using ClientApiWrapper.Interfaces;
using ClientApiWrapper.Types;
using ApiTypes.Communication.Friends;
using ClientApiWrapper.ApiWrapper;
using TMApi.ApiRequests.Friends;
using ApiTypes.Communication.Chats;
using ClientApiWrapper.ApiWrapper.Wrapper;
using TMApi.ApiRequests.Chats;
using TMApi.ApiRequests.Users;

namespace ClientApiWrapper.Wrapper
{
    internal class LongPollManager : IDisposable
    {
        private bool IsDisposed;
        private readonly LongPolling LongPolling;
        private readonly ClientApi Api;
        private readonly CacheManager Cache;
        private readonly SynchronizationContext UIContext;

        public event EventHandler<Message[]>? NewMessages;
        public event EventHandler<int[]>? ReadedMessages;

        public LongPollManager(LongPolling longPolling, ClientApi api, CacheManager cache, SynchronizationContext uiContext)
        {
            Api = api;
            Cache = cache;
            UIContext = uiContext;
            LongPolling = longPolling;


            longPolling.NewChats += HandleNewChats;
            longPolling.RemovedChats += HandleRemovedChats;
            longPolling.RelatedUsersChanged += HandleRelatedUsersChanged;
            longPolling.NewChatInivites += HandleNewChatInivites;
            longPolling.ChatsChanged += HandleChatChanged;
            longPolling.NewMessages += HandleNewMessages;
            longPolling.NewFriendRequests += HandleNewFriendRequests;
            longPolling.FriendsRemoved += HandleFriendsRemoved;
            longPolling.MessagesReaded += HandleReadedMessages;
            longPolling.UserOnline += HandleUserOnline;
            longPolling.UserOffline += HandleUserOffline;

            longPolling.Start();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            LongPolling.Stop();
            LongPolling.NewMessages -= HandleNewMessages;
            LongPolling.NewFriendRequests -= HandleNewFriendRequests;
            LongPolling.FriendsRemoved -= HandleFriendsRemoved;
            LongPolling.NewChats -= HandleNewChats;
            LongPolling.NewChatInivites -= HandleNewChatInivites;
            LongPolling.MessagesReaded -= HandleReadedMessages;
            LongPolling.RemovedChats -= HandleRemovedChats;
            LongPolling.ChatsChanged -= HandleChatChanged;
            LongPolling.RelatedUsersChanged -= HandleRelatedUsersChanged;
            LongPolling.UserOnline -= HandleUserOnline;
            LongPolling.UserOffline -= HandleUserOffline;
            IsDisposed = true;
        }

        private void HandleUserOffline(object? sender, int[] e)
        {
            UIContext.Post(userIds =>
            {
                foreach (var id in (int[])userIds)
                    if (Cache.TryGetUser(id, out var user))
                    {
                        user.LastAction = DateTime.UtcNow;
                        user.IsOnline = false;
                    }
            }, e);
        }
        private void HandleUserOnline(object? sender, int[] e)
        {
            UIContext.Post(userIds =>
            {
                foreach (var id in (int[])userIds)
                    if (Cache.TryGetUser(id, out var user))
                        user.IsOnline = true;
            }, e);
        }


        private  void HandleNewChatInivites(object? sender, int[] e)
        {
            UIContext.Post(async state =>
            {
                var invites = await Api.chats.GetChatInviteIgnoringCache(e);
                foreach (var inivite in invites)
                    Api.ChatInvites.Add(inivite);
                Cache.AddOrUpdateCache(TimeSpan.MaxValue, (invites).Select(i => i.Inviter).ToArray());
                Cache.AddOrUpdateCache(TimeSpan.MaxValue, (invites).Select(i => i.Chat).ToArray());
            }, null);
        }

        private async void HandleRelatedUsersChanged(object? sender, int[] e)
        {
            var users = await Api.users.GetUserIgnoringCache(e);
            UIContext.Post(usersObj =>
            {
                Cache.AddOrUpdateCache(TimeSpan.MaxValue, (User[])usersObj);
            }, users);
        }

        private async void HandleChatChanged(object? sender, int[] e)
        {
            UIContext.Post(async state =>
            {
                var chats = await Api.chats.GetChatIgnoringCache(e, false);
                Cache.AddOrUpdateCache(TimeSpan.MaxValue,chats);
            }, null);
        }
        private void HandleRemovedChats(object? sender, int[] e)
        {
            UIContext.Post(chatIds =>
            {
                foreach (var chatId in (int[])chatIds)
                    if (Cache.TryRemoveChat(chatId, out var chat))
                    {
                        chat.IsReadOnly = true;
                        Api.MultiuserChats.Remove(chat);
                    }
            }, e);
        }

        private void HandleFriendsRemoved(object? sender, int[] e)
        {
            UIContext.Post(friendIds =>
            {
                foreach (var friendId in (int[])friendIds)
                    if (Cache.TryRemoveUser(friendId, out var friend))
                        Api.FriendList.Remove(Api.FriendList.Single(f => f.Id == friend.Id));
            }, e);
        }


        private async void HandleNewFriendRequests(object? sender, int[] e)
        {
            var friendRequests = await Api.Friends.GetFriendRequest(e);
            var senders = await Api.users.GetUserIgnoringCache(friendRequests.Select(f => f.From.Id).ToArray());
            UIContext.Post(requestObj =>
            {
                foreach (var request in (FriendRequest[])requestObj)
                    Api.FriendRequests.Add(request);
                Cache.AddOrUpdateCache(TimeSpan.MaxValue, senders);
            }, friendRequests);
        }

        private async void HandleNewMessages(object? sender, int[] e)
        {
            var messages = await Api.Messages.GetMessages(e);
            messages = messages.OrderBy(m => m.SendTime).ToArray();
            UIContext.Post(messagesObj =>
            {
                foreach (var message in (Message[])messagesObj)
                {
                    if (message is SystemMessage)
                        continue;

                    if (message.Destination.LastMessage == null ||
                        message.SendTime > message.Destination.LastMessage.SendTime)
                    {
                        message.Destination.LastMessage = message;
                    }
                    message.Destination.UnreadCount++;
                }
            }, messages);
            NewMessages?.Invoke(this, messages);
        }
        private void HandleReadedMessages(object? sender, int[] e)
        {
            var messages = Api.MultiuserChats.Union(Api.FriendList.Select(f => f.Dialogue))
                                             .Where(c => c.LastMessage != null && e.Contains(c.LastMessage.Id))
                                             .Select(m => m.LastMessage);

            foreach (var message in messages)
                message.IsReaded = true;

            ReadedMessages?.Invoke(this, e);
        }

        private void HandleNewChats(object? sender, int[] e)
        {
            UIContext.Post(async state =>
            {
                var chats = await Api.chats.GetChatIgnoringCache(e, true);
                foreach (var chat in chats)
                {
                    Cache.AddOrUpdateCache(TimeSpan.MaxValue, chat);
                    AddToCollections(chat);
                }
            }, null);
        }

        private void AddToCollections(Chat chat)
        {
            if (chat.IsDialogue)
            {
                User friend;
                if (chat.Members[0].IsCurrentUser)
                    friend = chat.Members[1];
                else
                    friend = chat.Members[0];

                Api.FriendList.Add(new Friend(friend, chat));
            }
            else
                Api.MultiuserChats.Add(chat);
        }
    }
}
