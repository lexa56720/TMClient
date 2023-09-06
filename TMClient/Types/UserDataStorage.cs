using ApiTypes.Communication.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    class UserDataStorage
    {
        public required User CurrentUser { get; init; }
        public ObservableCollection<User> Friends { get; set; } = new();

        public ObservableCollection<Chat> Chats { get; set; } = new();

        [SetsRequiredMembers]
        public UserDataStorage(User currentUser) 
        {
            CurrentUser = currentUser;
        }
    }
}
