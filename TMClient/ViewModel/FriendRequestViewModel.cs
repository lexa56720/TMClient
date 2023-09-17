using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Types;

namespace TMClient.ViewModel
{
    class FriendRequestViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; } = new();


        public ICommand SearchCommand => new AsyncCommand<string>(Search);



        public ICommand AddFriend => new AsyncCommand<User>(SendFriendRequest);

        private FriendRequestModel Model = new();
        public FriendRequestViewModel()
        {
            for (int i = 0; i < 30; i++)
                Users.Add(new User() { Id = 1, Name = "Pete tami abu sali", IsOnline = true });
        }
        private async Task SendFriendRequest(User? user)
        {
            if (user != null)
                await Model.SendRequest(user);
        }
        private async Task Search(string? query)
        {
            throw new NotImplementedException();
        }
    }
}
