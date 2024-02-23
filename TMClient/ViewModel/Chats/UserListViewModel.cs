using System.Collections.ObjectModel;

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
