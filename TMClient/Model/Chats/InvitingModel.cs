using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Utils;

namespace TMClient.Model.Chats
{
    class InvitingModel : BaseModel
    {
        public async Task Invite(Chat chat, IEnumerable<User> users)
        {
            await Api.Chats.SendChatInvite(chat.Id, users.Select(u => u.Id).ToArray());
        }

        public User[] GetUsersToInivite(Chat chat, IEnumerable<Friend> friends)
        {
            var invitableFriends = new List<User>();
            var members = chat.Members.Select(m => m.Id);
            foreach (Friend friend in friends)
            {
                if (!members.Contains(friend.Id))
                    invitableFriends.Add(friend);
            }
            return invitableFriends.ToArray();
        }
        public UserContainer[] Search(IEnumerable<UserContainer> users, string query)
        {
            List<UserContainer> result = new List<UserContainer>();

            foreach (var user in users)
                if (user.User.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                   user.User.Login.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                {
                    result.Add(user);
                }
            return result.ToArray();
        }
    }
}
