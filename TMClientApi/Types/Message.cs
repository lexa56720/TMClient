using ApiTypes.Communication.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClientApi.Types
{
    public class Message : INotifyPropertyChanged
    {
        public int Id { get; }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string text = string.Empty;

        public DateTime SendTime
        {
            get => sendTime;
            set
            {
                sendTime = value;
                OnPropertyChanged(nameof(SendTime));
            }
        }
        private DateTime sendTime;

        public User Author
        {
            get => author;
            set
            {
                author = value;
                OnPropertyChanged(nameof(Author));
            }
        }
        private User author = null!;

        public Chat Destionation { get; }

        public Message(int id, string text, DateTime sendTime, User author, Chat destination)
        {
            Id = id;
            Text = text;
            SendTime = sendTime;
            Author = author;
            Destionation = destination;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
