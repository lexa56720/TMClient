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
        public async Task<Message?[]> GetLastMessages(params int[] chatIds)
        {
            var messages = await Api.Messages.GetMessagesForChats(chatIds);
            var converted = await Converter.Convert(messages.Where(m=>m!=null).ToArray());
            return chatIds.Select(id => converted.FirstOrDefault(m => m.Destination.Id == id))
                          .ToArray();
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
            convertedMessage.Destination.UnreadCount=0;

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
