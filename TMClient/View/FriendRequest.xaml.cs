using TMClient.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для NewFriendRequest.xaml
    /// </summary>
    public partial class FriendRequest : ModernWindow
    {

        public FriendRequest()
        {
            Title = "Добавить друга";
            InitializeComponent();
            DataContext = new FriendRequestViewModel();
        }
    }
}
