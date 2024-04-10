using ApiTypes.Communication.Chats;
using ClientApiWrapper.Types;
using System.ComponentModel;

namespace ClientApiWrapper.Types
{
    public class User : NamedImageEntity
    {
        public virtual bool IsCurrentUser { get; }

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

        public User(int id, string name, string login, bool isOnline, bool isCurrentUser,
            DateTime lastAction, string? picLarge, string? picMedium, string? picSmall) : base(id, name, picLarge, picMedium, picSmall)
        {
            Login = login;
            IsOnline = isOnline;
            IsCurrentUser = isCurrentUser;
            LastAction = lastAction;
        }
        public User(int id, string name, string login, bool isOnline, bool isCurrentUser, DateTime lastAction): base(id, name)
        {
            Login = login;
            IsOnline = isOnline;
            IsCurrentUser = isCurrentUser;
            LastAction = lastAction;
        }

        protected User():base()
        {

        }

        public virtual void Update(User user)
        {
            if (user == this)
                return;
            IsOnline = user.IsOnline;
            Login = user.Login;
            base.Update(user);
        }
    }
}
