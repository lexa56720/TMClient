using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Model.Chats;
using TMClient.Types;

namespace TMClient.ViewModel.Chats
{
    internal class FriendChatViewModel : BaseChatViewModel<FriendChatModel>
    {
        public FriendChatViewModel(Chat chat) : base(chat)
        {
            ChatName = Chat.Members.Single(m => m.Id != App.Api.Id).Name;
        }

        protected override FriendChatModel GetModel(Chat chat)
        {
            return new FriendChatModel(chat);
        }
    }
}
