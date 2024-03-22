using System.Text;

namespace TMClient.Model.Chats
{
    internal class ChatModel: BaseModel
    {
        protected Chat Chat { get; }
        public int Count { get; }

        public ChatModel(Chat chat,int count)
        {
            Chat = chat;
            Count = count;
        }

        public async Task<Message[]> GetHistory(Message lastMessage)
        {
            return await Api.Messages.GetMessages(Chat.Id, lastMessage.Id, Count);
        }
        public async Task<Message[]> GetHistory(int offset)
        {
            return await Api.Messages.GetMessagesByOffset(Chat.Id, Count, offset);
        }
        public async Task<Message?> SendMessage(Message message)
        {
            return await Api.Messages.SendMessage(message.Text, message.Destination.Id);
        }

        public async Task<bool> MarkAsReaded(Message[] messages)
        {
            return await Api.Messages.MarkAsReaded(messages.Where(m=>!m.IsReaded)
                                                           .ToArray());
        }

    }
}
