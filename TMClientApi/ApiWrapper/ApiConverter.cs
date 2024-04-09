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
using ClientApiWrapper.Types;
using System.Net;
using ApiTypes.Communication.Medias;
using TMApi;
using ApiTypes.Communication.Users;
using ApiWrapper.ApiWrapper.Wrapper;
using ApiTypes.Communication.Chats;
using TMApi.ApiRequests.Security;

namespace ApiWrapper.ApiWrapper
{
    internal class ApiConverter
    {
        private IChatsApi ChatApi => Api.Chats;
        private IUsersApi UserApi => Api.Users;
        private readonly ClientApi Api;
        private readonly CacheManager Cache;
        public static IPEndPoint? FileServer { get; set; }

        public ApiConverter(ClientApi api, CacheManager cache)
        {
            Api = api;
            Cache = cache;
        }

        public static User Convert(ApiUser user, bool isCurrentUser = false)
        {
            if (user.ProfilePics.Length < 3)
                return new User(user.Id, user.Name, user.Login, user.IsOnline, isCurrentUser, user.LastAction);

            var largePic = GetImageUrl(user.ProfilePics.SingleOrDefault(p => p.Size == ImageSize.Large));
            var mediumPic = GetImageUrl(user.ProfilePics.SingleOrDefault(p => p.Size == ImageSize.Medium));
            var smallPic = GetImageUrl(user.ProfilePics.SingleOrDefault(p => p.Size == ImageSize.Small));
            return new User(user.Id, user.Name, user.Login, user.IsOnline, isCurrentUser, user.LastAction, largePic, mediumPic, smallPic);
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
            var members = await UserApi.GetUser(chat.MemberIds);

            var largePic = GetImageUrl(chat.ChatCover.SingleOrDefault(p => p.Size == ImageSize.Large));
            var mediumPic = GetImageUrl(chat.ChatCover.SingleOrDefault(p => p.Size == ImageSize.Medium));
            var smallPic = GetImageUrl(chat.ChatCover.SingleOrDefault(p => p.Size == ImageSize.Small));

            var result = new Chat(chat.Id, chat.Name, members.Single(m => m.Id == chat.AdminId), chat.UnreadCount,
                                  chat.IsDialogue, largePic, mediumPic, smallPic);

            foreach (var member in members)
                result.Members.Add(member);

            return result;
        }
        public async Task<Chat[]> Convert(ApiChat[] chats, bool isIgnoreCache = false)
        {
            var result = new Chat[chats.Length];
            var members = chats.SelectMany(c => c.MemberIds).ToArray();
            var convertedMembers = isIgnoreCache ? await Api.users.GetUserIgnoringCache(members) : await UserApi.GetUser(members);
            for (int chatCount = 0, memberCount = 0; chatCount < result.Length; chatCount++)
            {
                var largePic = GetImageUrl(chats[chatCount].ChatCover.SingleOrDefault(p => p.Size == ImageSize.Large));
                var mediumPic = GetImageUrl(chats[chatCount].ChatCover.SingleOrDefault(p => p.Size == ImageSize.Medium));
                var smallPic = GetImageUrl(chats[chatCount].ChatCover.SingleOrDefault(p => p.Size == ImageSize.Small));

                var admin = convertedMembers.First(m => m.Id == chats[chatCount].AdminId);

                result[chatCount] = new Chat(chats[chatCount].Id, chats[chatCount].Name, admin,
                chats[chatCount].UnreadCount, chats[chatCount].IsDialogue, largePic, mediumPic, smallPic);

                for (int j = 0; j < chats[chatCount].MemberIds.Length; j++, memberCount++)
                    result[chatCount].Members.Add(convertedMembers[memberCount]);
                result[chatCount].Admin = result[chatCount].Members.Single(m => m.Id == chats[chatCount].AdminId);
            }
            return result;
        }

