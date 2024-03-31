using ApiTypes.Communication.Chats;
using ClientApiWrapper.Types;
using System.ComponentModel;

namespace ApiWrapper.Types
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
                    DateTime lastAction, string? picLarge, string? picMedium, string? picSmall)
        {
            Id = id;
            Name = name;
            Login = login;
            IsOnline = isOnline;
            IsCurrentUser = isCurrentUser;
            LastAction = lastAction;
            ImageLarge = picLarge;
            ImageMedium = picMedium;
            ImageSmall = picSmall;
            if (picLarge == null || picMedium == null || picSmall == null)
                IsHaveImage = false;
            else
                IsHaveImage = true;
        }
        public User(int id, string name, string login, bool isOnline, bool isCurrentUser, DateTime lastAction)
        {
            Id = id;
            Name = name;
            Login = login;
            IsOnline = isOnline;
            IsCurrentUser = isCurrentUser;
            LastAction = lastAction;
            ImageLarge = null;
            ImageMedium = null;
            ImageSmall = null;
            IsHaveImage = false;
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
            ImageLarge = user.ImageLarge;
            ImageMedium = user.ImageMedium;
            ImageSmall = user.ImageSmall;
            IsHaveImage=user.IsHaveImage;
            LastAction = user.LastAction;
        }
    }
}
