using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для ExtendeListView.xaml
    /// </summary>
    public partial class ExtendeListView : ListView
    {
        private ScrollViewer? ScrollViewer
        {
            get => scrollViewer;
            set
            {
                if (value != null && scrollViewer != value)
                {
                    scrollViewer = value;
                    scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                }
            }
        }

        private ScrollViewer? scrollViewer;


        public static readonly DependencyProperty ReachTopProperty =
       DependencyProperty.Register(
           nameof(ReachTop),
           typeof(ICommand),
           typeof(ExtendeListView),
           new PropertyMetadata(null));
        public ICommand ReachTop
        {
            get 
            { 
                return (ICommand)GetValue(ReachTopProperty);
            }
            set 
            { 
                SetValue(ReachTopProperty, value);
            }
        }

        public ExtendeListView()
        {
            InitializeComponent();
            ScrollViewer = FindScrollViewer(this);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer?.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + e.ExtentHeightChange);
            if (e.VerticalOffset == 0)
            {
                if (ReachTop != null && ReachTop.CanExecute(null))
                    ReachTop.Execute(null);
            }
        }

        private ScrollViewer FindScrollViewer(DependencyObject d)
        {
            if (d is ScrollViewer)
                return d as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var sw = FindScrollViewer(VisualTreeHelper.GetChild(d, i));
                if (sw != null)
                    return sw;
            }
            return null;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            ScrollViewer ??= FindScrollViewer(this);
        }
    }
}
