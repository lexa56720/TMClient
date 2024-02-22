global using ApiMessage = ApiTypes.Communication.Messages.Message;
global using ApiUser = ApiTypes.Communication.Users.User;
global using ApiChat = ApiTypes.Communication.Chats.Chat;
global using ApiChatInvite = ApiTypes.Communication.Chats.ChatInvite;
global using ApiFriendRequest = ApiTypes.Communication.Friends.FriendRequest;
global using Chat = ApiWrapper.Types.Chat;
global using ChatInvite = ApiWrapper.Types.ChatInvite;
global using FriendRequest = ApiWrapper.Types.FriendRequest;
global using User = ApiWrapper.Types.User;
using ApiWrapper.Types;
using ApiWrapper.Interfaces;
using System;
using TMApi.ApiRequests.Users;
using TMApi.ApiRequests.Chats;

namespace ApiWrapper.ApiWrapper
{
    internal class ApiConverter
    {
        private readonly IChatsApi ChatApi;
        private readonly IUsersApi UserApi;

        public ApiConverter(IApi api)
        {
            ChatApi = api.Chats;
            UserApi = api.Users;
        }

        public User Convert(ApiUser user)
        {
            return new User(user.Id, user.Name, user.Login, user.IsOnline);
        }
        public User[] Convert(ApiUser[] users)
        {
            var result = new User[users.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert(users[i]);
            return result;
        }

        public async Task<Chat> Convert(ApiChat chat)
        {
            var result = new Chat(chat.Id, chat.Name, chat.IsDialogue);
            var members = await UserApi.GetUser(chat.MemberIds);
            foreach (var member in members)
                result.Members.Add(member);
            return result;
        }
        public async Task<Chat[]> Convert(ApiChat[] chats)
        {
            var result = new Chat[chats.Length];
            var members = chats.SelectMany(c => c.MemberIds).ToArray();
            var convertedMembers = await UserApi.GetUser(members);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Chat(chats[i].Id, chats[i].Name, chats[i].IsDialogue);
                for (int j = 0; j < chats[i].MemberIds.Length; j++)
                    result[i].Members.Add(convertedMembers[j]);
            }
            return result;
        }

        public Message Convert(ApiMessage message, User author, Chat chat)
        {
            return new Message(message.Id, message.Text, message.SendTime, author, chat);
        }
        public async Task<Message?> Convert(ApiMessage message)
        {
            if (message == null)
                return null;

            var user = await UserApi.GetUser(message.AuthorId);
            var chat = await ChatApi.GetChat(message.DestinationId);
            if (chat == null || user == null)
                return null;

            return Convert(message, user, chat);
        }
        public async Task<Message[]> Convert(ApiMessage[] messages)
        {
            var chats = await ChatApi.GetChat(messages.Select(m => m.DestinationId).ToArray());
            var authors = await UserApi.GetUser(messages.Select(m => m.AuthorId).ToArray());
            var result = new Message[messages.Length];

            for (int i = 0; i < messages.Length; i++)
                result[i] = Convert(messages[i], authors[i], chats[i]);

            return result;
        }

        public async Task<FriendRequest?> Convert(ApiFriendRequest request)
        {
            var user = await UserApi.GetUser(request.Id);
            if (user == null)
                return null;
            return Convert(request, user);
        }
        public FriendRequest Convert(ApiFriendRequest request, User user)
        {
            return new FriendRequest(request.Id, user);
        }
        public async Task<FriendRequest[]> Convert(ApiFriendRequest[] requests)
        {
            var users = await UserApi.GetUser(requests.Select(r => r.FromId).ToArray());
            var result = new FriendRequest[requests.Length];
            for (int i = 0; i < requests.Length; i++)
            {
                result[i] = Convert(requests[i], users[i]);
            }
            return result;
        }

        public async Task<ChatInvite?> Convert(ApiChatInvite invite)
        {
            var user = await UserApi.GetUser(invite.FromUserId);
            var chat = await ChatApi.GetChat(invite.ChatId);


            if (user == null || chat == null)
                return null;

            return new ChatInvite(invite.Id, user, chat);
        }

        public async Task<ChatInvite[]> Convert(ApiChatInvite[] invites)
        {
            var users = await UserApi.GetUser(invites.Select(i => i.FromUserId).ToArray());
            var chats = await ChatApi.GetChat(invites.Select(i => i.ChatId).ToArray());
            var result = new ChatInvite[invites.Length];

            for (int i = 0; i < invites.Length; i++)
                result[i] = new ChatInvite(invites[i].Id, users[i], chats[i]);

            return result;
        }
    }
}
