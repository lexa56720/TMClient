using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для IconButton.xaml
    /// </summary>
    public partial class IconButton : Button, INotifyPropertyChanged
    {
        public Visibility TextVisibility
        {
            get => textVisibility;
            set
            {
                textVisibility = value;
                OnPropertyChanged(nameof(TextVisibility));
            }

        }
        private Visibility textVisibility = Visibility.Collapsed;


        public Visibility IconFirstVisibility
        {
            get => iconFirstVisibility;
            set
            {
                iconFirstVisibility = value;
                OnPropertyChanged(nameof(IconFirstVisibility));
            }

        }
        private Visibility iconFirstVisibility = Visibility.Visible;

        public Visibility TextFirstVisibility
        {
            get => textFirstVisibility;
            set
            {
                textFirstVisibility = value;
                OnPropertyChanged(nameof(TextFirstVisibility));
            }

        }
        private Visibility textFirstVisibility = Visibility.Collapsed;

        public bool IsIconFirst
        {
            get => isIconFirst;
            set
            {
                isIconFirst = value;
                OnPropertyChanged(nameof(IsIconFirst));
                if(isIconFirst)
                {
                    IconFirstVisibility = Visibility.Visible;
                    TextFirstVisibility = Visibility.Hidden;
                }
                else
                {
                    IconFirstVisibility = Visibility.Hidden;
                    TextFirstVisibility = Visibility.Visible;
                }
            }
        }
        private bool isIconFirst=true;

        public Orientation Orientation
        {
            get => orientation;
            set
            {
                orientation = value;
                OnPropertyChanged(nameof(Orientation));
            }
        }
        private Orientation orientation = Orientation.Horizontal;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconButton),
            new PropertyMetadata(string.Empty, TextPropertyChanged));


        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(IconButton),
            new PropertyMetadata(string.Empty, IconPropertyChanged));
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public IconButton()
        {
            InitializeComponent();
        }

        private void IconPropertyChanged(string text)
        {
            OnPropertyChanged(nameof(Icon));
        }

        private static void IconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IconButton)d).IconPropertyChanged((string)e.NewValue);
        }

        private void TextPropertyChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
                TextVisibility = Visibility.Collapsed;
            else
                TextVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(Text));
        }

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((IconButton)d).TextPropertyChanged((string)e.NewValue);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
