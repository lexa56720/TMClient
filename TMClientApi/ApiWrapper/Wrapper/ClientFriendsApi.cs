using TMApi;
using ClientApiWrapper.Interfaces;
using TMApi.API;

namespace ClientApiWrapper.ApiWrapper.Wrapper
{
    internal class ClientFriendsApi : IFriendsApi
    {
        private Api Api { get; }
        private ApiConverter Converter { get; }
        internal ClientFriendsApi(Api api, ApiConverter converter)
        {
            Api = api;
            Converter = converter;
        }

        public async Task<FriendRequest[]> GetAllRequests()
        {
            var requestIds = await Api.Friends.GetAllRequests();
            return await GetFriendRequest(requestIds);
        }

        public async Task<FriendRequest?> GetFriendRequest(int requestId)
        {
            var request = await Api.Friends.GetFriendRequest(requestId);
            if (request == null)
                return null;
            return await Converter.Convert(request);
        }

        public async Task<FriendRequest[]> GetFriendRequest(int[] requestIds)
        {
            var requests = await Api.Friends.GetFriendRequest(requestIds);
            if (requests.Length == 0)
                return [];
            return await Converter.Convert(requests);
        }

        public async Task<bool> ResponseFriendRequest(int requestId, bool isAccepted)
        {
            return await Api.Friends.ResponseFriendRequest(requestId, isAccepted);
        }

        public async Task<bool> SendFriendRequest(int toId)
        {
            return await Api.Friends.SendFriendRequest(toId);
        }

        public async Task<bool> RemoveFriend(int friendId)
        {
            return await Api.Friends.RemoveFriend(friendId);
        }
    }
}
