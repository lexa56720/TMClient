using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel
{
    class SidePanelViewModel : BaseViewModel
    {
        public ICommand AddFriendCommand => new AsyncCommand(AddFriend);
        public ICommand CreateChatCommand => new AsyncCommand(CreateChat);


        public ICommand ChatSelected => new AsyncCommand<Chat>(Selected);
        public ICommand FriendSelected => new AsyncCommand<Friend>(Selected);

        private async Task Selected(Friend? friend)
        {
            if (friend == null)
                return;

            var userChat = new FriendChat(friend);
            await OpenChat(userChat);
        }
        private async Task Selected(Chat? chat)
        {
            if (chat == null)
                return;

            var chatPage = new ChatView(chat);
            await OpenChat(chatPage);
        }
        public SidePanelViewModel()
        {

        }

        private async Task OpenChat(Page page)
        {
            await Messenger.Send(Messages.OpenChatPage, page);
        }

        private async Task AddFriend()
        {
            await Messenger.Send(Messages.ModalOpened);
            var mainwindow = App.Current.MainWindow;
            var friendSearchWindow = new View.FriendRequest
            {
                Owner = mainwindow,
                ShowInTaskbar = false
            };
            friendSearchWindow.ShowDialog();
            await Messenger.Send(Messages.ModalClosed);

        }
        private async Task CreateChat()
        {
            await Messenger.Send(Messages.ModalOpened);
            var mainwindow = App.Current.MainWindow;
            var chatCreationWindow = new ChatCreationWindow
            {
                Owner = mainwindow,
                ShowInTaskbar = false
            };
            if (chatCreationWindow.ShowDialog()==true)
            {
               var a= chatCreationWindow.Chat;
            }
            await Messenger.Send(Messages.ModalClosed);
        }
    }
}
