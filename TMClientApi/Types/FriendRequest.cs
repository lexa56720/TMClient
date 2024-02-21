using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClientApi.Types
{
    public class FriendRequest
    {
        public int Id { get; }
        public User From { get; }

        public FriendRequest() { }

        [SetsRequiredMembers]
        public FriendRequest(int id, User from)
        {
            From = from;
            Id = id;
        }
    }
}
