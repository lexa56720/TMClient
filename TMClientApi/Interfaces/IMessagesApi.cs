using ClientApiWrapper.Types;

namespace ClientApiWrapper.Interfaces
{
    public interface IMessagesApi
    {
        public Task<Message[]> GetMessagesByOffset(int chatId, int count, int offset);
        public Task<Message[]> GetMessages(int chatId, int fromMessageId,int count);
        public Task<Message[]> GetMessages(params int[] messageIds);

        public Task<Message?> SendMessage(string text, int destinationId, CancellationToken token, string[] filePaths);
        public Task<Message?> SendMessage(string text, int destinationId);
        public Task<bool> MarkAsReaded(params Message[] messages);
    }
}
