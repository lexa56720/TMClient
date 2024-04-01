using TMClient.Controls;
using TMClient.ViewModel.Chats;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для UserList.xaml
    /// </summary>
    public partial class ChatMembers : ModernWindow
    {
        public ChatMembers(User[] users,Chat chat)
        {
            InitializeComponent();
            Title = chat.Name;
            DataContext = new ChatMembersViewModel(users,chat);
        }
    }
}
