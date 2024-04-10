namespace ClientApiWrapper.Types
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
