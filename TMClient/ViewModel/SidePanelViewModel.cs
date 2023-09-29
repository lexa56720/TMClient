using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Types;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel
{
    class SidePanelViewModel : BaseViewModel
    {
        public ObservableCollection<User> Friends => App.UserData.Friends;

        public ObservableCollection<Chat> Chats => App.UserData.MultiUserChats;


        public ICommand AddFriendCommand => new AsyncCommand(AddFriend);
        public ICommand CreateChatCommand => new AsyncCommand(CreateChat);


        public ICommand ChatSelected => new AsyncCommand<Chat>(Selected);

        public ICommand FriendSelected => new AsyncCommand<User>(Selected);

        private async Task Selected(User? user)
        {
            if (user == null)
                return;

            var chat = App.UserData.FriendChats
                .Single(c => c.Members.Any(m => m.Id == user.Id));
            var userChat = new FriendChat(chat);
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
            var friendSearchWindow = new View.FriendRequest
            {
                Owner = App.Current.MainWindow,
                ShowInTaskbar = false
            };
            friendSearchWindow.ShowDialog();
            await Messenger.Send(Messages.ModalClosed);
        }
    }
}
