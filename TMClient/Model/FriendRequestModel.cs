namespace TMClient.Model
{
    class FriendRequestModel: BaseModel
    {
        public async Task<User[]> SearchByName(string name)
        {
            return await Api.Users.GetByName(name);
        }

        public async Task SendRequest(User user)
        {
            await Api.Friends.SendFriendRequest(user.Id);
        }
    }
}
