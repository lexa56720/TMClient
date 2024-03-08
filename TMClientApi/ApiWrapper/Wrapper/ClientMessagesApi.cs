using TMApi;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;
using TMApi.ApiRequests.Messages;

namespace ApiWrapper.ApiWrapper.Wrapper
{
    internal class ClientMessagesApi : IMessagesApi
    {
        private Api Api { get; }
        private ApiConverter Converter { get; }

        internal ClientMessagesApi(Api api, ApiConverter converter)
        {
            Api = api;
            Converter = converter;
        }
        public async Task<Message[]> GetMessages(int chatId, int count, int offset)
        {
            return await Converter.Convert(await Api.Messages.GetMessages(chatId, count, offset));
        }

        public async Task<Message[]> GetMessages(int chatId, int fromMessageId)
        {
            return await Converter.Convert(await Api.Messages.GetMessages(chatId, fromMessageId));
        }

        public async Task<Message[]> GetMessages(params int[] messagesId)
        {
            if (messagesId == null)
                return [];
            return await Converter.Convert(await Api.Messages.GetMessages(messagesId));
        }

        public async Task<Message?> SendMessage(string text, int destinationId)
        {
            var message = await Api.Messages.SendMessage(text, destinationId);
            if (message == null)
                return null;

            return await Converter.Convert(message);
        }

        public async Task<bool> MarkAsReaded(params Message[] messages)
        {
            if (messages.Length == 0)
                return true;

            var ids = messages.Where(m => !m.IsReaded)
                              .Select(m => m.Id)
                              .ToArray();

            var result = await Api.Messages.MarkAsReaded(ids);
            if (result)
                messages.ToList().ForEach(m => m.IsReaded = true);
            return result;
        }
    }
}
