using TMClient.Model.Chats;

namespace TMClient.ViewModel.Chats
{
    internal class FriendChatViewModel : BaseChatViewModel<FriendChatModel>
    {
        public string FriendLogin
        {
            get => friendLogin;
            set
            {
                friendLogin = value;
                OnPropertyChanged(nameof(FriendLogin));
            }
        }
        private string friendLogin;

        public FriendChatViewModel(Chat chat) : base(chat)
        {
            var friend = Chat.Members.Single(m => m.Id != CurrentUser.Info.Id);
            ChatName = friend.Name;
            FriendLogin = friend.Login;
        }

        protected override FriendChatModel GetModel(Chat chat)
        {
            return new FriendChatModel(chat);
        }
    }
}
