using ApiTypes.Shared;
using ClientApiWrapper.Types;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMClient.Controls;
using TMClient.Utils;

namespace TMClient.Model.Chats
{
    internal class ChatModel : BaseModel
    {
        protected Chat Chat { get; }
        public int Count { get; }

        public ChatModel(Chat chat, int count)
        {
            Chat = chat;
            Count = count;
        }

        //Получение сообщений относительно другого
        public async Task<Message[]> GetHistory(Message lastMessage)
        {
            return await Api.Messages.GetMessages(Chat.Id, lastMessage.Id, Count);
        }
        //Получение сообщений со смещением
        public async Task<Message[]> GetHistory(int offset)
        {
            return await Api.Messages.GetMessagesByOffset(Chat.Id, Count, offset);
        }

        //Пометить сообщения как прочтённые
        public async Task<bool> MarkAsReaded(Message[] messages)
        {
            return await Api.Messages.MarkAsReaded(messages.Where(m => !m.IsReaded)
                                                           .ToArray());
        }

        //Отправка сообщений
        public async Task<Message?> SendMessage(string text, IEnumerable<string> filePaths)
        {
            var paths= filePaths.ToArray();
            if (paths.Length == 0)
                return await Api.Messages.SendMessage(text, Chat.Id);

            using var tks = new CancellationTokenSource();
            return await Api.Messages.SendMessage(text, Chat.Id, tks.Token, paths);
        }

        //Установка флага IsReaded в сообщениях
        public void SetIsReaded(IEnumerable<Message> messages)
        {
            if (messages.Count() == 0)
                return;
            App.MainThread.Invoke(() =>
            {
                foreach (var message in messages)
                {
                    message.IsReaded = true;
                }
            });
        }

        public bool IsMessageValid(string text, params string[] filePaths)
        {
            return Api.DataValidator.IsMessageLegal(text, filePaths);
        }
    }
}
