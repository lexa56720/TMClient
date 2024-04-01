using AsyncAwaitBestPractices.MVVM;
using System.Windows.Input;
using TMClient.Model.Chats;

namespace TMClient.ViewModel.Chats
{
    internal class FriendChatViewModel : BaseChatViewModel<FriendChatModel>
    {
        public Friend Friend { get; }

        public ICommand BlockCommand => new AsyncCommand(Block);


        public FriendChatViewModel(Friend friend) : base(friend.Dialogue)
        {
            Friend = friend;
        }

        protected override FriendChatModel GetModel(Chat chat)
        {
            return new FriendChatModel(chat, 20);
        }

        private async Task Block()
        {
            if (await Model.RemoveFriend(Friend))
                Friend.Dialogue.IsReadOnly = true;
        }
    }
}
