using ClientApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model.Chats
{
    internal class FriendChatModel : ChatModel
    {
        public FriendChatModel(Chat chat, int count) : base(chat, count)
        {
        }

        public async Task<bool> RemoveFriend(Friend friend)
        {
            return await Api.Friends.RemoveFriend(friend.Id);
        }
    }
}
