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
            var messages = await Api.Messages.GetMessages(chatId, count, offset);
            if (messages.Length == 0)
                return [];
            return await Converter.Convert(messages);
        }

        public async Task<Message[]> GetMessages(int chatId, int fromMessageId)
        {
            var messages = await Api.Messages.GetMessages(chatId, fromMessageId);
            if (messages.Length == 0)
                return [];
            return await Converter.Convert(messages);
        }

        public async Task<Message[]> GetMessages(params int[] messagesId)
        {
            var messages = await Api.Messages.GetMessages(messagesId);
            if (messages.Length == 0)
                return [];
            return await Converter.Convert(messages);
        }
        public async Task<Message?[]> GetLastMessages(params int[] chatIds)
        {
            var messages = await Api.Messages.GetMessagesForChats(chatIds);
            if (messages.Length == 0)
                return [];
            var converted = await Converter.Convert(messages.Where(m => m != null).ToArray());
            return chatIds.Select(id => converted.FirstOrDefault(m => m.Destination.Id == id))
                          .ToArray();
        }
        public async Task<Message?> GetLastMessages(int chatId)
        {
            var messages = await Api.Messages.GetMessagesForChats(chatId);
            if (messages.Length == 0 || messages == null)
                return null;
            var converted = await Converter.Convert(messages.Where(m => m != null).ToArray());
            return converted.FirstOrDefault();
        }
        public async Task<Message?> SendMessage(string text, int destinationId)
        {
            var message = await Api.Messages.SendMessage(text, destinationId);
            if (message == null)
                return null;

            var convertedMessage = await Converter.Convert(message);
            if (convertedMessage == null)
                return null;

            if (convertedMessage.Destination.LastMessage == null ||
                convertedMessage.SendTime > convertedMessage.Destination.LastMessage.SendTime)
            {
                convertedMessage.Destination.LastMessage = convertedMessage;
            }
            convertedMessage.Destination.UnreadCount = 0;

            return convertedMessage;
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
                foreach (var message in messages)
                {
                    message.IsReaded = true;
                    message.Destination.UnreadCount--;
                }
            return result;
        }
    }
}
