using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClientApi.InternalApi;

namespace TMClientApi.ApiWrapper
{
    public class ClientApi : IApi
    {
        public IUsersApi Users => users;
        private ClientUsersApi users;

        public IMessagesApi Messages => messages;
        private ClientMessagesApi messages;

        public IFriendsApi Friends => friends;
        private ClientFriendsApi friends;

        public IAuthApi Auth => throw new NotImplementedException();
        public IChatsApi Chats => throw new NotImplementedException();

        private ApiConverter Converter { get; set; }
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
