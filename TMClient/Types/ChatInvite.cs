using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class ChatInvite
    {
        public required User Inviter { get; init; }

        public required Chat Chat { get; init; }
    }
}
