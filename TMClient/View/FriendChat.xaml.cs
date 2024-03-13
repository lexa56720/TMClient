using ClientApiWrapper.Types;
using System.Windows.Controls;
using TMClient.ViewModel.Chats;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для FriendChat.xaml
    /// </summary>
    public partial class FriendChat : Page
    {
        public FriendChat(Friend friend)
        {
            DataContext = new FriendChatViewModel(friend);
            InitializeComponent();
        }
    }
}
