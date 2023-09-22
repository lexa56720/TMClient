using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Types;

namespace TMClient.ViewModel.Chats
{
    class UserListViewModel:BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; } = new();
        public UserListViewModel(User[] users)
        {
            foreach(var user in users)
                Users.Add(user);
        }
    }
}
