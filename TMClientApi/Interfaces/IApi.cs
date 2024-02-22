using ApiTypes.Communication.BaseTypes;
using ApiTypes.Communication.Search;
using ApiTypes.Shared;
using ApiTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTypes.Communication.Chats;
using ApiTypes.Communication.Auth;
using ApiTypes.Communication.Users;
using System.Collections.ObjectModel;
using TMApi.ApiRequests.Friends;

namespace ApiWrapper.Interfaces
{
    public interface IApi : IDisposable
    {
        public IUsersApi Users { get; }

        public IMessagesApi Messages { get; }

        public IChatsApi Chats { get; }

        public IFriendsApi Friends { get; }


        public ObservableCollection<Chat> Dialogs { get; }
        public ObservableCollection<Chat> MultiuserChats { get; }
        public ObservableCollection<User> FriendList { get; }
        public ObservableCollection<FriendRequest> FriendRequests { get; }
        public ObservableCollection<ChatInvite> ChatInvites { get; }

        public UserInfo CurrentUser { get; }

        public Task Save(string path);
    }
}
