using ApiTypes.Communication.Users;
using ApiWrapper.Types;
using System.Collections.ObjectModel;

namespace ApiWrapper.Interfaces
{
    public interface IApi : IUserInfo, IDisposable
    {
        public IUsersApi Users { get; }

        public IMessagesApi Messages { get; }

        public IChatsApi Chats { get; }

        public IFriendsApi Friends { get; }

        public Task Save(string path);
    }
}
