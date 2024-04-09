using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TMClient.Controls
{
    public class MessageContainer:INotifyPropertyChanged
    {
        public Message Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        private Message message;
        public bool IsAuthorVisible
        {
            get => isAuthorVisibile;
            set
            {
                isAuthorVisibile = value;
                OnPropertyChanged(nameof(IsAuthorVisible));
            }
        }
        private bool isAuthorVisibile = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public MessageContainer(Message message)
        {
            Message = message;
        }
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
