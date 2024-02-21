using ApiTypes.Communication.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClientApi.Types
{
    public class User : INotifyPropertyChanged
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

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        private string login = string.Empty;

        public bool IsOnline
        {
            get => isOnline;
            set
            {
                isOnline = value;
                OnPropertyChanged(nameof(IsOnline));
            }
        }
        private bool isOnline = false;

        public User(int id, string name, string login, bool isOnline)
        {
            Id = id;
            Name = name;
            Login = login;
            IsOnline = isOnline;
        }

        public void Update(User user)
        {
            IsOnline = user.IsOnline;
            Name = user.Name;
            Login = user.Login;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