        public async ValueTask<Message> Convert(ApiMessage message, User author, Chat chat)
        {
            Cache.AddOrUpdateCache(TimeSpan.MaxValue, author);
            if (message.Kind == ApiTypes.Communication.Messages.ActionKind.None)
                return CreateMessage(message, author, chat);
            return await CreateSystemMessage(message, author, chat);
        }
        public async Task<Message?> Convert(ApiMessage message)
        {
            if (message == null)
                return null;

            var user = await UserApi.GetUser(message.AuthorId);
            var chat = await ChatApi.GetChat(message.DestinationId);
            if (chat == null || user == null)
                return null;

            return await Convert(message, user, chat);
        }
        public async Task<Message[]> Convert(ApiMessage[] messages)
        {
            var chats = await ChatApi.GetChat(messages.Select(m => m.DestinationId).ToArray());
            var authors = await UserApi.GetUser(messages.Select(m => m.AuthorId).ToArray());
            var result = new Message[messages.Length];

            for (int i = 0; i < messages.Length; i++)
                result[i] = await Convert(messages[i], authors[i], chats[i]);

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
                result[i] = Convert(requests[i], users[i]);

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

        public async Task<ChatInvite[]> Convert(ApiChatInvite[] invites, bool isIgnoreCache = false)
        {
            User[] users;
            Chat[] chats;
            if (isIgnoreCache)
            {
                users = await Api.users.GetUserIgnoringCache(invites.Select(i => i.FromUserId).ToArray());
                chats = await Api.chats.GetChatIgnoringCache(invites.Select(i => i.ChatId).ToArray(), true);
            }
            else
            {
                users = await UserApi.GetUser(invites.Select(i => i.FromUserId).ToArray());
                chats = await ChatApi.GetChat(invites.Select(i => i.ChatId).ToArray());
            }

            var result = new ChatInvite[invites.Length];

            for (int i = 0; i < invites.Length; i++)
                result[i] = new ChatInvite(invites[i].Id, users[i], chats[i]);

            return result;
        }


        private async Task<Message> CreateSystemMessage(ApiMessage message, User author, Chat chat)
        {
            User? target = null;
            bool isTargetCurrentUser = false;
            if (message.TargetId > 0)
                target = await Api.Users.GetUser(message.TargetId);

            if (target != null)
            {
                Cache.AddOrUpdateCache(TimeSpan.MaxValue, target);
                isTargetCurrentUser = target.IsCurrentUser;
            }
            return new SystemMessage()
            {
                Id = message.Id,
                Author = author,
                Destination = chat,
                IsOwn = author.IsCurrentUser,
                IsReaded = message.IsReaded,
                Text = message.Text,
                SendTime = message.SendTime,
                Attachments = [],
                Kind = message.Kind,
                Target = target,
                IsTargetAreCurrentUser = isTargetCurrentUser,
                IsExecutorAreCurrentUser = author.IsCurrentUser
            };
        }
        private Message CreateMessage(ApiMessage message, User author, Chat chat)
        {
            return new Message()
            {
                Id = message.Id,
                Author = author,
                Destination = chat,
                IsOwn = author.IsCurrentUser,
                IsReaded = message.IsReaded,
                Text = message.Text,
                SendTime = message.SendTime,
                Attachments = Convert(message.Photos, message.Files)
            };
        }

        private static Attachment[] Convert(PhotoLink[] photos, FileLink[] files)
        {
            var attachments = new Attachment[photos.Length + files.Length];
            for (int imageCount = 0, fileCount = 0; imageCount < attachments.Length; imageCount++)
            {
                if (imageCount < photos.Length)
                {
                    var url = GetImageUrl(photos[imageCount]);   
                    var imageName = photos[imageCount].Url.Split('/').Skip(1).FirstOrDefault();
                    if (string.IsNullOrEmpty(imageName) || string.IsNullOrEmpty(url))
                        continue;
                    attachments[imageCount] = new ImageAttachment(imageName[0..16], url);
                }
                else
                {
                    var url = GetFileUrl(files[fileCount]);
                    attachments[imageCount] = new FileAttachment(files[fileCount].Name, url);
                    fileCount++;
                }
            }
            return attachments;
        }
        private static string? GetFileUrl(FileLink? file)
        {
            if (file == null)
                return null;
            return $"http://{FileServer?.ToString()}/{file.Url}";
        }
        private static string? GetImageUrl(PhotoLink? photo)
        {
            if (photo == null)
                return null;
            return $"http://{FileServer?.ToString()}/{photo.Url}";
        }
    }
}
