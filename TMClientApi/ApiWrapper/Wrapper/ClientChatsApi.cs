using TMApi;
using ApiWrapper.Interfaces;
using TMApi.ApiRequests.Chats;
using TMApi.ApiRequests.Users;
using ApiWrapper.Types;
using ApiTypes.Communication.Chats;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    internal class ClientChatsApi : IChatsApi
    {
        private Api Api { get; }
        private ApiConverter Converter { get; }
        private CacheManager Cache { get; }

        internal ClientChatsApi(Api api, ApiConverter converter, CacheManager cacheManager)
        {
            Api = api;
            Converter = converter;
            Cache = cacheManager;
        }
        public async Task<Chat?> CreateChat(string name, int[] membersId)
        {
            var chat = await Api.Chats.CreateChat(name, membersId);
            if (chat == null)
                return null;

            var converted = await Converter.Convert(chat);
            if (converted == null)
                return null;

            Cache.AddToCache(TimeSpan.MaxValue, converted);
            return converted;
        }

        public async Task<Chat[]> GetAllChats()
        {
            var chatsIds = await Api.Chats.GetAllChats();
            return await GetChat(chatsIds);
        }

        public async Task<ChatInvite[]> GetAllInvites(int userId)
        {
            var inviteIds = await Api.Chats.GetAllInvites();
            return await GetChatInvite(inviteIds);
        }

        public async Task<Chat?> GetChat(int chatId)
        {
            if (Cache.TryGetChat(chatId, out var chat))
                return chat;
            var requestedChat = await Api.Chats.GetChat(chatId);
            if (requestedChat == null)
                return null;
            chat = await Converter.Convert(requestedChat);

            chat.LastMessage = await GetLastMessages(chatId);
            if (chat.LastMessage != null && chat.LastMessage.IsOwn)
                chat.UnreadCount = 0;

            Cache.AddToCache(chat);
            return chat;
        }

        public async Task<Chat[]> GetChat(int[] chatIds)
        {
            if (chatIds.Length == 0)
                return [];
            var result = new List<Chat>(chatIds.Length);
            var requestedChats = new List<int>();
            for (int i = 0; i < chatIds.Length; i++)
            {
                if (Cache.TryGetChat(chatIds[i], out var chat))
                    result.Add(chat);
                else
                    requestedChats.Add(chatIds[i]);
            }
            if (result.Count == chatIds.Length)
                return result.ToArray();

            var converted = await Converter.Convert(await Api.Chats.GetChat(requestedChats.ToArray()));
            if (converted.Length == 0)
                return [];
            Cache.AddToCache(converted);
            result.AddRange(converted);
            await AssingLastMessages(converted);
            return chatIds.Select(chatId => result.First(c => c.Id == chatId)).ToArray();
        }

        private async Task AssingLastMessages(IList<Chat> chats)
        {
            var lastMessages = await GetLastMessages(chats.Select(c => c.Id).ToArray());
            if (lastMessages == null)
                return;
            for (int i = 0; i < chats.Count; i++)
            {
                chats[i].LastMessage = lastMessages[i];
                if (lastMessages[i] != null && lastMessages[i].IsOwn)
                    chats[i].UnreadCount = 0;
            }
        }

        public async Task<ChatInvite?> GetChatInvite(int inviteId)
        {
            var invite = await Api.Chats.GetChatInvite(inviteId);
            if (invite == null)
                return null;
            return await Converter.Convert(invite);
        }

        public async Task<ChatInvite[]> GetChatInvite(int[] inviteIds)
        {
            if (inviteIds.Length == 0)
                return [];
            var invites = await Api.Chats.GetChatInvite(inviteIds);
            return await Converter.Convert(invites);
        }

        public async Task<bool> SendChatInvite(int chatId, params int[] toUserIds)
        {
            return await Api.Chats.SendChatInvite(chatId, toUserIds);
        }

        public async Task<bool> SendChatInviteResponse(int inviteId, bool isAccepted)
        {
            return await Api.Chats.SendChatInviteResponse(inviteId, isAccepted);
        }

        public async Task<bool> LeaveChat(int chatId)
        {
            return await Api.Chats.LeaveChat(chatId);
        }
        public async Task<bool> ChangeCover(int chatId, byte[] newCover)
        {
            return await Api.Chats.ChangeCover(chatId, newCover);
        }

        public async Task<bool> Rename(int chatId, string newName)
        {
            return await Api.Chats.RenameChat(chatId, newName);
        }
        public async Task<bool> KickUser(int chatId, int userId)
        {
            return await Api.Chats.KickUserFromChat(chatId, userId);
        }
        internal async Task<Chat[]> GetChatIgnoringCache(int[] chatIds)
        {
            var chats = await Api.Chats.GetChat(chatIds);
            if (chats.Length == 0)
                return [];

            var result = await Converter.Convert(chats);

            await AssingLastMessages(result);
            return result;
        }

        private async Task<Message?[]> GetLastMessages(params int[] chatIds)
        {
            var messages = await Api.Messages.GetMessagesForChats(chatIds);
            var converted = await Converter.Convert(messages.Where(m => m != null).ToArray());
            return chatIds.Select(id => converted.FirstOrDefault(m => m.Destination.Id == id))
                          .ToArray();
        }
        private async Task<Message?> GetLastMessages(int chatId)
        {
            var messages = await Api.Messages.GetMessagesForChats(chatId);
            if (messages.Length == 0 || messages == null)
                return null;
            var converted = await Converter.Convert(messages.Where(m => m != null).ToArray());
            return converted.FirstOrDefault();
        }
    }
}
