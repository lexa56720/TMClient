using ApiTypes.Communication.Messages;
using ClientApiWrapper.Types;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ClientApiWrapper.Types
{
    public class Message : INotifyPropertyChanged
    {
        public required int Id { get; init; }


        public virtual bool IsSystem => false;

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

        public required DateTime SendTime
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

        public required Chat Destination { get; init; }

        public required Attachment[] Attachments { get; init; } = [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public Message()
        {

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
