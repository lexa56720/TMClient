global using ApiMessage = ApiTypes.Communication.Messages.Message;
global using ApiUser = ApiTypes.Communication.Users.User;
global using ApiChat = ApiTypes.Communication.Chats.Chat;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Types;
using TMApi.ApiRequests.Messages;

namespace TMClient.Utils
{
    static class ApiConverter
    {
        public static async ValueTask<Message[]> Convert(UserDataStorage storage, params ApiMessage[] messages)
        {
            var authorsId = messages.Select(m => m.AuthorId).ToArray();
            var authors = await storage.GetUser(authorsId);

            var destId = messages.Select(m => m.DestinationId).ToArray();
            var destinations = await storage.GetChat(destId);

            var result = new Message[messages.Length];
            for (int i = 0; i < messages.Length; i++)
                result[i] = new Message(messages[i], authors[i], destinations[i]);

            return result;
        }

        public static User[] Convert(params ApiUser[] users)
        {
            return users.Select(u => new User(u)).ToArray();
        }
        public static async ValueTask<Chat[]> Convert(UserDataStorage storage, params ApiChat[] chats)
        {
            var membersId = chats.SelectMany(c => c.MemberIds).ToArray();
            var members = await storage.GetUser(membersId);

            var result = new Chat[chats.Length];
            for (int i = 0,fromIndex=0; i < chats.Length; i++)
            {
                var toIndex = (fromIndex + chats[i].MemberIds.Length);
                result[i] = new Chat(chats[i], members[fromIndex..toIndex]);
                fromIndex = toIndex;
            }

            return result;           
        }
    }
}
