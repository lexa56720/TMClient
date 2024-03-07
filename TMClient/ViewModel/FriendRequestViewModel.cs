using AsyncAwaitBestPractices.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TMClient.Model;

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
                if (CurrentUser.FriendList.Any(f=>f.Id!=user.Id))
                    Users.Add(user);
        }
    }
}
