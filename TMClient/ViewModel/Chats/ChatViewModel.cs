using ApiWrapper.Types;
using AsyncAwaitBestPractices.MVVM;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Chats;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Chats
{
    internal class ChatViewModel(Chat chat) : BaseChatViewModel<MultiUserChatModel>(chat)
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

        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
        private bool isReadOnly = false;

        protected override MultiUserChatModel GetModel(Chat chat)
        {
            return new MultiUserChatModel(chat, 20);
        }

        private async Task LeaveChat()
        {
            await Model.LeaveChat();
            IsReadOnly = true;
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
            var membersWindow = new UserList(Chat.Members.ToArray())
            {
                Owner = mainWindow,
                ShowInTaskbar = false
            };
            membersWindow.ShowDialog();
            await Messenger.Send(Utils.Messages.ModalClosed, true);
        }
    }
}
