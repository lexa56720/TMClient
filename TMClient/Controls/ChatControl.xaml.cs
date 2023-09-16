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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        public IEnumerable<MessageControl> ItemsSource
        {
            get { return (IEnumerable<MessageControl>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource),
                typeof(IEnumerable<MessageControl>),
                typeof(ChatControl),
                new PropertyMetadata(null));


        public static readonly DependencyProperty LoadMoreProperty =
        DependencyProperty.Register(
           nameof(LoadMore),
           typeof(ICommand),
           typeof(ChatControl),
           new PropertyMetadata(null));
        public ICommand LoadMore
        {
            get { return (ICommand)GetValue(LoadMoreProperty); }
            set { SetValue(LoadMoreProperty, value); }
        }

        public ChatControl()
        {
            InitializeComponent();
        }
    }
}
