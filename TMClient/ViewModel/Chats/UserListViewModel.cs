using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.ViewModel.Chats
{
    public class ChatMember
    {
        public required UserContainer UserContainer { get; set; }
        public required Visibility IsAdminVisibility { get; set; }

        [SetsRequiredMembers]
        public ChatMember(UserContainer userContainer, bool isAdmin)
        {
            UserContainer = userContainer;
            IsAdminVisibility=isAdmin? Visibility.Visible : Visibility.Collapsed;
        }
    }

    class UserListViewModel : BaseViewModel
    {
        public ObservableCollection<ChatMember> Users { get; set; } = new();

        public ICommand AddFriend => new AsyncCommand<ChatMember>(SendFriendRequest);

        private readonly FriendRequestModel FriendRequestModel = new();
        private async Task SendFriendRequest(ChatMember? cahtMember)
        {
            if (cahtMember == null)
                return;

            await FriendRequestModel.SendRequest(cahtMember.UserContainer.User);
            cahtMember.UserContainer.IsRequested = true;
        }

        public UserListViewModel(User[] users, Chat chat)
        {
            foreach (var user in users)
            {
                if (user.IsCurrentUser || CurrentUser.FriendList.Any(f => f.Id == user.Id))
                    Users.Add(new ChatMember(new UserContainer(user, Visibility.Collapsed), user.Id == chat.Admin.Id));
                else
                    Users.Add(new ChatMember(new UserContainer(user, Visibility.Visible), user.Id == chat.Admin.Id));
            }
        }
    }
}
