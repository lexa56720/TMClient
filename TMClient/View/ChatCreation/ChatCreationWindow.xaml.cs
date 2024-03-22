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
using System.Windows.Shapes;
using TMClient.Controls;
using TMClient.ViewModel;

namespace TMClient.View
{
    /// <summary>
    /// Логика взаимодействия для ChatCreationWindow.xaml
    /// </summary>
    public partial class ChatCreationWindow : ModernWindow
    {
        public Chat? Chat { get; private set; }

        public ChatCreationWindow()
        {
            Title = "Создание чата";
            DataContext = new ChatCreationWindowViewModel(DialogCompleted);
            InitializeComponent();
        }
        private void DialogCompleted(Chat? chat)
        {
            Chat = chat;
            DialogResult = Chat != null;
            Close();
        }
    }
}
