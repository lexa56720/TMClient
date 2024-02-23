using System.Text;

namespace TMClient.Model.Chats
{
    internal abstract class BaseChatModel: BaseModel
    {
        protected Chat Chat { get; }

        public BaseChatModel(Chat chat)
        {
            Chat = chat;
        }

        public async Task<Message[]> GetHistory(Message lastMessage)
        {
            return await Api.Messages.GetMessages(Chat.Id, lastMessage.Id);
   
        }
        public async Task<Message[]> GetHistory(int offset)
        {
            return await Api.Messages.GetMessages(Chat.Id, 20, offset);
        }

        public async Task<Message?> SendMessage(Message message)
        {
            return await Api.Messages.SendMessage(message.Text, message.Destionation.Id);
        }

    }
}
