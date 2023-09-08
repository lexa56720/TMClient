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
using TMClient.Types;

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для MessageControl.xaml
    /// </summary>
    public partial class MessageControl : UserControl,INotifyPropertyChanged
    {
        public bool IsOwn
        {
            get => isOwn;
            set
            {
                isOwn = value;
                OnPropertyWasChanged(nameof(IsOwn));
            }
        }
        private bool isOwn;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyWasChanged(nameof(Text));
            }
        }
        private string text;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MessageControl(Message message)
        {
            InitializeComponent();
            DataContext = this;
        }
        public MessageControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void OnPropertyWasChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
