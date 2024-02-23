using TMApi.ApiRequests;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;
using ApiTypes.Communication.Friends;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    internal class LongPollManager : IDisposable
    {
        private bool IsDisposed;
        private readonly LongPolling LongPolling;
        private readonly IApi Api;
        private readonly SynchronizationContext UIContext;

        public event EventHandler<Message[]>? NewMessages;

        public LongPollManager(LongPolling longPolling, IApi api, SynchronizationContext uiContext)
        {
            Api = api;
            UIContext = uiContext;
            LongPolling = longPolling;


            LongPolling.NewChats += HandleNewChats;
            longPolling.NewMessages += HandleNewMessages;
            longPolling.NewFriendRequests += HandleNewFriendRequests;
            longPolling.FriendsAdded += HandleFriendsAdded;
            longPolling.FriendsRemoved += HandleFriendsRemoved;
            longPolling.Start();
        }
        public void Dispose()
        {
            if (!IsDisposed)
            {
                LongPolling.Stop();
                LongPolling.NewMessages -= HandleNewMessages;
                LongPolling.NewFriendRequests -= HandleNewFriendRequests;
                LongPolling.FriendsAdded -= HandleFriendsAdded;
                LongPolling.FriendsRemoved -= HandleFriendsRemoved;
                LongPolling.NewChats -= HandleNewChats;

                NewMessages = null;
                IsDisposed = true;
            }
        }
        private async void HandleFriendsRemoved(object? sender, int[] e)
        {
            var friends = await Api.Users.GetUser(e);
            UIContext.Post(friendsObj =>
            {
                foreach (var friend in (User[])friendsObj)
                    Api.FriendList.Remove(friend);
            }, friends);
        }

        private async void HandleFriendsAdded(object? sender, int[] e)
        {
            var friends = await Api.Users.GetUser(e);
            UIContext.Post(friendsObj =>
            {
                foreach (var friend in (User[])friendsObj)
                    Api.FriendList.Add(friend);
            }, friends);
        }

        private async void HandleNewFriendRequests(object? sender, int[] e)
        {
            var friendRequests = await Api.Friends.GetFriendRequest(e);
            UIContext.Post(requestObj =>
            {
                foreach (var request in (FriendRequest[])requestObj)
                    Api.FriendRequests.Add(request);
            }, friendRequests);
        }

        private async void HandleNewMessages(object? sender, int[] e)
        {
            if (NewMessages == null)
                return;
            NewMessages.Invoke(this, await Api.Messages.GetMessages(e));
        }

        private async void HandleNewChats(object? sender, int[] e)
        {
            var chats = await Api.Chats.GetChat(e);
            UIContext.Post(chatsObj =>
            {
                foreach (var chat in (Chat[])chatsObj)
                    if (chat.IsDialogue)
                        Api.Dialogs.Add(chat);
                    else
                        Api.MultiuserChats.Add(chat);
            }, chats);
        }
    }
}
