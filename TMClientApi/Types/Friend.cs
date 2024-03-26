using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWrapper.Types
{
    public class Friend : User
    {
        public Chat Dialogue
        {
            get => dialogue;
            private set
            {
                dialogue = value;
                OnPropertyChanged(nameof(Dialogue));
            }
        }
        private Chat dialogue = null!;

        public override int Id => user.Id;

        public override string Name
        {
            get => user.Name;
            set
            {
                user.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public override string Login
        {
            get => user.Login;
            set
            {
                user.Login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public override bool IsOnline
        {
            get => user.IsOnline;
            set
            {
                user.IsOnline = value;
                OnPropertyChanged(nameof(IsOnline));
            }
        }


        public override string ProfilePicLarge
        {
            get => user.ProfilePicLarge;
            set
            {
                user.ProfilePicLarge = value;
                OnPropertyChanged(nameof(ProfilePicLarge));
            }
        }
        public override string ProfilePicMedium
        {
            get => user.ProfilePicMedium;
            set
            {
                user.ProfilePicMedium = value;
                OnPropertyChanged(nameof(ProfilePicMedium));
            }
        }
        public override string ProfilePicSmall
        {
            get => user.ProfilePicSmall;
            set
            {
                user.ProfilePicSmall = value;
                OnPropertyChanged(nameof(ProfilePicSmall));
            }
        }


        private User user;

        public Friend(User user, Chat dialogue) : base()
        {
            this.user = user;
            Dialogue = dialogue;
        }


        public override void Update(User user)
        {
            if (this.user == user)
                return;
            this.user = user;
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(IsOnline));

            OnPropertyChanged(nameof(ProfilePicLarge));
            OnPropertyChanged(nameof(ProfilePicMedium));
            OnPropertyChanged(nameof(ProfilePicSmall));
        }
    }
}
