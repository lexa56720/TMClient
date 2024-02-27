using TMApi;
using ApiWrapper.Interfaces;
using TMApi.ApiRequests.Chats;

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

            Cache.AddToCache(converted);
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

        public async ValueTask<Chat?> GetChat(int chatId)
        {
            if (Cache.TryGetChat(chatId, out var chat))
                return chat;
            var requestedChat = await Api.Chats.GetChat(chatId);
            chat = await Converter.Convert(requestedChat);
            Cache.AddToCache(chat);
            return chat;
        }

        public async ValueTask<Chat[]> GetChat(int[] chatIds)
        {
            if (chatIds == null)
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
            var converted = await Converter.Convert(await Api.Chats.GetChat(requestedChats.Distinct().ToArray()));
            Cache.AddToCache(converted);
            result.AddRange(converted);

            return chatIds.Select(chatId => result.First(c => c.Id == chatId)).ToArray();
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
            if (inviteIds == null)
                return [];
            var invites = await Api.Chats.GetChatInvite(inviteIds);
            return await Converter.Convert(invites);
        }

        public async Task<bool> SendChatInvite(int chatId, int toUserId)
        {
            return await Api.Chats.SendChatInvite(chatId, toUserId);
        }

        public async Task<bool> SendChatInviteResponse(int inviteId, bool isAccepted)
        {
            return await Api.Chats.SendChatInviteResponse(inviteId, isAccepted);
        }
    }
}
