using ApiTypes.Communication.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class Message : INotifyPropertyChanged
    {
        public int Id { get; init; }
        public required User Author
        {
            get => author;
            set
            {
                author = value;
                OnPropertyChanged(nameof(Author));
            }
        }
        private User author = null!;

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

        [SetsRequiredMembers]
        public Message(ApiTypes.Communication.Messages.Message message,User user)
        {
            Id=message.Id;
            Author=user;
            Text=message.Text;
            SendTime=message.SendTime;
        }
        public Message()
        {

        }

        public required Chat Destionation { get; init; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
