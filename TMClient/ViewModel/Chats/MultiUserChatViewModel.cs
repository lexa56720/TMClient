using ApiWrapper.Types;
using AsyncAwaitBestPractices.MVVM;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Model.Chats;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Chats
{
    internal class MultiUserChatViewModel : BaseChatViewModel<MultiUserChatModel>
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
        public ICommand EditCommand => new Command(Edit);


        private readonly Action OpenEditorPage;

        public MultiUserChatViewModel(Chat chat,Action openEditorPage) : base(chat)
        {
            OpenEditorPage = openEditorPage;
        }
        protected override MultiUserChatModel GetModel(Chat chat)
        {
            return new MultiUserChatModel(chat, 20);
        }

        private async Task LeaveChat()
        {
            await Model.LeaveChat();
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
        private void Edit(object? obj)
        {
            OpenEditorPage();
        }
    }
}
