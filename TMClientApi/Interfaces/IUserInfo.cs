using ApiTypes.Communication.Users;
using ApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWrapper.Interfaces
{
    public interface IUserInfo
    {
        public ObservableCollection<Chat> Dialogs { get; }
        public ObservableCollection<Chat> MultiuserChats { get; }
        public ObservableCollection<User> FriendList { get; }
        public ObservableCollection<FriendRequest> FriendRequests { get; }
        public ObservableCollection<ChatInvite> ChatInvites { get; }

        public event EventHandler<Message[]> NewMessages;
        public User Info { get; }
    }
}
