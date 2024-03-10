using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApiWrapper.Types
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
        private Chat dialogue;

        public Friend(User user, Chat dialogue) : base(user.Id, user.Name, user.Login, user.IsOnline)
        {
            Dialogue = dialogue;
        }
    }
}
