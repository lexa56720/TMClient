using ApiTypes.Communication.Chats;
using System.ComponentModel;

namespace ApiWrapper.Types
{
    public class User : INotifyPropertyChanged
    {
        public virtual int Id { get; }

        public virtual string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string name = string.Empty;

        public virtual string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        private string login = string.Empty;

        public virtual bool IsOnline
        {
            get => isOnline;
            set
            {
                isOnline = value;
                OnPropertyChanged(nameof(IsOnline));
            }
        }
        private bool isOnline = false;


        public event PropertyChangedEventHandler? PropertyChanged;

        public User(int id, string name, string login, bool isOnline)
        {
            Id = id;
            Name = name;
            Login = login;
            IsOnline = isOnline;
        }

        protected User()
        {

        }

        public virtual void Update(User user)
        {
            if (user == this)
                return;
            IsOnline = user.IsOnline;
            Name = user.Name;
            Login = user.Login;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
