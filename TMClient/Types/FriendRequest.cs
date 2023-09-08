using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class FriendRequest
    {
        public required User From { get; init; }
    }
}
