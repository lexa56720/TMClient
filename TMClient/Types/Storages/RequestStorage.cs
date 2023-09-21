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


        public async Task Load()
        {
            var requests = await LoadRequests();
            foreach (var request in requests)
                FriendRequests.Add(request);

            var invites = await LoadInvites(); 
            foreach (var invite in invites)
                ChatInvites.Add(invite);
        }
        private async Task<FriendRequest[]> LoadRequests()
        {
            var requests = await App.Api.Friends.GetFriendRequest(App.Api.UserInfo.FriendRequests);
            var convertedRequest = requests.Select(async r => new FriendRequest()
            {
                From = await App.UserData.GetUser(r.FromId),
                Id = r.Id
            });
            return await Task.WhenAll(convertedRequest);
        }
        private async Task<ChatInvite[]> LoadInvites()
        {
            var invites = await App.Api.Chats.GetChatInvite(App.Api.UserInfo.ChatInvites);
            var convertedInvites = invites.Select(async i => new ChatInvite()
            {
                Id = i.Id,
                Chat = await App.UserData.GetChat(i.ChatId),
                Inviter = await App.UserData.GetUser(i.FromUserId)
            });
            return await Task.WhenAll(convertedInvites);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
