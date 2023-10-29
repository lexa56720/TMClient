using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Types;
using TMClient.Utils;

namespace TMClient.Model.Chats
{
    internal abstract class BaseChatModel
    {
        protected Chat Chat { get; }

        public BaseChatModel(Chat chat)
        {
            Chat = chat;
        }

        public async Task<Message[]> GetHistory(Message lastMessage)
        {
            var messages = await App.Api.Messages.GetMessages(Chat.Id, lastMessage.Id);
            return await ApiConverter.Convert(App.UserData, messages);
        }
        public async Task<Message[]> GetHistory(int offset)
        {
            var messages = await App.Api.Messages.GetMessages(Chat.Id, 20, offset);
            return await ApiConverter.Convert(App.UserData, messages);
        }

        public async Task<Message?> SendMessage(Message message)
        {
            var result = await App.Api.Messages.SendMessage(message.Text, message.Destionation.Id);
            if (result == null)
                return null;
            return new Message(result, App.CurrentUser,message.Destionation);
        }

    }
}
