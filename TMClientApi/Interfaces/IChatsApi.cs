namespace ApiWrapper.Interfaces
{
    public interface IChatsApi
    {
        public Task<Chat?> CreateChat(string name, int[] membersId);
        public ValueTask<Chat?> GetChat(int chatId);
        public ValueTask<Chat[]> GetChat(int[] chatIds);
        public Task<Chat[]> GetAllChats();
        public Task<bool> SendChatInvite(int chatId, int toUserId);
        public Task<ChatInvite?> GetChatInvite(int inviteId);
        public Task<ChatInvite[]> GetChatInvite(int[] inviteId);
        public Task<ChatInvite[]> GetAllInvites(int userId);
        public Task<bool> SendChatInviteResponse(int inviteId, bool isAccepted);
    }
}
