using ApiTypes.Communication.Chats;
using System.ComponentModel;

namespace ApiWrapper.Types
{
    public class User : INotifyPropertyChanged
    {
        public virtual int Id { get; }

        public virtual bool IsCurrentUser { get; }

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

        public virtual DateTime LastAction
        {
            get => lastAction; 
            set 
            {
                lastAction = value;
                OnPropertyChanged(nameof(LastAction));
            }
        }
        private DateTime lastAction;

        public virtual string ProfilePicLarge
        {
            get => profilePicLarge;
            set
            {
                profilePicLarge = value;
                OnPropertyChanged(nameof(ProfilePicLarge));
            }
        }
        private string profilePicLarge;

        public virtual string ProfilePicMedium
        {
            get => profilePicMedium;
            set
            {
                profilePicMedium = value;
                OnPropertyChanged(nameof(ProfilePicMedium));
            }
        }
        private string profilePicMedium;

        public virtual string ProfilePicSmall
        {
            get => profilePicSmall;
            set
            {
                profilePicSmall = value;
                OnPropertyChanged(nameof(ProfilePicSmall));
            }
        }
        private string profilePicSmall;

        public event PropertyChangedEventHandler? PropertyChanged;

        public User(int id, string name, string login, bool isOnline, bool isCurrentUser,
                    DateTime lastAction,string picLarge, string picMedium,string picSmall)
        {
            Id = id;
            Name = name;
            Login = login;
            IsOnline = isOnline;
            IsCurrentUser = isCurrentUser;
            LastAction = lastAction;
            ProfilePicLarge = picLarge;
            ProfilePicMedium = picMedium;
            ProfilePicSmall = picSmall;
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
            ProfilePicLarge = user.ProfilePicLarge;
            ProfilePicMedium = user.ProfilePicMedium;
            ProfilePicSmall = user.ProfilePicSmall;
            LastAction=user.LastAction;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
