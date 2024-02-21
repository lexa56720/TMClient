global using ApiMessage = ApiTypes.Communication.Messages.Message;
global using ApiUser = ApiTypes.Communication.Users.User;
global using ApiChat = ApiTypes.Communication.Chats.Chat;
global using ApiFriendRequest = ApiTypes.Communication.Friends.FriendRequest;
global using Chat = TMClientApi.Types.Chat;
global using ChatInvite = TMClientApi.Types.ChatInvite;
global using FriendRequest = TMClientApi.Types.FriendRequest;
global using User = TMClientApi.Types.User;
using TMClientApi.Types;
using TMClientApi.InternalApi;
using System;
using TMApi.ApiRequests.Users;

namespace TMClientApi.ApiWrapper
{
    internal class ApiConverter
    {
        private readonly IChatsApi ChatApi;
        private readonly IUsersApi UserApi;

        public ApiConverter(IChatsApi chatsApi, IUsersApi usersApi)
        {
            ChatApi = chatsApi;
            UserApi = usersApi;
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

        public Chat Convert(ApiChat chat)
        {
            return new Chat(chat.Id, chat.Name);
        }
        public Chat[] Convert(ApiChat[] chats)
        {
            var result = new Chat[chats.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert(chats[i]);
            return result;
        }

        public Message Convert(ApiMessage message, User author, Chat chat)
        {
            return new Message(message.Id, message.Text, message.SendTime, author, chat);
        }
        public async ValueTask<Message?> Convert(ApiMessage message)
        {
            if (message == null)
                return null;

            var user = await UserApi.GetUser(message.AuthorId);
            var chat = await ChatApi.GetChat(message.DestinationId);
            if (chat == null || user == null)
                return null;

            return Convert(message, user, chat);
        }
        public async ValueTask<Message[]> Convert(ApiMessage[] messages)
        {
            var chats = await ChatApi.GetChat(messages.Select(m => m.DestinationId).ToArray());
            var authors = await UserApi.GetUser(messages.Select(m => m.AuthorId).ToArray());
            var result = new Message[messages.Length];

            for (int i = 0; i < messages.Length; i++)
                result[i] = Convert(messages[i], authors[i], chats[i]);

            return result;
        }


        public async ValueTask<FriendRequest?> Convert(ApiFriendRequest request)
        {
            var user = await UserApi.GetUser(request.Id);
            if (user == null) return null;
            return Convert(request, user);
        }
        public FriendRequest Convert(ApiFriendRequest request, User user)
        {
            return new FriendRequest(request.Id, user);
        }
        public async ValueTask<FriendRequest[]> Convert(ApiFriendRequest[] requests)
        {
            var users = await UserApi.GetUser(requests.Select(r=>r.FromId).ToArray());
            var result = new FriendRequest[requests.Length];
            for (int i = 0; i < requests.Length; i++)
            {
                result[i]= Convert(requests[i], users[i]);
            }
            return result;
        }
    }
}
