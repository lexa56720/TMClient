
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Types;

namespace TMClient.ViewModel
{
    class NotificationsViewModel : BaseViewModel
    {
        public ObservableCollection<FriendRequest> FriendRequests { get; set; } = new();

        public NotificationsViewModel()
        {
            for (int i = 0; i < 10; i++)
                FriendRequests.Add(new FriendRequest(new User()
                {
                    Id = 1,
                    IsOnline = true,
                    Login = "ramus",
                    Name = "Halif"
                }, 10));
        }
    }
}
