using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class RequestStorage : INotifyPropertyChanged
    {
        public ObservableCollection<FriendRequest> FriendRequests
        {
            get => friendRequests;
            set
            {
                friendRequests = value;
            }
        }
        private ObservableCollection<FriendRequest> friendRequests = new();

        public ObservableCollection<ChatInvite> ChatInvites
        {
            get => chatInvites;
            set
            {
                chatInvites = value;
                OnPropertyChanged(nameof(ChatInvites));
            }
        }
        private ObservableCollection<ChatInvite> chatInvites = new();


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
