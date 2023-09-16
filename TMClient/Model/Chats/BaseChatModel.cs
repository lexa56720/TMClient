using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Types;

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
            throw new NotImplementedException();
        }
        public async Task<Message[]> GetHistory(int offset)
        {
            throw new NotImplementedException();

        }

        public async Task SendMessage(Message message)
        {
            throw new NotImplementedException();
        }

    }
}
