using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
    public partial class MessageControl : UserControl, INotifyPropertyChanged
    {
        public required bool IsOwn
        {
            get => isOwn;
            set
            {
                isOwn = value;
                OnPropertyChanged(nameof(IsOwn));
            }
        }
        private bool isOwn;

        public required string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string text = string.Empty;

        public required string Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
        private string time=string.Empty;


        public event PropertyChangedEventHandler? PropertyChanged;

        [SetsRequiredMembers]
        public MessageControl(Message message)
        {
            InitializeComponent();
            IsOwn = message.Id == App.Api.Id;
            Text = message.Text;
            Time = message.SendTime.ToString();
            DataContext = this;
        }
        public MessageControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
