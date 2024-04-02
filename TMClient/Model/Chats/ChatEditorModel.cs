using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model.Chats
{
    internal class ChatEditorModel : BaseModel
    {
        internal async Task ChangeCover(Chat chat, byte[] imageData)
        {
            await Api.Chats.ChangeCover(chat.Id, imageData);
        }

        internal async Task ChangeName(Chat chat, string newName)
        {
            await Api.Chats.Rename(chat.Id, newName);
        }

        public async Task KickUser(User user, Chat chat)
        {
            await Api.Chats.KickUser(chat.Id, user.Id);
        }
    }
}
