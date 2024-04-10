using ApiTypes.Communication.Users;
using ClientApiWrapper.Types;
using ClientApiWrapper.Interfaces;
using System.Collections.ObjectModel;

namespace ClientApiWrapper.Interfaces
{
    public interface IApi : IUserInfo, IDisposable
    {
        public IUsersApi Users { get; }

        public IMessagesApi Messages { get; }

        public IChatsApi Chats { get; }

        public IFriendsApi Friends { get; }

        public IDataValidator DataValidator { get; }

        public Task Save(string path);
    }
}
