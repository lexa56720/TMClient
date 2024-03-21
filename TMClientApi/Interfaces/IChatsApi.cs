namespace ApiWrapper.Interfaces
{
    public interface IChatsApi
    {
        public Task<Chat?> CreateChat(string name, int[] membersId);
        public Task<Chat?> GetChat(int chatId);
        public Task<Chat[]> GetChat(int[] chatIds);
        public Task<Chat[]> GetAllChats();
        public Task<bool> SendChatInvite(int chatId,params int[] toUserId);
        public Task<ChatInvite?> GetChatInvite(int inviteId);
        public Task<ChatInvite[]> GetChatInvite(int[] inviteId);
        public Task<ChatInvite[]> GetAllInvites(int userId);
        public Task<bool> SendChatInviteResponse(int inviteId, bool isAccepted);
    }
}
