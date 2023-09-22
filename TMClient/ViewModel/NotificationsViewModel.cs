
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

namespace TMClient.ViewModel
{
    class NotificationsViewModel : BaseViewModel
    {
        public ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();
        public ObservableCollection<ChatInvite> ChatInvites { get; set; } = new();


        public ICommand AcceptFriendRequest => new AsyncCommand<FriendRequest>(AcceptFriend);
        public ICommand DeclineFriendRequest => new AsyncCommand<FriendRequest>(DeclineFriend);

        public ICommand AcceptChatInvite => new AsyncCommand<ChatInvite>(AcceptInvite);
        public ICommand DeclineChatInvite => new AsyncCommand<ChatInvite>(DeclineInvite);

        public ICommand ShowChat => new AsyncCommand<ChatInvite>(ShowChatMembers);

        private async Task AcceptFriend(FriendRequest? request)
        {

        }
        private async Task DeclineFriend(FriendRequest? request)
        {
           
        }

        private async Task AcceptInvite(ChatInvite? invite)
        {
            
        }
        private async Task DeclineInvite(ChatInvite? invite)
        {
         
        }

        private async Task ShowChatMembers(ChatInvite? invite)
        {
            await Messenger.Send(Utils.Messages.ModalOpened);
            var membersWindow = new View.UserList(invite.Chat.Members.ToArray())
            {
                Owner = App.Current.MainWindow,
                ShowInTaskbar = false
            };
            membersWindow.ShowDialog();
            await Messenger.Send(Utils.Messages.ModalClosed);
        }
        
        public NotificationsViewModel()
        {
            for (int i = 0; i < 10; i++)
            {
                FriendRequests.Add(new FriendRequest(new User()
                {
                    Id = 1,
                    IsOnline = true,
                    Login = "ramus",
                    Name = "Halif"
                }, 10));

                ChatInvites.Add(new ChatInvite(new User()
                {
                    Id = i,
                    IsOnline = false,
                    Login = "Ramadan",
                    Name = "Ivan Kal",
                }, 
                new Chat() 
                { 
                    Id = 1, 
                    Name = "BEST CHAT" 
                },i));
            }
            
        }
    }
}
