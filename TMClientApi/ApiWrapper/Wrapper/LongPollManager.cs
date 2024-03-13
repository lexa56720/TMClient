﻿using TMApi.ApiRequests;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;
using ApiTypes.Communication.Friends;
using ClientApiWrapper.Types;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    internal class LongPollManager : IDisposable
    {
        private bool IsDisposed;
        private readonly LongPolling LongPolling;
        private readonly IApi Api;
        private readonly SynchronizationContext UIContext;

        public event EventHandler<Message[]>? NewMessages;
        public event EventHandler<int[]>? ReadedMessages;

        public LongPollManager(LongPolling longPolling, IApi api, SynchronizationContext uiContext)
        {
            Api = api;
            UIContext = uiContext;
            LongPolling = longPolling;


            longPolling.NewChats += HandleNewChats;
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
            LongPolling.MessagesReaded -= HandleReadedMessages;

            IsDisposed = true;
        }
        private void HandleFriendsRemoved(object? sender, int[] e)
        {
            var friends = Api.FriendList.Where(f => e.Contains(f.Id)).ToArray();
            UIContext.Post(friendsObj =>
            {
                foreach (var friend in (Friend[])friendsObj)
                    Api.FriendList.Remove(friend);
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
            var messages = await Api.Messages.GetMessages(e);

            UIContext.Post(messagesObj =>
            {
                foreach (var message in (Message[])messagesObj)
                {
                    if (message.Destination.LastMessage == null ||
                        message.SendTime > message.Destination.LastMessage.SendTime)
                    {
                        message.Destination.LastMessage = message;
                    }
                    message.Destination.UnreadedCount++;
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
                    if (chat.IsDialogue)
                    {
                        var friend = chat.Members[0].Id == Api.Info.Id ? chat.Members[1] : chat.Members[0];
                        Api.FriendList.Add(new Friend(friend, chat));
                    }
                    else
                        Api.MultiuserChats.Add(chat);
            }, chats);
        }
    }
}
