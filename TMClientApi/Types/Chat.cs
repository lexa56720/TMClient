using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ApiWrapper.Types
{
    public class Chat : INotifyPropertyChanged,IDisposable
    {
        public int Id { get; }

        public bool IsDialogue { get; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string name = string.Empty;

        public int UnreadedCount
        {
            get => unreadedCount;
            set
            {
                unreadedCount = value;
                OnPropertyChanged(nameof(UnreadedCount));
            }
        }
        private int unreadedCount;

        public User? WritingUser
        {
            get => writingUser;
            set
            {
                writingUser = value;
                OnPropertyChanged(nameof(WritingUser));
            }
        }
        private User? writingUser;

        public Message? LastMessage
        {
            get => lastMessage;
            set
            {
                lastMessage = value;
                OnPropertyChanged(nameof(LastMessage));
            }
        }
        private Message? lastMessage;
        public ObservableCollection<User> Members { get; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool IsDisposed;
        public Chat(int id, string name,int unreadCount ,bool isDialogue)
        {
            Id = id;
            Name = name;
            IsDialogue = isDialogue;
            UnreadedCount=unreadCount;
        }
        public void Dispose()
        {
            if (IsDisposed)
                return;

            PropertyChanged = null;
            Members.Clear();
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void Update(Chat chat)
        {
            if (chat == this)
                return;
            Name = chat.name;
            WritingUser = chat.WritingUser;
            LastMessage = chat.LastMessage;

            Members.Clear();
            foreach (var member in chat.Members)
                Members.Add(member);
        }
    }
}
