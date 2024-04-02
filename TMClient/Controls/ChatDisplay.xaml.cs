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
using TMClient.Utils;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для ChatDisplay.xaml
    /// </summary>
    public partial class ChatDisplay : UserControl
    {
        public static readonly DependencyProperty ChatProperty =
        DependencyProperty.Register(nameof(Chat),
                                    typeof(Chat),
                                    typeof(ChatDisplay),
                                    new PropertyMetadata(null));
        public Chat Chat
        {
            get => (Chat)GetValue(ChatProperty);
            set
            {
                SetValue(ChatProperty, value);
            }
        }
        public ChatDisplay()
        {
            InitializeComponent();
        }
        private void OnClick(object sender, RoutedEventArgs e)
        {
            Messenger.Send(Messages.ModalOpened, true);
            var mainwindow = App.Current.MainWindow;
            var chatCard = new View.ChatCard(Chat)
            {
                Owner = mainwindow,
                ShowInTaskbar = false
            };
            chatCard.ShowDialog();
            Messenger.Send(Messages.ModalClosed, true);
        }
    }
}
