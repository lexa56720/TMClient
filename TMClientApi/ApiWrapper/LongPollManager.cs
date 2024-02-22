using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi.ApiRequests;
using TMClientApi.InternalApi;
using TMClientApi.Types;

namespace TMClientApi.ApiWrapper
{
    internal class LongPollManager : IDisposable
    {
        private bool IsDisposed;
        private readonly LongPolling LongPolling;
        private readonly IApi Api;

        public event EventHandler<Message[]>? NewMessages;

        public LongPollManager(LongPolling longPolling, IApi api)
        {
            longPolling.NewMessages += HandleNewMessages;
            longPolling.NewFriendRequests += HandleNewFriendRequests;
            longPolling.FriendsAdded += HandleFriendsAdded;
            longPolling.FriendsRemoved += HandleFriendsRemoved;
            longPolling.Start();

            LongPolling = longPolling;
            Api = api;
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

                NewMessages = null;
                IsDisposed = true;
            }
        }
        private async void HandleFriendsRemoved(object? sender, int[] e)
        {
            var friends = await Api.Users.GetUser(e);
            foreach (var friend in friends)
                Api.FriendList.Remove(friend);
        }

        private async void HandleFriendsAdded(object? sender, int[] e)
        {
            var friends = await Api.Users.GetUser(e);
            foreach (var friend in friends)
                Api.FriendList.Add(friend);
        }

        private async void HandleNewFriendRequests(object? sender, int[] e)
        {
            var friendRequests = await Api.Friends.GetFriendRequest(e);
            foreach (var request in friendRequests)
                Api.FriendRequests.Add(request);
        }

        private async void HandleNewMessages(object? sender, int[] e)
        {
            if (NewMessages == null)
                return;
            NewMessages.Invoke(this, await Api.Messages.GetMessages(e));
        }
    }
}
