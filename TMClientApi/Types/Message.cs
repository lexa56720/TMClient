using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ApiWrapper.Types
{
    public class Message : INotifyPropertyChanged
    {
        public int Id { get; init; }

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
        private DateTime sendTime = DateTime.UtcNow;

        public required bool IsReaded
        {
            get => isReaded;
            set
            {
                isReaded = value;
                OnPropertyChanged(nameof(IsReaded));
            }
        }
        private bool isReaded;

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

        public required Chat Destionation { get; init; }
        [SetsRequiredMembers]

        public Message(int id, string text, DateTime sendTime, User author, Chat destination,bool isReaded)
        {
            Id = id;
            Text = text;
            SendTime = sendTime;
            Author = author;
            Destionation = destination;
            IsReaded = isReaded;
        }

        public Message()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
