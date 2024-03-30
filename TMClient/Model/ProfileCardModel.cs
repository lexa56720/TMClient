using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model
{
    class ProfileCardModel : BaseModel
    {
        public async Task<bool> AddToFriend(User user)
        {
            return await Api.Friends.SendFriendRequest(user.Id);
        }
        public bool IsAlreadyFriend(User user)
        {
            if(user.IsCurrentUser || Api.FriendList.Any(f=>f.Id==user.Id))
                return true;
            return false;
        }
    }
}
