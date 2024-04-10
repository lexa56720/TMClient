using ClientApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model.Chats
{
    class MultiUserChatModel : ChatModel
    {
        public MultiUserChatModel(Chat chat, int count) : base(chat, count)
        {
        }

        public async Task<bool> LeaveChat()
        {
            return await Api.Chats.LeaveChat(Chat.Id);
        }
    }
}
