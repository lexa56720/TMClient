using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;

namespace TMClient.ViewModel
{

    internal class UserRequest : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public bool IsRequested
        {
            get => isRequested;
            set
            {
                isRequested = value;
                OnPropertyChanged(nameof(IsRequested));
            }
        }
        private bool isRequested;
        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }
        private Visibility visibility;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        private User user = null!;

        public UserRequest(User user)
        {
            User = user;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    internal class FriendPickerViewModel : BaseViewModel
    {
        public ObservableCollection<UserRequest> Users { get; set; }

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
                Search(value);
                query = value;
            }
        }
        private string query = string.Empty;
        public ICommand ConfirmCommand => new Command(Confirm);
        public ICommand SelectCommand => new Command(Select);

        private void Select(object? obj)
        {
            if (obj is UserRequest user)
                user.IsRequested = !user.IsRequested;

            IsCanSumbit = Users.Where(u => u.IsRequested).Count() > 1;
        }

        private FriendPickerModel Model = new();
        private readonly Action<User[]> ReturnValue;
        public FriendPickerViewModel(Action<User[]> returnValue)
        {
            var requests = new UserRequest[CurrentUser.FriendList.Count];
            for (var i = 0; i < requests.Length; i++)
                requests[i] = new UserRequest(CurrentUser.FriendList[i]);
            Users = new ObservableCollection<UserRequest>(requests);
            ReturnValue = returnValue;
        }
        private void Confirm()
        {
            ReturnValue(Users.Where(u => u.IsRequested)
                             .Select(r => r.User)
                             .ToArray());
        }
        private void Search(object? obj)
        {
            if (obj is not string query || string.IsNullOrEmpty(query))
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
