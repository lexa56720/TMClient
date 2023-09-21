using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Types
{
    public class FriendRequest
    {
        public required int Id { get; init; }
        public required User From { get; init; }

        public FriendRequest() { }

        [SetsRequiredMembers]
        public FriendRequest(User from,int id)
        {
            From = from;
            Id= id;
        }
    }
}
