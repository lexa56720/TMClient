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
        }
        private async Task SendFriendRequest(User? user)
        {
            if (user != null)
                await Model.SendRequest(user);
        }
        private async Task Search(string? query)
        {
            if (string.IsNullOrEmpty(query) || query.Length < 3)
                return;
            var users = await Model.SearchByName(query);
            Users.Clear();
            foreach (var user in users)
                Users.Add(user);
        }
    }
}
