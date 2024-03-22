using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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


        public static readonly DependencyProperty IsReachTopEnabledProperty =
        DependencyProperty.Register(
            nameof(IsReachTopEnabled),
            typeof(bool),
            typeof(ExtendeListView),
            new PropertyMetadata(false));
        public bool IsReachTopEnabled
        {
            get { return (bool)GetValue(IsReachTopEnabledProperty); }
            set { SetValue(IsReachTopEnabledProperty, value); }
        }

        public ExtendeListView()
        {
            InitializeComponent();

            ScrollViewer = FindScrollViewer(this);
        }


        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (Math.Abs(e.VerticalOffset - ((ScrollViewer)e.Source).ScrollableHeight) < 150)
            {
                if (e.ExtentHeightChange < 0)
                    ScrollViewer?.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.ExtentHeightChange * 2);
                else
                    ScrollViewer?.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + e.ExtentHeightChange);
            }
            else
                ScrollViewer?.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + e.ExtentHeightChange);
            if (e.VerticalOffset == 0)
            {
                if (IsReachTopEnabled != true && ReachTop != null && ReachTop.CanExecute(null))
                    ReachTop.Execute(null);
            }
        }

        private ScrollViewer? FindScrollViewer(DependencyObject d)
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
