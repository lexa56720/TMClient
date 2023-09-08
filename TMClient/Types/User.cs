using ApiTypes.Communication.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class User : INotifyPropertyChanged
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

        public required bool IsOnline
        {
            get => isOnline;
            set
            {
                isOnline = value;
                OnPropertyChanged(nameof(IsOnline));
            }
        }
        private bool isOnline = false;

        [SetsRequiredMembers]
        public User(ApiTypes.Communication.Users.User user)
        {
            Id = user.Id;
            Name = user.Name;
            IsOnline = user.IsOnline;
        }
        public User()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
