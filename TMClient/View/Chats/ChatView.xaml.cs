using ApiWrapper.Types;
using System.Windows.Controls;
using TMClient.ViewModel.Chats;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для ChatView.xaml
    /// </summary>
    public partial class ChatView : Page
    {
        public ChatView(Chat chat)
        {
            DataContext = new ChatViewModel(chat);
            InitializeComponent();
        }
    }
}
