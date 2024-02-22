using ApiTypes.Communication.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi;
using ApiWrapper.Interfaces;
using ApiWrapper.Types;

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
            return await Converter.Convert(await Api.Messages.GetMessages(messagesId));
        }

        public async Task<Message?> SendMessage(string text, int destinationId)
        {
            var message = await Api.Messages.SendMessage(text, destinationId);
            if (message == null)
                return null;

            return await Converter.Convert(message);
        }
    }
}
