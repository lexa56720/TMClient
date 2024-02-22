using ApiTypes.Communication.BaseTypes;
using ApiTypes.Communication.Search;
using ApiTypes.Shared;
using ApiTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTypes.Communication.Chats;
using TMClientApi.Types;
using ApiTypes.Communication.Auth;
using ApiTypes.Communication.Users;
using System.Collections.ObjectModel;
using TMApi.ApiRequests.Friends;

namespace TMClientApi.InternalApi
{
    public interface IApi : IDisposable
    {
        public IUsersApi Users { get; }

        public IMessagesApi Messages { get; }

        public IChatsApi Chats { get; }

        public IFriendsApi Friends { get; }


        public ObservableCollection<Chat> Dialogs { get; }
        public ObservableCollection<Chat> MultiuserChats { get; }
        public ObservableCollection<User> FriendList { get; }
        public ObservableCollection<FriendRequest> FriendRequests { get; }
        public ObservableCollection<ChatInvite> ChatInvites { get; }

        public UserInfo CurrentUser { get; }

        public Task Save(string path);
    }

    public interface IFriendsApi
    {
        public Task<FriendRequest?> GetFriendRequest(int requestId);
        public Task<FriendRequest[]> GetFriendRequest(int[] requestIds);
        public Task<FriendRequest[]> GetAllRequests();
        public Task<bool> ResponseFriendRequest(int requestId, bool isAccepted);
        public Task<bool> SendFriendRequest(int toId);
    }

    public interface IChatsApi
    {
        public Task<Chat?> CreateChat(string name, int[] membersId);
        public ValueTask<Chat?> GetChat(int chatId);
        public ValueTask<Chat[]> GetChat(int[] chatIds);
        public Task<Chat[]> GetAllChats();
        public Task<bool> SendChatInvite(int chatId, int toUserId);
        public Task<ChatInvite?> GetChatInvite(int inviteId);
        public Task<ChatInvite[]> GetChatInvite(int[] inviteId);
        public Task<ChatInvite[]> GetAllInvites(int userId);
        public Task<bool> SendChatInviteResponse(int inviteId, bool isAccepted);
    }

    public interface IMessagesApi
    {
        public Task<Message[]> GetMessages(int chatId, int count, int offset);
        public Task<Message[]> GetMessages(int chatId, int fromMessageId);
        public Task<Message[]> GetMessages(params int[] messagesId);
        public Task<Message?> SendMessage(string text, int destinationId);
    }

    public interface IUsersApi
    {
        public Task<bool> ChangeName(string name);
        public ValueTask<User?> GetUser(int userId);
        public ValueTask<User[]> GetUser(int[] userId);
        public Task<User[]> GetByName(string name);
    }
}
