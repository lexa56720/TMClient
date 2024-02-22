using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWrapper.Types
{
    public class ChatInvite
    {
        public int Id { get; }
        public User Inviter { get; }
        public Chat Chat { get;}

        public ChatInvite(int id, User inviter, Chat chat)
        {
            Inviter = inviter;
            Chat = chat;
            Id = id;
        }
    }
}
