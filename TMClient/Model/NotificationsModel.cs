namespace TMClient.Model
{
    class NotificationsModel: BaseModel
    {
        public async Task AcceptFriend(FriendRequest request)
        {
            await Api.Friends.ResponseFriendRequest(request.Id, true);
        }
        public async Task DeclineFriend(FriendRequest request)
        {
            await Api.Friends.ResponseFriendRequest(request.Id, false);
        }

        public async Task AcceptInvite(ChatInvite invite)
        {
            await Api.Chats.SendChatInviteResponse(invite.Id, true);
        }
        public async Task DeclineInvite(ChatInvite invite)
        {
            await Api.Chats.SendChatInviteResponse(invite.Id, false);
        }
    }
}
