using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi;
using TMClient.Types;

namespace TMClient.Model
{
    class FriendRequestModel
    {
        public async Task<User[]> SearchByName(string name)
        {
            var users = await App.Api.Users.GetByName(name);

            return users.Select(u => new User(u)).ToArray();
        }

        public async Task SendRequest(User user)
        {
            await App.Api.Friends.SendFriendRequest(user.Id);
        }
    }
}
