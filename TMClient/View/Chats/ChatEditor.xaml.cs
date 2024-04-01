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
    /// Логика взаимодействия для ChatEditor.xaml
    /// </summary>
    public partial class ChatEditor : Page
    {
        public ChatEditor(Chat chat,Action openChatPage)
        {
            DataContext=new ChatEditorViewModel(chat,openChatPage);
            InitializeComponent();
        }
    }
}
