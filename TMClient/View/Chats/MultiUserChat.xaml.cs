using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMClient.ViewModel.Chats;

namespace TMClient.View.Chats
{
    /// <summary>
    /// Логика взаимодействия для MultiUserChat.xaml
    /// </summary>
    public partial class MultiUserChat : Page
    {
        public MultiUserChat(Chat chat,Action openEditorPage)
        {
            InitializeComponent();
            DataContext = new MultiUserChatViewModel(chat, openEditorPage, ChatControl.SetChatBoxFocus);

        }
    }
}
