using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TMClient.Utils;

namespace TMClient.Controls
{
    public abstract class MessageBaseControl : UserControl,INotifyPropertyChanged
    {
        public Message Message 
        { 
            get => message; 
            set
            {
                message = value;
                OnPropertyChanged(nameof(message));
            }
        }
        private Message message;

        public MessageBaseControl(Message message)
        {
            Message= message;
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
