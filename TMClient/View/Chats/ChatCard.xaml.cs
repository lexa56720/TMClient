using TMClient.Controls;
using TMClient.ViewModel.Chats;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для UserList.xaml
    /// </summary>
    public partial class ChatCard : ModernWindow
    {
        public ChatCard(Chat chat)
        {
            Title = chat.Name;
            DataContext = new ChatMembersViewModel(chat);
            InitializeComponent();
        }
    }
}
