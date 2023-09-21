using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class ChatInvite
    {
        public required int Id { get; init; }
        public required User Inviter { get; init; }

        public required Chat Chat { get; init; }

        [SetsRequiredMembers]
        public ChatInvite(User inviter, Chat chat, int id)
        {
            Inviter = inviter;
            Chat = chat;
            Id = id;
        }

        public ChatInvite() { }
    }
}
