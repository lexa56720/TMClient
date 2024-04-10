using ApiTypes.Communication.Users;
using ClientApiWrapper.Types;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ClientApiWrapper.Types
{
    public class Chat : NamedImageEntity
    {
        public bool IsDialogue { get; }

        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
        private bool isReadOnly = false;

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

        public User Admin
        {
            get => admin;
            set
            {
                admin = value;
                OnPropertyChanged(nameof(Admin));
            }
        }
        private User admin = null!;

        public ObservableCollection<User> Members { get; } = new();

        public Chat(int id, string name, User admin, int unreadCount, bool isDialogue,
             string? picLarge, string? picMedium, string? picSmall) : base(id, name, picLarge, picMedium, picSmall)
        {
            IsDialogue = isDialogue;
            Admin = admin;
            UnreadCount = unreadCount;
        }
        public Chat(int id, string name, User admin, int unreadCount, bool isDialogue):base(id,name)
        {
            IsDialogue = isDialogue;
            Admin = admin;
            UnreadCount = unreadCount;
        }
        internal void Update(Chat chat)
        {
            if (chat == this)
                return;
            WritingUser = chat.WritingUser;
            LastMessage = chat.LastMessage;
            base.Update(chat);

            Members.Clear();
            foreach (var member in chat.Members)
                Members.Add(member);
        }
    }
}
