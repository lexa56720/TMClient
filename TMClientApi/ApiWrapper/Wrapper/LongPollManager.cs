using TMApi.ApiRequests;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;
using ApiTypes.Communication.Friends;
using ApiWrapper.ApiWrapper;
using TMApi.ApiRequests.Friends;
using ApiTypes.Communication.Chats;
using ApiWrapper.ApiWrapper.Wrapper;
using TMApi.ApiRequests.Chats;
using ClientApiWrapper.Types;
using TMApi.ApiRequests.Users;

namespace ApiWrapper.Wrapper
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

            IsDisposed = true;
        }


        private async void HandleNewChatInivites(object? sender, int[] e)
        {
            var invites = await Api.Chats.GetChatInvite(e);
            UIContext.Post(invitesObj =>
            {
                foreach (var inivite in (ChatInvite[])invitesObj)
                    Api.ChatInvites.Add(inivite);
            }, invites);
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
            var chats = await Api.chats.GetChatIgnoringCache(e);
            UIContext.Post(chatsObj =>
            {
                Cache.AddOrUpdateCache(TimeSpan.MaxValue, (Chat[])chatsObj);
            }, chats);
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
                        chat.Dispose();
                    }
            }, e);
        }

        private void HandleFriendsRemoved(object? sender, int[] e)
        {
            UIContext.Post(friendIds =>
            {
                foreach (var friendId in (int[])friendIds)
                    if (Cache.TryRemoveUser(friendId, out var friend))
                        Api.FriendList.Remove(Api.FriendList.Single(f=>f.Id==friend.Id));
            }, e);
        }


        private async void HandleNewFriendRequests(object? sender, int[] e)
        {
            var friendRequests = await Api.Friends.GetFriendRequest(e);
            UIContext.Post(requestObj =>
            {
                foreach (var request in (FriendRequest[])requestObj)
                    Api.FriendRequests.Add(request);
                Cache.AddOrUpdateCache(friendRequests.Select(f => f.From).ToArray());
            }, friendRequests);
        }

        private async void HandleNewMessages(object? sender, int[] e)
        {
            var messages = await Api.Messages.GetMessages(e);

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

        private async void HandleNewChats(object? sender, int[] e)
        {
            var chats = await Api.Chats.GetChat(e);
            UIContext.Post(chatsObj =>
            {
                foreach (var chat in (Chat[])chatsObj)
                {
                    if (Cache.AddToCache(TimeSpan.MaxValue, chat))
                        AddToCollections(chat);
                    else if (Cache.TryGetChat(chat.Id, out var updatedChat))
                        AddToCollections(updatedChat);
                }
            }, chats);
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
