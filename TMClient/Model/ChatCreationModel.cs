using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TMClient.Model
{
    class ChatCreationModel : BaseModel
    {
        public async Task<Chat> CreateChat(IEnumerable<User> users, string name)
        {
            var result = await Api.Chats.CreateChat(name, users.Select(u => u.Id).ToArray());
            return result;
        }

        public bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && ApiTypes.Shared.DataConstraints.IsNameLegal(name);
        }
    }
}
