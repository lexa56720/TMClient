using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Types;

namespace TMClient.Model
{
    class NotificationsModel
    {
        public async Task AcceptFriend(FriendRequest request)
        {
            await App.Api.Friends.ResponseFriendRequest(request.Id, true);
        }
        public async Task DeclineFriend(FriendRequest request)
        {
            await App.Api.Friends.ResponseFriendRequest(request.Id, false);
        }

        public async Task AcceptInvite(ChatInvite invite)
        {
            await App.Api.Chats.SendChatInviteResponse(invite.Id, true);
        }
        public async Task DeclineInvite(ChatInvite invite)
        {
            await App.Api.Chats.SendChatInviteResponse(invite.Id, false);
        }
    }
}
