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
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable<MessageControl>),
            typeof(ChatControl),
            new PropertyMetadata(null));
        public IEnumerable<MessageControl> ItemsSource
        {
            get { return (IEnumerable<MessageControl>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


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


        public static readonly DependencyProperty SendCommandProperty =
        DependencyProperty.Register(
           nameof(SendCommand),
           typeof(ICommand),
           typeof(ChatControl),
           new PropertyMetadata(null));

        public ICommand SendCommand
        {
            get { return (ICommand)GetValue(SendCommandProperty); }
            set { SetValue(SendCommandProperty, value); }
        }


        public static readonly DependencyProperty AttachCommandProperty =
        DependencyProperty.Register(
           nameof(AttachCommand),
           typeof(ICommand),
           typeof(ChatControl),
           new PropertyMetadata(null));
        public ICommand AttachCommand
        {
            get { return (ICommand)GetValue(AttachCommandProperty); }
            set { SetValue(AttachCommandProperty, value); }
        }


        public static readonly DependencyProperty WrittenTextProperty =
        DependencyProperty.Register(
           nameof(WrittenText),
           typeof(string),
           typeof(ChatControl),
           new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string WrittenText
        {
            get { return (string)GetValue(WrittenTextProperty); }
            set { SetValue(WrittenTextProperty, value); }
        }

        public ChatControl()
        {
            InitializeComponent();
        }

        private bool IsFirstTimeLoad = true;
        private void ChatLoaded(object sender, EventArgs e)
        {
            if (LoadMore.CanExecute(null) && IsFirstTimeLoad)
            {
                IsFirstTimeLoad = false;
                LoadMore.Execute(null);
            }            
        }
    }
}
