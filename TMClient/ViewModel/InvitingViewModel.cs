using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.ViewModel
{
    class InvitingViewModel : BaseViewModel
    {
        public ObservableCollection<UserContainer> Users { get; set; }

        public bool IsCanSumbit
        {
            get => isCanSumbit;
            set
            {
                isCanSumbit = value;
                OnPropertyChanged(nameof(IsCanSumbit));
            }
        }
        private bool isCanSumbit;

        public string Query
        {
            get => query;
            set
            {
                query = value;
                Search(Query);
            }
        }
        private string query = string.Empty;

        public ICommand ConfirmCommand => new AsyncCommand(Confirm);
        public ICommand SelectCommand => new Command(Select);


        private readonly InvitingModel Model = new();

        public Chat Chat { get; }
        public Action Close { get; }

        public InvitingViewModel(Chat chat,Action close)
        {
            var invitableFriends = Model.GetUsersToInivite(chat, CurrentUser.FriendList);
            Users = new ObservableCollection<UserContainer>(invitableFriends.Select(friend => new UserContainer(friend)));
            Chat = chat;
            Close = close;
        }

        private void Select(object? obj)
        {
            if (obj is UserContainer user)
                user.IsRequested = !user.IsRequested;

            IsCanSumbit = Users.Any(u => u.IsRequested);
        }

        private async Task Confirm()
        {
            var invited = Users.Where(u => u.IsRequested).Select(u => u.User);
            await Model.Invite(Chat, invited);
            Close();
        }

        private void Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                Users.ToList().ForEach(u => u.Visibility = Visibility.Visible);
                return;
            }

            var searchedUsers = Model.Search(Users, query);

            Users.ToList().ForEach(u => u.Visibility = Visibility.Collapsed);
            searchedUsers.ToList().ForEach(u => u.Visibility = Visibility.Visible);
        }
    }
}
