using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.ViewModel.Chats
{
    public class ChatMember : UserContainer
    {
        public required Visibility IsAdminVisibility { get; set; }

        [SetsRequiredMembers]
        public ChatMember(User user, Visibility visibility, bool isAdmin) : base(user, visibility)
        {
            IsAdminVisibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    class ChatMembersViewModel : BaseViewModel
    {
        public Chat Chat { get; }
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }
        private bool isAdmin;
        public ICommand AddFriend => new AsyncCommand<ChatMember>(SendFriendRequest);
        public ICommand KickUserCommand => new AsyncCommand<ChatMember>(KickUser);
        public ObservableCollection<ChatMember> Members { get; set; } = new();

        private readonly ChatMembersModel Model = new();


        public ChatMembersViewModel(User[] users, Chat chat)
        {
            IsAdmin = chat.Admin.IsCurrentUser;
            foreach (var user in users)
            {
                var visibility = Visibility.Visible;
                if (user.IsCurrentUser || CurrentUser.FriendList.Any(f => f.Id == user.Id))
                    visibility = Visibility.Collapsed;
                var isAdmin = user.Id == chat.Admin.Id;
                var member = new ChatMember(user, visibility, isAdmin);
                if (isAdmin)
                    Members.Insert(0, member);
                else
                    Members.Add(member);
            }
            Chat = chat;
        }

        private async Task SendFriendRequest(ChatMember? member)
        {
            if (member == null)
                return;

            await Model.SendRequest(member.User);
            member.IsRequested = true;
        }
        private async Task KickUser(ChatMember? member)
        {
            if (member == null)
                return;
            await Model.KickUser(member.User, Chat);
            Members.Remove(member);
        }
    }
}
