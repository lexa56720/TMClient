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
        public override bool IsHaveImage => user.IsHaveImage;
        public override bool IsCurrentUser => false;

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

        public override DateTime LastAction
        {
            get => user.LastAction;
            set
            {
                user.LastAction = value;
                OnPropertyChanged(nameof(LastAction));
            }
        }

        public override string ImageLarge
        {
            get => user.ImageLarge;
            set
            {
                user.ImageLarge = value;
                OnPropertyChanged(nameof(ImageLarge));
            }
        }
        public override string ImageMedium
        {
            get => user.ImageMedium;
            set
            {
                user.ImageMedium = value;
                OnPropertyChanged(nameof(ImageMedium));
            }
        }
        public override string ImageSmall
        {
            get => user.ImageSmall;
            set
            {
                user.ImageSmall = value;
                OnPropertyChanged(nameof(ImageSmall));
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
            OnPropertyChanged(nameof(LastAction));

            OnPropertyChanged(nameof(ImageLarge));
            OnPropertyChanged(nameof(ImageMedium));
            OnPropertyChanged(nameof(ImageSmall));
            OnPropertyChanged(nameof(IsHaveImage));
        }
    }
}
