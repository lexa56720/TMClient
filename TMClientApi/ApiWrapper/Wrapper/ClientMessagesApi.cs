using TMApi;
using ClientApiWrapper.Interfaces;
using ClientApiWrapper.Types;
using TMApi.ApiRequests.Messages;
using ApiTypes.Communication.BaseTypes;

namespace ClientApiWrapper.ApiWrapper.Wrapper
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
        public async Task<Message[]> GetMessagesByOffset(int chatId, int count, int offset)
        {
            var messages = await Api.Messages.GetMessagesByOffset(chatId, count, offset);
            if (messages.Length == 0)
                return [];
            return await Converter.Convert(messages);
        }

        public async Task<Message[]> GetMessages(int chatId, int fromMessageId, int count)
        {
            var messages = await Api.Messages.GetMessages(chatId, fromMessageId, count);
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
        public async Task<Message?> SendMessage(string text, int destinationId, CancellationToken token, string[] filePaths)
        {
            var result = new SerializableFile[filePaths.Length];
            try
            {
                await Parallel.ForAsync(0, filePaths.Length, token, async (i, token) =>
                {
                    var bytes = await File.ReadAllBytesAsync(filePaths[i], token);
                    var fileName = Path.GetFileName(filePaths[i]);
                    result[i] = new SerializableFile(fileName, bytes);
                });
            }
            catch
            {
                return null;
            }

            var message = await Api.Messages.SendMessage(text, destinationId, result);
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
