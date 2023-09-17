using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Types;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel
{
    class SidePanelViewModel : BaseViewModel
    {
        public ObservableCollection<User> Friends
        {
            get => friends;
            set
            {
                friends = value;
                OnPropertyChanged(nameof(Friends));
            }
        }
        private ObservableCollection<User> friends = new ();

        public ObservableCollection<Chat> Chats
        {
            get => chats;
            set
            {
                chats = value;
                OnPropertyChanged(nameof(Chats));
            }
        }
        private ObservableCollection<Chat> chats=new();


        public ICommand AddFriendCommand => new AsyncCommand(AddFriend);
        public ICommand CreateChatCommand => new AsyncCommand(CreateChat);

        public SidePanelViewModel()
        {
            for (int i = 0; i < 20; i++)
                Friends.Add(new User() { Name = "Ffff",IsOnline=true,Id=1 });
        }
 
        private async Task AddFriend()
        {
            await Messenger.Send(Messages.ModalOpened);
            var friendSearchWindow = new NewFriendRequest();
            friendSearchWindow.Owner = App.Current.MainWindow;
            friendSearchWindow.ShowInTaskbar = false;
            friendSearchWindow.ShowDialog();
            if(friendSearchWindow.DialogResult!=null)
            {

            }
            await Messenger.Send(Messages.ModalClosed);

        }
        private async Task CreateChat()
        {

        }
    }
}
