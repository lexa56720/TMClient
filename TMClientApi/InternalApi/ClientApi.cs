using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClientApi.InternalApi
{
    public class ClientApi : IApi
    {
        public IUsersApi Users => throw new NotImplementedException();

        public IMessagesApi Messages => throw new NotImplementedException();

        public IAuthApi Auth => throw new NotImplementedException();

        public IChatsApi Chats => throw new NotImplementedException();

        public IFriendsApi Friends => throw new NotImplementedException();

        private ClientApiConverter Converter { get; set; }
        private CacheManager Cache { get; set; }

        public ClientApi()
        {
        }

        public Task Save(string path)
        {
            throw new NotImplementedException();
        }

        public static IApi Load(string path)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
