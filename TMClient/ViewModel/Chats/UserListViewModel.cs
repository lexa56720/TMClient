using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.ViewModel.Chats
{

    class UserListViewModel : BaseViewModel
    {
        public ObservableCollection<UserContainer> Users { get; set; } = new();

        public ICommand AddFriend => new AsyncCommand<UserContainer>(SendFriendRequest);

        private readonly FriendRequestModel FriendRequestModel = new();
        private async Task SendFriendRequest(UserContainer? userContainer)
        {
            if (userContainer == null)
                return;

            await FriendRequestModel.SendRequest(userContainer.User);
            userContainer.IsRequested = true;
        }

        public UserListViewModel(User[] users)
        {
            foreach (var user in users)
            {
                if (CurrentUser.Info.Id == user.Id || CurrentUser.FriendList.Any(f => f.Id == user.Id))
                    Users.Add(new UserContainer(user, Visibility.Collapsed));
                else
                    Users.Add(new UserContainer(user, Visibility.Visible));
            }
        }
    }
}
