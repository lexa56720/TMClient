using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model;

namespace TMClient.ViewModel
{
    class ProfileCardViewModel : BaseViewModel
    {
        private readonly ProfileCardModel Model = new();

        public User User { get; }
        public bool IsAlreadyFriends { get; }

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

        public ICommand SendFriendRequest => new AsyncCommand(AddToFriends);

        private async Task AddToFriends()
        {
            IsRequested = true;
            await Model.AddToFriend(User);
        }

        public ProfileCardViewModel(User user)
        {
            User = user;
            IsAlreadyFriends = Model.IsAlreadyFriend(user);
        }
    }
}
