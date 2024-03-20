using ApiWrapper.Types;
using AsyncAwaitBestPractices.MVVM;
using System.Windows.Input;
using TMClient.Model.Chats;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Chats
{
    internal class ChatViewModel : BaseChatViewModel<ChatModel>
    {
        public int MemberCount
        {
            get => memberCount;
            set
            {
                memberCount = value;
                OnPropertyChanged(nameof(MemberCount));
            }
        }
        private int memberCount;


        public ICommand LeaveCommand => new AsyncCommand(LeaveChat);
        public ICommand InviteCommand => new AsyncCommand(InviteToChat);
        public ICommand ShowMembersCommand => new AsyncCommand(ShowMembers);


        public ChatViewModel(Chat chat) : base(chat)
        {

        }

        protected override ChatModel GetModel(Chat chat)
        {
            return new ChatModel(chat);
        }

        private async Task LeaveChat()
        {
            throw new NotImplementedException();
        }
        private async Task InviteToChat()
        {
            await Messenger.Send(Utils.Messages.ModalOpened, true);
            var mainWindow = App.Current.MainWindow;
            var membersWindow = new InvitingWindow(Chat)
            {
                Owner = mainWindow,
                ShowInTaskbar = false
            };
            membersWindow.ShowDialog();
            await Messenger.Send(Utils.Messages.ModalClosed, true);
        }
        private async Task ShowMembers()
        {
            await Messenger.Send(Utils.Messages.ModalOpened, true);
            var mainWindow = App.Current.MainWindow;
            var membersWindow= new UserList(Chat.Members.ToArray())
            {
                Owner = mainWindow,
                ShowInTaskbar = false
            };
            membersWindow.ShowDialog();
            await Messenger.Send(Utils.Messages.ModalClosed,true);
        }
    }
}
