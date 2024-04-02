using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model
{
    internal class ChatMembersModel:BaseModel
    {
        public async Task SendRequest(User user)
        {
            await Api.Friends.SendFriendRequest(user.Id);
        }

        public async Task KickUser(User user, Chat chat)
        {
            await Api.Chats.KickUser( chat.Id, user.Id);
        }
    }
}
