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

        public abstract Message[] GetMessages();

    }
}
