using System.Diagnostics.CodeAnalysis;

namespace ApiWrapper.Types
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
