using ClientApiWrapper.Types;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ApiWrapper.Types
{
    public class Chat : NamedImageEntity,IDisposable
    {
        public bool IsDialogue { get; }
        public int UnreadCount
        {
            get => unreadCount;
            set
            {
                if (value < 0)
                    value = 0;
                unreadCount = value;
                OnPropertyChanged(nameof(UnreadCount));
            }
        }
        private int unreadCount;

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

        private bool IsDisposed;
        public Chat(int id, string name,int unreadCount ,bool isDialogue)
        {
            Id = id;
            Name = name;
            IsDialogue = isDialogue;
            UnreadCount=unreadCount;
        }
        public void Dispose()
        {
            if (IsDisposed)
                return;

            Members.Clear();         
            IsDisposed = true;
        }

        internal void Update(Chat chat)
        {
            if (chat == this)
                return;
            Name = chat.Name;
            WritingUser = chat.WritingUser;
            LastMessage = chat.LastMessage;

            Members.Clear();
            foreach (var member in chat.Members)
                Members.Add(member);
        }
    }
}
