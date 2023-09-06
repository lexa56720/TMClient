using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    internal class FriendRequest
    {
        public required User From { get; init; }

        public required DateTime Date { get; init; }

    }
}
