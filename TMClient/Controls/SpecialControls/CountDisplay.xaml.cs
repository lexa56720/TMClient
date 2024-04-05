using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для CountDisplay.xaml
    /// </summary>
    public partial class CountDisplay : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register(nameof(Count),
                                    typeof(int),
                                    typeof(CountDisplay),
                                    new PropertyMetadata(0, CountPropertyChanged));
        public int Count
        {
            get => (int)GetValue(CountProperty);
            set
            {
                SetValue(CountProperty, value);
            }
        }

        public string CountText
        {
            get => countText;
            set
            {
                countText = value;
                OnPropertyChanged(nameof(CountText));
            }
        }
        private string countText = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        public CountDisplay()
        {
            InitializeComponent();
            Visibility = Visibility.Collapsed;
        }
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private static void CountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CountDisplay)d).TextPropertyChanged((int)e.NewValue);
        }
        private void TextPropertyChanged(int count)
        {
            if (count == 0)
                Visibility = Visibility.Collapsed;
            else if (Visibility != Visibility.Visible)
                Visibility = Visibility.Visible;

            if (count > 99)
                CountText = "99";
            else
                CountText = count.ToString();
        }
    }
}
