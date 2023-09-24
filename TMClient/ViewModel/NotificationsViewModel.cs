
using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Types;
using TMClient.Utils;

namespace TMClient.ViewModel
{
    class NotificationsViewModel : BaseViewModel
    {
        public ObservableCollection<FriendRequest> FriendRequests => App.Requests.FriendRequests;

        public ObservableCollection<ChatInvite> ChatInvites => App.Requests.ChatInvites;


        public ICommand AcceptFriendRequest => new AsyncCommand<FriendRequest>(AcceptFriend);
        public ICommand DeclineFriendRequest => new AsyncCommand<FriendRequest>(DeclineFriend);

        public ICommand AcceptChatInvite => new AsyncCommand<ChatInvite>(AcceptInvite);
        public ICommand DeclineChatInvite => new AsyncCommand<ChatInvite>(DeclineInvite);

        public ICommand ShowChat => new AsyncCommand<ChatInvite>(ShowChatMembers);

        private async Task AcceptFriend(FriendRequest? request)
        {
            if (request != null)
            {
                await Model.AcceptFriend(request);
                FriendRequests.Remove(request);
            }
        }
        private async Task DeclineFriend(FriendRequest? request)
        {
            if (request != null)
            {
                await Model.DeclineFriend(request);
                FriendRequests.Remove(request);
            }
        }

        private async Task AcceptInvite(ChatInvite? invite)
        {
            if (invite != null)
            {
                await Model.AcceptInvite(invite);
                ChatInvites.Remove(invite);
            }
        }
        private async Task DeclineInvite(ChatInvite? invite)
        {
            if (invite != null)
            {
                await Model.DeclineInvite(invite);
                ChatInvites.Remove(invite);
            }
        }

        private async Task ShowChatMembers(ChatInvite? invite)
        {
            await Messenger.Send(Messages.ModalOpened);
            var membersWindow = new View.UserList(invite.Chat.Members.ToArray())
            {
                Owner = App.Current.MainWindow,
                ShowInTaskbar = false
            };
            membersWindow.ShowDialog();
            await Messenger.Send(Messages.ModalClosed);
        }

        private NotificationsModel Model;

        public NotificationsViewModel()
        {
            Model = new NotificationsModel();

            //    ChatInvites.Add(new ChatInvite(new User()
            //    {
            //        Id = i,
            //        IsOnline = false,
            //        Login = "Ramadan",
            //        Name = "Ivan Kal",
            //    },
            //    new Chat()
            //    {
            //        Id = 1,
            //        Name = "BEST CHAT"
            //    }, i));
            //}

        }

    }
}
