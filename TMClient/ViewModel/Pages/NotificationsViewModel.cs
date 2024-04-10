using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TMClient.Model.Pages;
using TMClient.Utils;

namespace TMClient.ViewModel.Pages
{
    class NotificationsViewModel : BaseViewModel
    {
        public ObservableCollection<FriendRequest> FriendRequests => CurrentUser.FriendRequests;

        public ObservableCollection<ChatInvite> ChatInvites => CurrentUser.ChatInvites;

        public ICommand AcceptFriendRequest => new AsyncCommand<FriendRequest>(AcceptFriend);
        public ICommand DeclineFriendRequest => new AsyncCommand<FriendRequest>(DeclineFriend);

        public ICommand AcceptChatInvite => new AsyncCommand<ChatInvite>(AcceptInvite);
        public ICommand DeclineChatInvite => new AsyncCommand<ChatInvite>(DeclineInvite);

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


        private readonly NotificationsModel Model = new();

    }
}
