using ClientApiWrapper.Types;
using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Pages
{
    class SidePanelViewModel : BaseViewModel
    {
        public ICommand AddFriendCommand => new AsyncCommand(AddFriend);
        public ICommand CreateChatCommand => new AsyncCommand(CreateChat);

        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }
        private string searchQuery;

        public Friend? SelectedFriend
        {
            get => selectedFriend;
            set
            {
                selectedFriend = value;
                OnPropertyChanged(nameof(SelectedFriend));
                if (value != null)
                {
                    SelectedChat = null;
                    OpenChat(new FriendChat(value));
                }
            }
        }
        private Friend? selectedFriend;

        public Chat? SelectedChat
        {
            get => selectedChat;
            set
            {
                selectedChat = value;
                OnPropertyChanged(nameof(SelectedChat));
                if (value != null)
                {
                    SelectedFriend = null;
                    OpenChat(new ChatView(value));
                }
            }
        }
        private Chat? selectedChat;

        public SidePanelViewModel()
        {

        }

        private void OpenChat(Page page)
        {
            Messenger.Send(Messages.OpenChatPage, page, true);
        }

        private async Task AddFriend()
        {
            await Messenger.Send(Messages.ModalOpened, true);
            var mainwindow = System.Windows.Application.Current.MainWindow;
            var friendSearchWindow = new View.FriendRequest
            {
                Owner = mainwindow,
                ShowInTaskbar = false
            };
            friendSearchWindow.ShowDialog();
            await Messenger.Send(Messages.ModalClosed, true);

        }
        private async Task CreateChat()
        {
            await Messenger.Send(Messages.ModalOpened, true);
            var mainwindow = System.Windows.Application.Current.MainWindow;
            var chatCreationWindow = new ChatCreationWindow
            {
                Owner = mainwindow,
                ShowInTaskbar = false
            };
            chatCreationWindow.ShowDialog();
            await Messenger.Send(Messages.ModalClosed, true);
        }
    }
}
