using TMClient.Controls;
using TMClient.ViewModel.Chats;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для UserList.xaml
    /// </summary>
    public partial class UserList : ModernWindow
    {
        public UserList(User[] users,Chat chat)
        {
            InitializeComponent();
            Title = "Пользователи";
            DataContext = new UserListViewModel(users,chat);
        }
    }
}
