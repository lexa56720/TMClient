using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class Chat : INotifyPropertyChanged
    {
        public required int Id { get; init; }

        public required string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string name = string.Empty;

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

        public ObservableCollection<User> Members { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        [SetsRequiredMembers]
        public Chat(ApiTypes.Communication.Chats.Chat chat, User[] users)
        {
            Id = chat.Id;
            Name = chat.Name;
            Members = new ObservableCollection<User>(users);
        }
        public Chat()
        {
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
