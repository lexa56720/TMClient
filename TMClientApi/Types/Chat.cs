using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClientApi.Types
{
    public class Chat : INotifyPropertyChanged
    {
        public int Id { get; }

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

        public ObservableCollection<User> Members { get; }

        public Chat(int id, string name, User[] users)
        {
            Id = id;
            Name = name;
            Members = new ObservableCollection<User>(users);
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void Update(Chat chat)
        {
            Name = chat.name;
            WritingUser = chat.WritingUser;
            LastMessage = chat.LastMessage;

            Members.Clear();
            foreach (var member in chat.Members)
                Members.Add(member);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
