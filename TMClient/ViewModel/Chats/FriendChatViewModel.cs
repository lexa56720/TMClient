using AsyncAwaitBestPractices.MVVM;
using System.Windows.Input;
using TMClient.Model.Chats;

namespace TMClient.ViewModel.Chats
{
    internal class FriendChatViewModel : BaseChatViewModel<ChatModel>
    {
        public Friend Friend { get; }

        public ICommand BlockCommand => new AsyncCommand(Block);
   
        public FriendChatViewModel(Friend friend) : base(friend.Dialogue)
        {
            Friend = friend;
        }

        protected override ChatModel GetModel(Chat chat)
        {
            return new ChatModel(chat,20);
        }

        private async Task Block()
        {
            throw new NotImplementedException();
        }
    }
}
