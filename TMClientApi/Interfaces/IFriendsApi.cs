namespace ClientApiWrapper.Interfaces
{
    public interface IFriendsApi
    {
        public Task<FriendRequest?> GetFriendRequest(int requestId);
        public Task<FriendRequest[]> GetFriendRequest(int[] requestIds);
        public Task<FriendRequest[]> GetAllRequests();
        public Task<bool> ResponseFriendRequest(int requestId, bool isAccepted);
        public Task<bool> SendFriendRequest(int toId);
        public Task<bool> RemoveFriend(int toId);
    }
}
